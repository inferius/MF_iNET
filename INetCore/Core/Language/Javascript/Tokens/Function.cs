using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using INetCore.Core.Language.Javascript.Actions;

namespace INetCore.Core.Language.Javascript.Tokens
{
    public class Function
    {
        public string Name { get; private set; }
        public bool IsAnonymous { get; private set; } = false;
        public bool IsAsync { get; private set; } = false;
        public BlockScope Scope { get; private set; } = new BlockScope();
        public IEnumerable<IActions> Value { get; private set; }
        public IEnumerable<Variable> Arguments { get; private set; }

        public void Define(IEnumerable<IActions> value = null, IEnumerable<Variable> arguments = null, string name = null)
        {
            if (name == null) IsAnonymous = true;
            else Name = name;

            Arguments = arguments;
            Value = value;
        }

        public void Set(IEnumerable<Actions.IActions> value)
        {
            Value = value;
        }

        public async Task<Variable> Execute()
        {
            Variable v = null;
            foreach (var action in Value)
            {
                if (action is AsyncAction)
                {
                    await (action as AsyncAction).ExecuteAsync();
                }
                else if (action is ReturnActions)
                {
                    action.Execute();
                    v = action.ReturnValue;
                    break;
                }
                else
                {
                    action.Execute();
                }
            }

            return v;
        }
    }
}
