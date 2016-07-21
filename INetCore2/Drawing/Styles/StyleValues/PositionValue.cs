namespace INetCore2.Drawing.Styles.StyleValues
{
    public class PositionValue : BaseStyleValue
    {
        protected new PositionType currentValue = PositionType.Static;
        public override object DefaultValue { get; protected set; } = PositionType.Static;

        public static implicit operator PositionValue(PositionType val)
        {
            var a = new PositionValue {CurrentValue = val};
            return a;
        }

        public PositionValue()
        {
            currentValue = PositionType.Static;
        }

        public PositionValue(PositionType val) : this()
        {
            CurrentValue = val;
        }

        public override string ToString()
        {
            return currentValue.ToString().ToLower();
        }
    }

    public enum PositionType
    {
        Static,
        Relative,
        Absolute,
        Fixed
    }
}
