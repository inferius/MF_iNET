using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using INetCore.Core.Exception;
using INetCore.Drawing.Objects;

namespace INetCore.Core.Language.CSS.Styles
{
    public class Display : BaseStyle
    {
        public Display()
        {
            Name = "display";
        }

        public override void ParseValue(string value)
        {
            Value = value;
            DisplayPropertyType pt;
            StyleValue = DisplayPropertyType.Inline;
            if (!Enum.TryParse(value, true, out pt))
            {
                throw new NotValidCSSValueException($"Not valid position value: {value}");
            }
            StyleValue = pt;

        }

        private static string GetDescription<T>(T enumerationValue)
            where T : struct
        {
            Type type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", nameof(enumerationValue));
            }

            //Tries to find a DescriptionAttribute for a potential friendly name
            //for the enum
            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    //Pull out the description value
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            //If we have no description attribute, just return the ToString of the enum
            return enumerationValue.ToString();

        }

        public override string GetValueToText()
        {
            var enumerationValue = (DisplayPropertyType)StyleValue;
            var type = enumerationValue.GetType();

            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo != null && memberInfo.Length > 0)
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description.ToLower(CultureInfo.InvariantCulture);
                }
            }
            return enumerationValue.ToString().ToLower(CultureInfo.InvariantCulture);
        }

        public override void ApplyStyle(BaseObject bo)
        {
            var o = (DisplayPropertyType)StyleValue;
            if (bo.Display == null) bo.Display = new DisplayProperty();
            bo.Display.Type = o;
        }
    }

}
