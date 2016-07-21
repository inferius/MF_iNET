using System;
using System.Globalization;
using System.Text;

namespace INetCore.Drawing.Objects
{
    public class MeasuredUnit
    {
        public bool IsInitial { get; set; } = false;
        public bool IsInherited { get; set; } = false;
        public bool IsAuto { get; set; } = false;

        public float Value
        {
            get { return _value; }
            set
            {
                _value = value;
                RealValue = value;
                IsDynamicSet = false;
            }
        }

        public Unit Unit { get; set; } = Unit.Pixels;
        public bool IsDynamicSet { get; set; } = false;

        private float _value = 0f;
        public float RealValue { get; set; }

        public static MeasuredUnit Parse(string unit)
        {
            var u = unit.Trim().ToLower(CultureInfo.InvariantCulture);
            var n = new MeasuredUnit();

            switch (u)
            {
                case "initial":
                    n.IsInitial = true;
                    n.IsDynamicSet = true;
                    break;
                case "inherit":
                    n.IsInherited = true;
                    n.IsDynamicSet = true;
                    break;
                case "auto":
                    n.IsAuto = true;
                    n.IsDynamicSet = true;
                    break;
            }

            if (!n.IsDynamicSet)
            {
                var num = new StringBuilder(6);
                var numComplete = false;
                var unitText = new StringBuilder(3);
                foreach (var c in u)
                {
                    if (char.IsDigit(c) || c == '.')
                    {
                        if (numComplete) continue;
                        num.Append(c);
                    }
                    else
                    {
                        numComplete = true;
                        if (char.IsWhiteSpace(c)) continue;
                        unitText.Append(c);
                    }
                }
                float unitValue;
                if (!float.TryParse(num.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture, out unitValue))
                {
                    throw new FormatException("NumberValueNotValidFormat");
                }
                n.Unit = ParseUnitString(unitText.ToString());
                n.Value = unitValue;
            }


            return n;
        }

        public static Unit ParseUnitString(string unit)
        {
            switch (unit)
            {
                case "cm":
                    return Unit.Centimeter;
                case "em":
                    return Unit.Em;
                case "ex":
                    return Unit.Ex;
                case "in":
                    return Unit.Inch;
                case "mm":
                    return Unit.Milimeter;
                case "%":
                    return Unit.Percentage;
                case "pc":
                    return Unit.Pica;
                case "px":
                    return Unit.Pixels;
                case "pt":
                    return Unit.Poshort;
                case "rem":
                    return Unit.RootEm;
                default:
                    return Unit.Pixels;
            }
        }

        public static string UnitToString(Unit unit)
        {
            switch (unit)
            {
                case Unit.Centimeter:
                    return "cm";
                case Unit.Em:
                    return "em";
                case Unit.Ex:
                    return "ex";
                case Unit.Inch:
                    return "in";
                case Unit.Milimeter:
                    return "mm";
                case Unit.Percentage:
                    return "%";
                case Unit.Pica:
                    return "pc";
                case Unit.Pixels:
                    return "px";
                case Unit.Poshort:
                    return "pt";
                case Unit.RootEm:
                    return "rem";
                default:
                    return "px";
            }
        }

        public static float ConvertUnit(float input, Unit from, Unit to = Unit.Pixels, float dpiX = 1.3333333333333333333333333f)
        {
            float ret = 0;
            var mm_px = 0.26455026455026455026455026455026f;//3.78f;
            var cm_px = 0.026455026455026455026455026455026f; //37.8f;
            var in_px = 0.01041666666666666666666666666667f;//96;
            var pc_px = 0.0625f;//16;
            var pt_px = dpiX / 72;

            if (from == Unit.Milimeter) ret = input / mm_px;
            else if (from == Unit.Centimeter) ret = input / cm_px;
            else if (from == Unit.Pica) ret = input / pc_px;
            else if (from == Unit.Inch) ret = input / in_px;
            else if (from == Unit.Poshort) ret = input * pt_px;
            else if (from == Unit.Pixels) ret = input;

            if (to == Unit.Pixels) return ret;
            if (@from == Unit.Milimeter) ret = input * mm_px;
            else if (@from == Unit.Centimeter) ret = input * cm_px;
            else if (@from == Unit.Pica) ret = input * pc_px;
            else if (@from == Unit.Inch) ret = input * in_px;
            else if (@from == Unit.Poshort) ret = input / pt_px;

            return ret;
        }

        #region Standard methods

        public static implicit operator MeasuredUnit(float value)
        {
            if (float.IsNaN(value) || value == null)
            {
                return new MeasuredUnit
                {
                    IsDynamicSet = true,
                    Value = float.NaN,
                    IsInherited = true
                };
            }

            return new MeasuredUnit
            {
                IsDynamicSet = false,
                Unit = Unit.Pixels,
                Value = value
            };
        }

        public static implicit operator float(MeasuredUnit value)
        {
            if (value == null) return float.NaN;
            return value.Value;
        }

        public override string ToString()
        {
            if (!IsDynamicSet) return $"{Value}{UnitToString(Unit)}";
            if (IsInherited) return "inherit";
            if (IsInitial) return "initial";
            if (IsAuto) return "auto";

            return "";
        }

        public override bool Equals(object obj)
        {
            var o = obj as MeasuredUnit;
            if (o == null) return false;
            if (o.IsDynamicSet && IsDynamicSet)
            {
                return o.IsAuto == IsAuto && o.IsInitial == IsInitial && o.IsInherited == IsInherited;
            }

            return Math.Abs(o.Value - Value) < 0.001 && o.Unit == Unit;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        #endregion
    }
}
