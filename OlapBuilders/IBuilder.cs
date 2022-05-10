namespace Sql.Olap.OlapBuilders
{
    public interface IBuilder
    {
        void GenerateXMLA();
        void Process();
        void ClearObsoleteOlapDatabase();
    }
}