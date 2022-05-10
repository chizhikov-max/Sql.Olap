using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using Dapper;
using Sql.Olap.Common;

namespace Sql.Olap.DataExtractors
{
    public class BulkDataExtractor<T> : IDataExtractor
    {
        public BulkDataExtractor(Config<T> config)
        {
            this.config = config;
        }

        private List<T> items;
        private readonly Config<T> config;

        public void ReadData()
        {
            items = config.DataReader().ToList();
        }

        public void InsertData()
        {
            using (var bulkCopy = new SqlBulkCopy(config.ConnectionString))
            {
                bulkCopy.BulkCopyTimeout = 0;
                bulkCopy.DestinationTableName = $"{config.UniqueName}";
                var dataTable = new DataTable(config.Name);
                dataTable.Columns.AddRange(GetDataColumns());
                foreach (var item in items)
                {
                    var values = GetPropertyValues(item);
                    dataTable.Rows.Add(values);
                }
                bulkCopy.WriteToServer(dataTable);
            }

            using (var connection = new SqlConnection(config.ConnectionString))
            {
                bool isColumnIdExists;
                connection.Open();

                isColumnIdExists =
                    Convert.ToBoolean(connection.ExecuteScalar(
                        $"select iif(exists(select 1 from sys.syscolumns where name = 'AGENTNAME' and id = object_id('{config.UniqueName}')),1,0)"));
                if (isColumnIdExists)
                {
                    connection.Execute($@"update {config.UniqueName} set AGENTNAME=replace(ltrim(rtrim(replace(AGENTNAME, char(9), '    '))),'    ', char(9))", 
                    commandTimeout: 0);
                }

                isColumnIdExists =
                    Convert.ToBoolean(connection.ExecuteScalar(
                        $"select iif(exists(select 1 from sys.syscolumns where name = 'DESCRIPTION' and id = object_id('{config.UniqueName}')),1,0)"));
                if (isColumnIdExists)
                {
                    connection.Execute($@"update {config.UniqueName} set DESCRIPTION=replace(replace(replace(ltrim(rtrim(replace([DESCRIPTION], char(9), '    '))),'    ', char(9)), char(10),''),char(13),'')",
                        commandTimeout: 0);
                }

                isColumnIdExists =
                    Convert.ToBoolean(connection.ExecuteScalar(
                        $"select iif(exists(select 1 from sys.syscolumns where name = 'Id' and id = object_id('{config.UniqueName}')),1,0)"));
                if (!isColumnIdExists)
                {
                    string sql = $@"alter table {config.UniqueName} add Id int not null identity primary key";
                    connection.Execute(sql, commandTimeout: 0);
                }
            }
        }

        public bool CreateTable()
        {
            using (var connection = new SqlConnection(config.ConnectionString))
            {
                string sql = String.Empty;
                connection.Open();
                bool isTableExists =
                    Convert.ToBoolean(connection.ExecuteScalar(
                        $"select iif(exists(select 1 from sys.objects where object_id = object_id('{config.UniqueName}') and type = 'U'),1,0)"));
                if (isTableExists)
                {
                    sql = $@"drop table {config.UniqueName}";
                    connection.Execute(sql);
                }
                sql = $@"create table {config.UniqueName}({string.Join(", ", GetColumns())})";
                connection.Execute(sql);
                return true;
            }
        }

        public void ClearObsoleteTables()
        {
            using (var connection = new SqlConnection(config.ConnectionString))
            {
                connection.Open();
                var objects = connection.Query("select name from sysobjects where type = 'U'");
                foreach (var obj in objects)
                {
                    string name = obj.name;
                    var parts = name.Split(new[] {'_'}, StringSplitOptions.RemoveEmptyEntries);
                    var length = parts.Length;
                    if (length > 3)
                    {
                        string dateStr = $"{parts[length - 3]}_{parts[length - 2]}_{parts[length - 1]}";
                        DateTime date = DateTime.Now;
                        if (DateTime.TryParseExact(dateStr, "yyyyMMdd_HHmmss_fff", null, DateTimeStyles.None, out date) &&
                            (DateTime.Now - date).TotalMinutes > config.ActiveDataTimeout)
                        {
                            connection.Execute($"drop table {name}");
                        }
                    }
                }
            }
        }

        public void TruncateData()
        {
            using (var connection = new SqlConnection(config.ConnectionString))
            {
                connection.Open();
                var sql = $@"truncate table {config.UniqueName}";
                connection.Execute(sql);
            }
        }

        private List<string> GetColumns()
        {
            var properties = typeof(T).GetProperties();
            var result = new List<string>();
            foreach (var property in properties)
            {
                var sqlDbType = SqlHelper.GetDbType(property.PropertyType);
                var isNullable = Nullable.GetUnderlyingType(property.PropertyType) != null;
                string nullable = isNullable ? "null" : "not null";
                string propertyType = sqlDbType.ToString();
                if (propertyType == SqlDbType.NVarChar.ToString()) propertyType += "(max)";
                result.Add($"[{property.Name}] {propertyType} {nullable}");
            }
            return result;
        }

        private DataColumn[] GetDataColumns()
        {
            var dataColumns = new List<DataColumn>();
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var isNullable = Nullable.GetUnderlyingType(property.PropertyType) != null;
                var dataColumn = new DataColumn(property.Name)
                {
                    AllowDBNull = isNullable,
                    DataType = Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType
                };
                dataColumns.Add(dataColumn);
            }
            return dataColumns.ToArray();
        }

        private object[] GetPropertyValues(T item)
        {
            var objects = new List<object>();
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var value = property.GetValue(item, null);
                objects.Add(value);
            }
            return objects.ToArray();
        }
    }
}