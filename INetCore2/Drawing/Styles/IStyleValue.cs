using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INetCore2.Drawing.Styles
{
    public interface IStyleValue
    {
        bool IsDefault { get; }
        object DefaultValue { get; }
        object CurrentValue { get; }
    }
}
