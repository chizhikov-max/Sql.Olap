using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Sql.Olap.OlapBuilders
{
    [DataContract]
    public class ViewObject
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string FullName { get; set; }

        [DataMember]
        public string Query { get; set; }

        [DataMember]
        public string DataSourceName { get; set; }

        [DataMember]
        public List<ViewElement> Elements { get; set; }
    }
}