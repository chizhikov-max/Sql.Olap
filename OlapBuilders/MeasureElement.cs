using System.Runtime.Serialization;

namespace Sql.Olap.OlapBuilders
{
    [DataContract]
    public class MeasureElement
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string DataType { get; set; }
    }
}