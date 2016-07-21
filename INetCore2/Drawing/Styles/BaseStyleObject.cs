using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INetCore2.Drawing.Styles
{
    public class BaseStyleObject
    {
        public string Name { get; protected set; }
        public string DisplayName { get; protected set; }
        public IStyleValue Value { get; set; }
        public bool IsUsed { get; set; } = true;

        public override string ToString()
        {
            return IsUsed ? $"{DisplayName}: {Value};" : $"/*{DisplayName}: {Value};*/";
        }
    }
}
