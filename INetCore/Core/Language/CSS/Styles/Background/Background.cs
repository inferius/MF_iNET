using System;
using INetCore.Drawing.Objects;

namespace INetCore.Core.Language.CSS.Styles
{
    public class Background : BaseStyle
    {
        public Background()
        {
            Name = "background";
        }

        public override void ParseValue(string value)
        {
            Value = value;
            var prms = value.Split(new [] {" "}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var prm in prms)
            {
                var p = prm.Trim();

            }
        }

        public override void ApplyStyle(BaseObject bo)
        {
            foreach (var child in Children)
            {
                child.ApplyStyle(bo);
            }
        }
    }
}
