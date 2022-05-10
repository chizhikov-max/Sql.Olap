namespace Oper.Sql.OlapBuilders
{
    public interface IBuilder
    {
        void GenerateXMLA();
        void Process();
        void ClearObsoleteOlapDatabase();
    }
}