﻿using System.Runtime.Serialization;

namespace Sql.Olap.OlapBuilders
{
    [DataContract]
    public class DimensionElement
    {
        public string Key { get; set; }

        [DataMember]
        public string KeyDataType { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string NameDataType { get; set; }

        [DataMember]
        public string OrderBy { get; set; }
    }
}