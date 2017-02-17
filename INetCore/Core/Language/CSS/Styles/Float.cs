using System;
using INetCore.Core.Exception;
using INetCore.Drawing.Objects;

namespace INetCore.Core.Language.CSS.Styles
{
    public class Float : BaseStyle
    {
        //public new Drawing.Objects.Float StyleValue { get; private set; }

        public Float()
        {
            Name = "float";
        }

        public override void ParseValue(string value)
        {
            Value = value;
            Drawing.Objects.Float pt;
            StyleValue = Drawing.Objects.Float.None;
            if (!Enum.TryParse(value, true, out pt))
            {
                throw new NotValidCSSValueException($"Not valid position value: {value}");
            }
            StyleValue = pt;

        }

        public override void ApplyStyle(BaseObject bo)
        {
            base.ApplyStyle(bo);
            //bo.Float = StyleValue;
            var o = (Drawing.Objects.Float)StyleValue;
            bo.Float = o;
        }
    }
}
