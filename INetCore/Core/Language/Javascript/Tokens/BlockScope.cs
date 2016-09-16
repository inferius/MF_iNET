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

        public Variable GetVariableByName(string name)
        {
            foreach (var v in Variables)
            {
                if (v.Name == name) return v;
            }

            return null;
        }


        public void SetVariable(Variable v)
        {
            var exist = GetVariableByName(v.Name);

            if (exist != null)
            {
                exist.Set(v.Value);
            }
            else
            {
                Variables.Add(v);
            }
        }
    }
}
