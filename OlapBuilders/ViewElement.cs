using System.Runtime.Serialization;

namespace Oper.Sql.OlapBuilders
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