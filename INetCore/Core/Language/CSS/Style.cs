using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INetCore.Core.Language.CSS.Styles;

namespace INetCore.Core.Language.CSS
{
    public class Style
    {
        public string FullName { get; set; }
        public BaseStyle[] Styles { get; set; }
        public int Priority { get; set; } = 0;

        public override string ToString()
        {
            return $"Priority: {Priority}, Name: {FullName}";
        }
    }
}
