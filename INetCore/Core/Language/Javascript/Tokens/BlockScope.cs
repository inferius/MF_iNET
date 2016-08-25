using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INetCore.Core.Language.Javascript.Tokens
{
    public class BlockScope
    {
        public List<Variable> Variables { get; private set; } = new List<Variable>();
    }
}
