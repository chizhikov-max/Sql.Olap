using System.Runtime.Serialization;

namespace Sql.Olap.OlapBuilders
{
    [DataContract]
    public class BuilderSource
    {
        [DataMember]
        public string FullName { get; set; }
        [DataMember]
        public string DataSource { get; set; }
        [DataMember]
        public string View { get; set; }
        [DataMember]
        public string Dimension { get; set; }
        [DataMember]
        public string Cube { get; set; }
    }
}