using INetCore.Drawing.Objects;

namespace INetCore.Core.Language.CSS.Styles
{
    #region Left
    public class Left : BaseStyle
    {
        //public new MeasuredUnit StyleValue { get; private set; }

        public Left()
        {
            Name = "left";
        }

        public override void ParseValue(string value)
        {
            Value = value;
            StyleValue = MeasuredUnit.Parse(value);
        }

        public override void ApplyStyle(BaseObject bo)
        {
            var o = StyleValue as MeasuredUnit;
            if (o != null) bo.ObjectPosition.Left = o;
        }
    }
    #endregion
    #region Right
    public class Right : BaseStyle
    {
        //public new MeasuredUnit StyleValue { get; private set; }

        public Right()
        {
            Name = "right";
        }

        public override void ParseValue(string value)
        {
            Value = value;
            StyleValue = MeasuredUnit.Parse(value);
        }

        public override void ApplyStyle(BaseObject bo)
        {
            var o = StyleValue as MeasuredUnit;
            if (o != null) bo.ObjectPosition.Right = o;
            //bo.ObjectPosition.Right = StyleValue;
        }
    }
    #endregion
    #region Top
    public class Top : BaseStyle
    {
        //public new MeasuredUnit StyleValue { get; private set; }

        public Top()
        {
            Name = "top";
        }

        public override void ParseValue(string value)
        {
            Value = value;
            StyleValue = MeasuredUnit.Parse(value);
        }

        public override void ApplyStyle(BaseObject bo)
        {
            //bo.ObjectPosition.Top = StyleValue;
            var o = StyleValue as MeasuredUnit;
            if (o != null) bo.ObjectPosition.Top = o;
        }
    }
    #endregion
    #region Bottom
    public class Bottom : BaseStyle
    {
        //public new MeasuredUnit StyleValue { get; private set; }

        public Bottom()
        {
            Name = "bottom";
        }

        public override void ParseValue(string value)
        {
            Value = value;
            StyleValue = MeasuredUnit.Parse(value);
        }

        public override void ApplyStyle(BaseObject bo)
        {
            //bo.ObjectPosition.Bottom = StyleValue;
            var o = StyleValue as MeasuredUnit;
            if (o != null) bo.ObjectPosition.Bottom = o;
        }
    }
    #endregion
}
