using System;

namespace Oper.Sql.Common
{
    public class HashAttribute : Attribute
    {
        public HashAttribute(int index)
        {
            Index = index;
        }

        public int Index { get; set; }
    }
}
