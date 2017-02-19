using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace INetCore.Core.Language.JavaScript
{
    public struct NumberVar
    {
        private bool IsInteger { get; set; }
        private bool IsNaN { get; set; }

        public long intValue;
        public double floatValue;

        public static implicit operator NumberVar(long value)
        {
            var n = new NumberVar
            {
                intValue = value,
                IsInteger = true
            };

            return n;
        }

        public static implicit operator NumberVar(double value)
        {
            var n = new NumberVar
            {
                floatValue = value,
                IsInteger = false
            };

            return n;
        }

        public static implicit operator long(NumberVar value)
        {
            if (value.IsInteger) return value.intValue;
            return (long)value.floatValue;
        }

        public static implicit operator double (NumberVar value)
        {
            return value.IsInteger ? value.intValue : value.floatValue;
        }
    }
}
