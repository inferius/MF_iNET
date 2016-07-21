using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace INetCore2.Drawing.Styles
{
    public class Size
    {
        private static Regex SizePattern { get; } = new Regex(@"^([0-9]*(?:.[0-9]+)?)(?:\s*)([a-zA-Z%]*)?");
        public float Value { get; set; }
        public Unit Unit { get; set; } = UnitType.Pixels;


        public override string ToString()
        {
            var u = Value != 0 ? Unit.ToString() : string.Empty;
            return $"{Value.ToString(CultureInfo.InvariantCulture)}{u}";
        }

        public static implicit operator Size(float val)
        {
            return new Size {Value = val};
        }

        public static implicit operator Size(int val)
        {
            return new Size { Value = val };
        }

        public static Size Parse(string value)
        {
            var grp = SizePattern.Match(value.Trim());
            var s = new Size();

            if (grp.Groups.Count == 0 || grp.Groups.Count > 3) throw new ArgumentException("Size value not valid");

            s.Value = float.Parse(grp.Groups[1].Value, CultureInfo.InvariantCulture);
            s.Unit = Unit.Parse(grp.Groups[2].Value);

            return s;
        }
    }
}
