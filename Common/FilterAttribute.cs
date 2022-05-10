using System;

namespace Sql.Olap.Common
{
    public class FilterAttribute : Attribute
    {
        public FilterAttribute(int index)
        {
            Index = index;
        }

        public int Index { get; set; }
    }
}
