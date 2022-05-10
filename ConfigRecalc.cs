using System;
using System.Collections.Generic;
using System.Linq;
using Sql.Olap.Common;

namespace Sql.Olap
{
    public class ConfigRecalc<In, Out>
        where In : InParametersBase, new()
        where Out : OutParametersBase
    {
        public ConfigRecalc(In inputParameters, Out outputParameters, string storeProcedure)
        {
            InputParameters = inputParameters;
            OutputParameters = outputParameters;
            StoreProcedure = storeProcedure;
        }

        public In InputParameters { get; set; }

        public In InputDefaulParameters
        {
            get
            {
                var result = new In();

                var hashProperties = ReflectionHelper.GetPropertiesWithAttribute(InputParameters.GetType(), typeof(HashAttribute)).ToList();
                foreach (var info in hashProperties)
                {
                    var value = info.GetValue(InputParameters, null);
                    info.SetValue(result, value);
                }

                var filterProperties = ReflectionHelper.GetPropertiesWithAttribute(InputParameters.GetType(), typeof(FilterAttribute)).ToList();
                foreach (var info in filterProperties)
                {
                    var value = InputParameters.GetDefaultValue(info.Name);
                    info.SetValue(result, value);
                }

                return result;
            }
        }

        public Out OutputParameters { get; set; }
        public string StoreProcedure { get; set; }
        public string QR { get; set; }

        /// <summary>
        /// Временная метка.
        /// </summary>
        public DateTime Constract { get; set; } = DateTime.Now;

        /// <summary>
        /// Наименование.
        /// Обязательный параметр
        /// </summary>
        public string Name { get; set; }

        public string UniqueName => $"{Name}_{Constract:yyyyMMdd_HHmmss_fff}";

        /// <summary>
        /// Строка для подключения к FireBird.
        /// Обязательный параметр
        /// </summary>
        public string FbConnectionString { get; set; }

        /// <summary>
        /// Строка для подключения к SQL Server.
        /// Обязательный параметр
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Строка для подключения к базе.
        /// Обязательный параметр
        /// </summary>
        public string OlapConnectionString { get; set; }

        /// <summary>
        /// Путь до папки, в которой лежат шаблоны.
        /// Обязательный параметр
        /// </summary>
        public string TemplatePath { get; set; }

        /// <summary>
        /// Очистить данные перед загрузкой.
        /// </summary>
        public bool IsTruncateData { get; set; }

        /// <summary>
        /// Создавать структуры для куба
        /// </summary>
        public bool IsCreateCube { get; set; }

        /// <summary>
        /// Задается сколько хранятся данные в минутах.
        /// По-умолчанию 60 минут
        /// </summary>
        public int ActiveDataTimeout { get; set; } = 120;

        ///// <summary>
        ///// Метод для извлечения данных.
        ///// Обязательный параметр
        ///// </summary>
        //public Func<IEnumerable<T>> DataReader { get; set; }

        public Config<Out> GetOlapConfig(Func<IEnumerable<Out>> dataReader)
        {
            var config = new Config<Out>
            {
                Constract = Constract,
                Name = Name,
                ConnectionString = ConnectionString,
                OlapConnectionString = OlapConnectionString,
                TemplatePath = TemplatePath,
                IsTruncateData = IsTruncateData,
                IsCreateCube = IsCreateCube,
                ActiveDataTimeout = ActiveDataTimeout,
                DataReader = dataReader
            };
            return config;
        }
    }
}
