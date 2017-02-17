using System;
using System.Collections.Generic;
using System.Linq;
using INetCore.Drawing.Objects;

namespace INetCore.Core.Language.CSS.Styles
{
    public class BaseStyle
    {
        public string Name { get; protected set; } = "BaseStyle";
        public string Value { get; protected set; } = "";
        public virtual object StyleValue { get; protected set; } = null;
        public List<BaseStyle> Children { get; private set; } = new List<BaseStyle>();
        public BaseStyle Parent { get; set; } = null;
        public bool IsUsed { get; set; } = true;
        public bool IsImportant { get; set; } = false;

        public virtual void ParseValue(string value)
        {
            throw new NotImplementedException();
        }

        public virtual void ApplyStyle(BaseObject bo)
        {
            bo._cssProperties[Name] = this;
        }

        public virtual string GetValueToText()
        {
            return StyleValue?.ToString() ?? "";
        }

        public override string ToString()
        {
            var value = GetValueToText();
            if (Children.Count > 0) value = string.Join(" ", Children.Select(item => item.StyleValue.ToString()));

            var importantText = IsImportant ? " !important;" : "";
            var formatText = $"{Name}: {value}{importantText};";

            return IsUsed ? $"{formatText}" : $"/*{formatText}*/";
        }
    }
}
