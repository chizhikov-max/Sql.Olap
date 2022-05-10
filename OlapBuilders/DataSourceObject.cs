using System.Runtime.Serialization;

namespace Oper.Sql.OlapBuilders
{
    [DataContract]
    public class DataSourceObject
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public string ConnectionString { get; set; }
    }
}