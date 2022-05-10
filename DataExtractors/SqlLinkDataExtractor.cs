using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Oper.Sql.Common;

namespace Oper.Sql.DataExtractors
{
    public class SqlLinkDataExtractor<T> : IDataExtractor
    {
        public SqlLinkDataExtractor(Config<T> config)
        {
            this.config = config;
        }

        private readonly Config<T> config;

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
    {string.Join(", ", GetColumns()) }
)";
                    connection.Execute(sql);
                    return true;
                }
                return false;
            }
        }

        public void ReadData()
        {
            config.DataReader();
        }

        public void InsertData()
        {
            using (var connection = new SqlConnection(config.ConnectionString))
            {
                connection.Open();
                var properties = typeof(T).GetProperties();
                var fields = string.Join(",", properties.Select(x => x.Name));

                //var sql = $@"insert into {config.Name}({fields}) select {fields} from FBALL...E2_KAP_PF_PER_00";
                var sql = $@"insert into {config.Name}({fields}) select {fields} from FBALL...E2_KAP_PF_PER_00";
                connection.Execute(sql, commandTimeout: 3600);
                sql = $@"update {config.Name} set AGENTNAME=replace(ltrim(rtrim(replace(AGENTNAME, char(9), '    '))),'    ', char(9))";
                connection.Execute(sql, commandTimeout: 3600);
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
    }
}