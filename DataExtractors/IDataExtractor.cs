namespace Sql.Olap.DataExtractors
{
    public interface IDataExtractor
    {
        bool CreateTable();
        void ReadData();
        void InsertData();
        void TruncateData();
        void ClearObsoleteTables();
    }
}