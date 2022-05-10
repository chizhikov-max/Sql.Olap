using System.Runtime.Serialization;

namespace Sql.Olap.OlapBuilders
{
    [DataContract]
    public class ViewElement
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Type { get; set; }
    }
}