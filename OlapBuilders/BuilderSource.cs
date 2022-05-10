using System.Runtime.Serialization;

namespace Oper.Sql.OlapBuilders
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