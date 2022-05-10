using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sql.Olap.OlapBuilders
{
    [DataContract]
    public class DimensionObject
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public string ViewName { get; set; }

        [DataMember]
        public List<DimensionElement> Elements { get; set; }
    }
}