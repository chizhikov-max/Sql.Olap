using System;

namespace Oper.Sql.Common
{
    public class MeasureAttribute : Attribute
    {
        public MeasureAttribute()
        {
        }

        public MeasureAttribute(string caption)
        {
            Caption = caption;
        }

        public string Caption { get; set; }
    }
}
