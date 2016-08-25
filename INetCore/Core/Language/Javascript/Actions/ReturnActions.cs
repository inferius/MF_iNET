using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INetCore.Core.Language.Javascript.Tokens;

namespace INetCore.Core.Language.Javascript.Actions
{
    public class ReturnActions : IActions
    {
        public Variable ReturnValue { get; private set; } = null;
        public bool Execute()
        {
            throw new NotImplementedException();
        }

    }
}
