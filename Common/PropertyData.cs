namespace Sql.Olap.Common
{
    /// <summary>
    /// Представляет структуру, содержащую данные, необходимые для получения значения свойства
    /// </summary>
    public class PropertyData
    {
        /// <summary>
        /// Имя свойства
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Массив строковых представлений индексных параметров
        /// </summary>
        public string[] IndexParameters { get; set; }
    }
}
