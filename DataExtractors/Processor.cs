using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Dapper;
using FirebirdSql.Data.FirebirdClient;
using Newtonsoft.Json;
using Sql.Olap.Common;
using Sql.Olap.Models;

namespace Sql.Olap.DataExtractors
{
    public interface IProcessor
    {
        void Execute();
        IEnumerable<T> GetData<T>();
    }

    public class Processor<In, Out> : IProcessor where In : InParametersBase, new()
        where Out : OutParametersBase
    {
        public Processor(ConfigRecalc<In, Out> config)
        {
            this.config = config;
            LoadFirms();
        }

        private readonly ConfigRecalc<In, Out> config;
        private readonly List<Firm> firms = new List<Firm>();

        public IEnumerable<T> GetData<T>()
        {
            string tableName = $"T_{config.Name}".ToUpper();
            using (var connection = new FbConnection(config.FbConnectionString))
            {
                connection.Open();
                var whereClause = GetWhereClause();
                var items = connection.Query<T>($"select * from {tableName} {whereClause}", config.InputParameters);
                return items;
            }
        }

        public void Execute()
        {
            CheckBeforeRun();
            CreateIfNotExistsTable();
            if (config.InputParameters.Recalc)
            {
                Recalc();
            }
            else if (config.InputParameters.STDM == STDMTypeList.Dynamic)
            {
                firms.ForEach(FillDynamicCalculatedDate);
                if (firms.Any(x => x.DynamicCalculatedDate == null))
                {
                    Recalc();
                    return;
                }

                foreach (var firm in firms)
                {
                    FillActualDate(firm);
                    if (firm.ActualDate.HasValue && firm.DynamicCalculatedDate >= firm.ActualDate)
                    {
                        Recalc(firm);
                    }
                }
            }
        }

        private void FillActualDate(Firm firm)
        {
            using (var connection = new FbConnection(firm.ConnectionString))
            {
                connection.Open();
                var sql = @"select min(coalesce(D.MOVEDATE, D.DOCDATE))
                            from AUDIT A, DOCUMENT D
                            where (A.OID = D.DOCID) and
                                  (A.OPERTYPE between 8 and 64) and
                                  (A.SYSDATE > @Date)";
                firm.ActualDate = connection.ExecuteScalar<DateTime?>(sql, new { Date = firm.DynamicCalculatedDate });
            }
        }

        private void FillDynamicCalculatedDate(Firm firm)
        {
            using (var connection = new FbConnection(config.FbConnectionString))
            {
                connection.Open();
                var rows = connection.Query($"select * from OLAP_AUDIT where FIRM=@Firm and REPORT=@ReportName and HASH=@Hash",
                    new { Firm = firm.Id, ReportName = config.Name, config.InputParameters.Hash });
                DateTime? dynamicCalculatedDate = (DateTime?)rows.FirstOrDefault()?.DMDATE;
                firm.DynamicCalculatedDate = dynamicCalculatedDate;
            }
        }

        private void LoadFirms()
        {
            firms.Clear();
            using (var connection = new FbConnection(config.FbConnectionString))
            {
                connection.Open();
                var sql = "select * from FIRMS where (OK=1)";
                if (config.InputParameters.Firm == "0") sql += " and (AL=1)";
                else sql += " and (@Firm containing '~' || ID || '~')";
                var items = connection.Query<Firm>(sql, config.InputParameters);
                firms.AddRange(items);
            }
        }

        private void CheckBeforeRun()
        {
            var properties = ReflectionHelper.GetPropertiesWithAttribute(config.InputParameters.GetType(), typeof(FilterAttribute)).ToList();

            #region Проверка на наличие значений по-умолчанию для Входных параметров

            foreach (var info in properties)
            {
                if (!config.InputParameters.CheckExistsDefaultValue(info.Name))
                    throw new Exception($"Не задано значение по-умолчанию для входного параметра [{info.Name}]");
            }

            #endregion

            #region Проверка на наличие условия фильтрации для Входных параметров

            foreach (var info in properties)
            {
                if (!config.InputParameters.CheckExistsWhereClause(info.Name))
                    throw new Exception($"Не задано условие фильтрации для входного параметра [{info.Name}]");
            }

            #endregion
        }

        private void CreateIfNotExistsTable()
        {
            using (var connection = new FbConnection(config.FbConnectionString))
            {
                connection.Open();
                var rows = connection.Query($"SELECT * FROM RDB$RELATIONS WHERE RDB$RELATION_NAME = upper('T_{config.Name}')");
                if (!rows.Any())
                {
                    var sql = $@"create table T_{config.Name}({string.Join(", ", GetColumns())})";
                    connection.Execute(sql.ToUpper());
                    sql = $"CREATE INDEX IDX_T_{config.Name}_HASHSTDM ON T_{config.Name} (HASH, STDM)";
                    connection.Execute(sql.ToUpper());
                }
            }
        }

        private List<string> GetColumns()
        {
            var properties = typeof(Out).GetProperties();
            var result = new List<string>(new[] { "HASH varchar(512) not null", "STDM INTEGER not null" });
            foreach (var property in properties)
            {
                var fbDbType = FbHelper.GetDbType(property.PropertyType);
                var isNullable = Nullable.GetUnderlyingType(property.PropertyType) != null;
                string nullable = isNullable ? "null" : "not null";
                string propertyType = fbDbType.ToDbString();
                result.Add($"{property.Name} {propertyType}");
            }
            return result;
        }

        private void Recalc(Firm firm = null)
        {
            try
            {
                SetRecalMode(true);
                ClearOldData(firm);
                FillData(firm);
                UpdateOlapAudit(firm);
            }
            finally
            {
                SetRecalMode(false);
            }
        }

        private void SetRecalMode(bool mode)
        {
            string modeName = $"RECALC_{config.Name}".ToUpper();
            using (var connection = new FbConnection(config.FbConnectionString))
            {
                connection.Open();
                var setting = connection.Query("select * from E_CONSTS where NAME=@Name", new {Name = modeName}).FirstOrDefault();
                if (setting == null)
                {
                    connection.Execute(
                        "insert into E_CONSTS(NAME, CVALUE, COMMENTS) values (@Name, @Value, @Comment)",
                        new {Name = modeName, Value  = mode ? 1 : 0, Comment = $"Пересчет {config.Name}"});
                }
                else
                {
                    connection.Execute(
                        "update E_CONSTS set CVALUE=@Value where NAME=@Name",
                        new { Name = modeName, Value = mode ? 1 : 0 });
                }
            }
        }

        private void ClearOldData(Firm firm)
        {
            string tableName = $"T_{config.Name}".ToUpper();
            using (var connection = new FbConnection(config.FbConnectionString))
            {
                connection.Open();
                if (firm == null)
                    connection.Execute($"delete from {tableName} where HASH=@Hash", new { config.InputParameters.Hash });
                else
                    connection.Execute($"delete from {tableName} where HASH=@Hash and MOVEDATE >= @Date and STDM=@STDM and FIRM=@Firm",
                        new { config.InputParameters.Hash, Date = firm.ActualDate, STDM = (int)STDMTypeList.Dynamic, Firm = firm.Id });
            }
        }

        private void FillData(Firm firm)
        {
            string hash = config.InputParameters.Hash;
            string tableName = $"T_{config.Name}".ToUpper();
            var insertClause = GetInsertClause();
            var fromClause = GetFromClause();

            using (var connection = new FbConnection(config.FbConnectionString))
            {
                connection.Open();
                if (firm == null)
                {
                    var selectClause = GetSelectClause(STDMTypeList.Static);
                    string sql = $@"
                        {insertClause}
                        {selectClause} 
                        {fromClause}     
                    ";
                    connection.Execute(sql, config.InputDefaulParameters);

                    selectClause = GetSelectClause(STDMTypeList.Dynamic);
                    sql = $@"
                        {insertClause}
                        {selectClause} 
                        from {tableName} 
                        where HASH='{hash}' and STDM=1
                    ";
                    connection.Execute(sql, config.InputDefaulParameters);
                }
                else if (firm.ActualDate.HasValue)
                {
                    var selectClause = GetSelectClause(STDMTypeList.Dynamic);
                    string sql = $@"
                        {insertClause}
                        {selectClause} 
                        {fromClause}     
                    ";
                    var serializeObject = JsonConvert.SerializeObject(config.InputDefaulParameters);
                    var inputParameters = JsonConvert.DeserializeObject<In>(serializeObject);
                    inputParameters.StartDate = firm.ActualDate.Value;
                    inputParameters.Firm = $"{firm.Id};";
                    connection.Execute(sql, inputParameters);
                }
            }
        }

        private string GetInsertClause()
        {
            string tableName = $"T_{config.Name}".ToUpper();
            var fields = new List<string>(new[] { "HASH", "STDM" });

            var properties = ReflectionHelper.GetPropertiesWithAttribute(config.OutputParameters.GetType(), typeof(DimensionAttribute)).ToList();
            foreach (var info in properties)
            {
                fields.Add(info.Name.ToUpper());
            }

            properties = ReflectionHelper.GetPropertiesWithAttribute(config.OutputParameters.GetType(), typeof(MeasureAttribute)).ToList();
            foreach (var info in properties)
            {
                fields.Add(info.Name.ToUpper());
            }

            var columns = string.Join(", ", fields.Select(x => $"{x}"));
            var result = $"insert into {tableName}({columns})";
            return result;
        }

        private string GetSelectClause(STDMTypeList stdm)
        {
            string hash = config.InputParameters.Hash;
            var fields = new List<string>(new[] { $"'{hash}' as HASH", $"{(int)stdm} as STDM" });
            var properties = ReflectionHelper.GetPropertiesWithAttribute(config.OutputParameters.GetType(), typeof(DimensionAttribute)).ToList();
            foreach (var info in properties)
            {
                fields.Add(info.Name.ToUpper());
            }
            properties = ReflectionHelper.GetPropertiesWithAttribute(config.OutputParameters.GetType(), typeof(MeasureAttribute)).ToList();
            foreach (var info in properties)
            {
                fields.Add(info.Name.ToUpper());
            }
            var result = "select " + string.Join(", ", fields);
            return result;
        }

        private string GetFromClause()
        {
            var inputParameters = new Dictionary<int, string>();
            var properties = config.InputParameters.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var hashProperties = ReflectionHelper.GetPropertiesWithAttribute(config.InputParameters.GetType(), typeof(HashAttribute)).ToList();
            var filterProperties = ReflectionHelper.GetPropertiesWithAttribute(config.InputParameters.GetType(), typeof(FilterAttribute)).ToList();

            foreach (var info in properties)
            {
                var hashProperty = hashProperties.FirstOrDefault(x => x.Name == info.Name);
                if (hashProperty != null)
                {
                    var hashAttribute = hashProperty.GetCustomAttribute<HashAttribute>();
                    inputParameters.Add(hashAttribute.Index, $"@{info.Name}");
                }
                var filterProperty = filterProperties.FirstOrDefault(x => x.Name == info.Name);
                if (filterProperty != null)
                {
                    var filterAttribute = filterProperty.GetCustomAttribute<FilterAttribute>();
                    inputParameters.Add(filterAttribute.Index, $"@{info.Name}");
                }
            }
            return $"from {config.StoreProcedure}({string.Join(", ", inputParameters.OrderBy(x => x.Key).Select(x => x.Value))})";
        }

        private string GetWhereClause()
        {
            string result = "where (1=1)";
            if (!string.IsNullOrEmpty(config.QR))
                return config.QR;

            result += " and (HASH=@Hash)";
            result += " and (STDM=@STDM)";
            foreach (var clause in config.InputParameters.WhereClauses)
            {
                if (!string.IsNullOrEmpty(clause.Value)) result += $" and ({clause.Value})";
            }
            return result;
        }

        private void UpdateOlapAudit(Firm firm)
        {
            string hash = config.InputParameters.Hash;
            using (var connection = new FbConnection(config.FbConnectionString))
            {
                connection.Open();
                if (firm == null)
                {
                    string sql = "delete from OLAP_AUDIT where REPORT=@ReportName and HASH=@Hash";
                    connection.Execute(sql, new { ReportName = config.Name, Hash = hash });

                    sql = @"
                      insert into OLAP_AUDIT (REPORT, FIRM, FIRMNAME, STDATE, DMDATE, HASH)
                      select @ReportName, E.ID, E.NAME, @Stamp, @Stamp, @Hash
                      from FIRMS E
                      where E.OK = 1";
                    connection.Execute(sql, new { ReportName = config.Name, Stamp = config.Constract, Hash = hash });
                }
                else
                {
                    string sql = "update OLAP_AUDIT set DMDATE=@Date where REPORT=@ReportName and HASH=@Hash and FIRM=@Firm";
                    connection.Execute(sql, new { ReportName = config.Name, Hash = hash, Firm = firm.Id, Date = config.Constract });
                }
            }
        }
    }
}