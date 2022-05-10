using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Oper.Sql.OlapBuilders
{
    [DataContract]
    public class CubeObject
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public string DimensionName { get; set; }

        [DataMember]
        public string ViewName { get; set; }

        [DataMember]
        public List<DimensionElement> Dimensions { get; set; }

        [DataMember]
        public List<MeasureElement> Measures { get; set; }
    }
}