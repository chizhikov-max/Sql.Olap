using System;
using System.Collections.Generic;

namespace Oper.Sql
{
    public class Config<T>
    {
        /// <summary>
        /// Метод для извлечения данных.
        /// Обязательный параметр
        /// </summary>
        public Func<IEnumerable<T>> DataReader { get; set; }

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
    }
}
