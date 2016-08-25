using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INetCore.Core.Language.CSS.Styles;
using INetCore.Core.Language.Javascript.Tokens;

namespace INetCore.Core.Language.Javascript.Actions
{
    public class AssingAction : IActions
    {
        public IActions Right { get; private set; } = null;
        public Variable ReturnValue { get; private set; } = null;
        public bool Execute()
        {
            try
            {
                Right.Execute();
                ReturnValue = Right.ReturnValue;
            }
            catch (System.Exception)
            {
                return false;
            }
            return true;
        }

        public void Define(Variable left, IActions right = null)
        {
            ReturnValue = left;
            Right = right;
        }

        public void SetRight(IActions var)
        {
            Right = var;
        }

    }
}
