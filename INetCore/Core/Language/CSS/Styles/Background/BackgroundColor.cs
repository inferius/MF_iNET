using System;
using System.Drawing;
using INetCore.Core.Language.CSS.Values;
using INetCore.Drawing.Objects;

namespace INetCore.Core.Language.CSS.Styles
{
    public class BackgroundColor : BaseStyle
    {
        //public override CssColorValue StyleValue { get; private set; }

        public BackgroundColor()
        {
            Name = "background-color";
        }

        public override void ParseValue(string value)
        {
            Value = value;
            StyleValue = new CssColorValue(value);
        }

        public override void ApplyStyle(BaseObject bo)
        {
            base.ApplyStyle(bo);
            var o = StyleValue as CssColorValue;
            if (o != null) bo.Background.Color = o.Color;
        }
    }
}
