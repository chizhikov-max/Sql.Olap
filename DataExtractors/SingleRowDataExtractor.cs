using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Sql.Olap.Common;

namespace Sql.Olap.DataExtractors
{
    public class SingleRowDataExtractor<T> :IDataExtractor
    {
        public SingleRowDataExtractor(Config<T> config)
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
            using (var connection = new SqlConnection(config.ConnectionString))
            {
                var parameters = items.Select(GetParameters).ToList();
                connection.Open();
                var properties = typeof(T).GetProperties();
                var fields = string.Join(",", properties.Select(x => x.Name));
                var pFields = string.Join(",", properties.Select(x => $"@{x.Name}"));
                var sql = $@"insert into {config.Name}(Constract,{fields}) values(@Constract,{pFields})";
                connection.Execute(sql, parameters);
            }
        }

        public bool CreateTable()
        {
            using (var connection = new SqlConnection(config.ConnectionString))
            {
                connection.Open();
                bool isTableExists =
                    Convert.ToBoolean(connection.ExecuteScalar(
                        $"select iif(exists(select 1 from sys.objects where object_id = object_id('{config.Name}') and type = 'U'),1,0)"));
                if (!isTableExists)
                {
                    string sql = $@"
create table {config.Name}(
    Id int not null identity primary key,
    Constract datetime not null, {string.Join(", ", GetColumns()) }
)";
                    connection.Execute(sql);
                    return true;
                }
                return false;
            }
        }

        public void TruncateData()
        {
            using (var connection = new SqlConnection(config.ConnectionString))
            {
                connection.Open();
                var sql = $@"truncate table {config.Name}";
                connection.Execute(sql);
            }
        }

        public void ClearObsoleteTables()
        {
            
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

        private DynamicParameters GetParameters(T item)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Constract", config.Constract);
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var value = property.GetValue(item);
                if (value is string) value = ((string) value).Trim();
                parameters.Add($"@{property.Name}", value);
            }
            return parameters;
        }
    }
}
