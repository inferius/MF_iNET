using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INetCore.Core.Language.JavaScript
{
    public class Variable
    {
        public VarType Type { get; private set; }
        private NumberVar numValue;

        #region Implicit

        //public static implicit operator Variable(int val)
        //{
        //    VarType 
        //}
        #endregion
    }

    public enum VarType
    {
        String,
        Number,
        Object,
        Boolean
    }
}
