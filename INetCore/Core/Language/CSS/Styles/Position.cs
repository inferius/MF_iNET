using System;
using System.Drawing;
using INetCore.Core.Exception;
using INetCore.Core.Language.CSS.Values;
using INetCore.Drawing.Objects;

namespace INetCore.Core.Language.CSS.Styles
{
    public class Position : BaseStyle
    {
        //public new PositionType StyleValue { get; private set; }

        public Position()
        {
            Name = "position";
        }

        public override void ParseValue(string value)
        {
            Value = value;
            PositionType pt;
            StyleValue = PositionType.Static;
            if (!Enum.TryParse(value, true, out pt))
            {
                throw new NotValidCSSValueException($"Not valid position value: {value}");
            }
            StyleValue = pt;

        }

        public override void ApplyStyle(BaseObject bo)
        {
            //bo.PositionType = StyleValue;
            var o = (PositionType)StyleValue;
            bo.PositionType = o;
        }
    }
}
