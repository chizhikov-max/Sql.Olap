using System.Runtime.Serialization;

namespace Sql.Olap.OlapBuilders
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