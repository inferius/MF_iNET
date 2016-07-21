namespace INetCore2.Drawing.Styles
{
    public class BaseStyleValue : IStyleValue
    {
        protected object currentValue;
        public bool IsDefault { get; protected set; } = true;
        public virtual object DefaultValue { get; protected set; }

        public object CurrentValue
        {
            get { return currentValue; }
            set { currentValue = value;
                IsDefault = false;
            }
        }

        public override string ToString()
        {
            return "initial";
        }
    }

    public enum ValueType
    {
        Size,
        Text,
        MultiSize,
        Combine // obsahuje vice podrizenych stylu
    }
}
