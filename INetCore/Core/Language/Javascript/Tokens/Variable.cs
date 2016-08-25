using System;

namespace INetCore.Core.Language.Javascript.Tokens
{
    public class Variable
    {
        public VariableType VariableType { get; private set; }
        public string Name { get; private set; }
        public object Value { get; private set; }

        public void Define(VariableType type, string name, object value = null)
        {
            VariableType = type;
            Name = name;
            Value = value;
        }

        public object Get()
        {
            return Value;
        }

        public void Set(object value)
        {
            if (VariableType == VariableType.Const) throw new ArgumentException("Variable is constant");
            Value = value;
        }
    }

    public enum VariableType
    {
        Let,
        Const,
        Var
    }
}
