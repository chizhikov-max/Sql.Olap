using System.Runtime.Serialization;

namespace Oper.Sql.OlapBuilders
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