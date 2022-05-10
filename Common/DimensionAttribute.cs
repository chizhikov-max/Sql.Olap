using System;

namespace Sql.Olap.Common
{
    public class DimensionAttribute : Attribute
    {
        public DimensionAttribute()
        {
        }

        public DimensionAttribute(string key, string keyDataType, string name, string dataType, string caption)
        {
            Key = key;
            KeyDataType = keyDataType;
            Name = name;
            DataType = dataType;
            Caption = caption;
        }

        public string Key { get; set; }
        public string KeyDataType { get; set; }
        public string Name { get; set; }
        public string DataType { get; set; }
        public string Caption { get; set; }
    }
}
