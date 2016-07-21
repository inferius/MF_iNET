using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INetCore2.Drawing.Styles.StyleValues
{
    public class SizeValue : BaseStyleValue
    {
        protected new Size currentValue;
        public override object DefaultValue { get; protected set; } = 0;

        public static implicit operator SizeValue(float val)
        {
            var a = new SizeValue { CurrentValue = val };
            return a;
        }

        public SizeValue()
        {
            currentValue = 0f;
        }

        public SizeValue(Size val) : this()
        {
            CurrentValue = val;
        }

        public override string ToString()
        {
            return currentValue.ToString();
        }
    }
}
