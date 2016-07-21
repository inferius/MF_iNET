using INetCore.Drawing.Objects;

namespace INetCore.Core.Language.CSS.Styles
{
    #region Width
    public class Width : BaseStyle
    {
        //public new MeasuredUnit StyleValue { get; private set; }

        public Width()
        {
            Name = "width";
        }

        public override void ParseValue(string value)
        {
            Value = value;
            StyleValue = MeasuredUnit.Parse(value);
        }

        public override void ApplyStyle(BaseObject bo)
        {
            //bo.Width = StyleValue;
            var o = StyleValue as MeasuredUnit;
            if (o != null) bo.Width = o;
        }
    }
    #endregion
    #region Height
    public class Height : BaseStyle
    {
        //public new MeasuredUnit StyleValue { get; private set; }

        public Height()
        {
            Name = "height";
        }

        public override void ParseValue(string value)
        {
            Value = value;
            StyleValue = MeasuredUnit.Parse(value);
        }

        public override void ApplyStyle(BaseObject bo)
        {
            //bo.Height = StyleValue;
            var o = StyleValue as MeasuredUnit;
            if (o != null) bo.Height = o;
        }
    }
    #endregion
}
