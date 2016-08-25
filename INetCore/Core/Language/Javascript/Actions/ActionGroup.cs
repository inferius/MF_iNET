using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INetCore.Core.Language.Javascript.Tokens;

namespace INetCore.Core.Language.Javascript.Actions
{
    public class ActionGroup : IActions
    {
        public List<IActions> Actions { get; private set; }
        public string OriginalInput { get; set; }

        public Variable ReturnValue { get; private set; }

        public bool Execute()
        {
            foreach (var action in Actions)
            {
                action.Execute();
            }

            ReturnValue = Actions.LastOrDefault()?.ReturnValue;

            return true;
        }
    }
}
