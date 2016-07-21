using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using INetCore.Core.Exception;

namespace INetCore.Core.Language.CSS.Values
{
    public class CssColorValue
    {
        private string _originalValue = "";

        public CSSColorTypeUsed OutputColorUsed = CSSColorTypeUsed.Hex;
        public Color Color { get; private set; }
        public string OriginalValue
        {
            get
            {
                return _originalValue;
            }
            set
            {
                parseValue(value);
            }
        }

        public CssColorValue(string value = "transparent")
        {
            parseValue(value);
        }

        public static CssColorValue Parse(string value)
        {
            return new CssColorValue(value);
        }

        private void parseValue(string value)
        {
            //TODO: Dodělat kontrolu validity zadané value pomocí regularního výrazu
            _originalValue = value.Trim();
            if (_originalValue.Length == 0) throw new NotValidCSSValueException("Empty value not valid");

            if (_originalValue.StartsWith("#")) Color = ColorTranslator.FromHtml(_originalValue);
            else if (_originalValue.StartsWith("rgb("))
            {
                var lst = getIntList(_originalValue);
                if (lst.Length < 3) throw new NotValidCSSValueException($"Not valid input color value {value}");
                Color = Color.FromArgb(lst[0], lst[1], lst[2]);
            }
            else if (_originalValue.StartsWith("rgba("))
            {
                var lst = getIntList(_originalValue);
                if (lst.Length < 4) throw new NotValidCSSValueException($"Not valid input color value {value}");
                Color = Color.FromArgb(lst[3], lst[0], lst[1], lst[2]);
            }
            else if (_originalValue.StartsWith("hsl("))
            {
                var lst = getFloatList(_originalValue);
                if (lst.Length < 3) throw new NotValidCSSValueException($"Not valid input color value {value}");
                Color = new HSLColor(Math.PI / 180 * lst[0], lst[1] / 100, lst[2] / 100).Color;
            }
            else if (_originalValue.StartsWith("hsla("))
            {
                var lst = getFloatList(_originalValue);
                if (lst.Length < 4) throw new NotValidCSSValueException($"Not valid input color value {value}");
                Color = Color.FromArgb((int)lst[3], new HSLColor(Math.PI / 180 * lst[0], lst[1] / 100, lst[2] / 100).Color);
            }
            else
            {
                KnownColor col;
                var suc = Enum.TryParse(_originalValue, true, out col);
                if (!suc) throw new NotValidCSSValueException($"Not valid input color value {value}");

                Color = Color.FromKnownColor(col);
            }
        }


        private int[] getIntList(string value)
        {
            var flist = new List<int>(5);
            var temp = new StringBuilder();

            foreach (var v in value)
            {
                if (char.IsDigit(v))
                {
                    temp.Append(v);
                }
                else
                {
                    if (temp.Length > 0)
                    {
                        flist.Add(int.Parse(temp.ToString(), NumberStyles.Integer, CultureInfo.InvariantCulture));
                        temp.Clear();
                    }
                }
            }

            return flist.ToArray();
        }

        private float[] getFloatList(string value)
        {
            var flist = new List<float>(5);
            var temp = new StringBuilder();

            foreach (var v in value)
            {
                if (char.IsDigit(v) || v == '.')
                {
                    temp.Append(v);
                }
                else
                {
                    if (temp.Length > 0)
                    {
                        flist.Add(float.Parse(temp.ToString(), NumberStyles.Float, CultureInfo.InvariantCulture));
                        temp.Clear();
                    }
                }
            }

            return flist.ToArray();
        }

        private string toHex(bool upper = false)
        {
            var hexType = upper ? "X" : "x";
            return $"#{Color.R.ToString(hexType)}{Color.G.ToString(hexType)}{Color.B.ToString(hexType)}";
        }

        private static string toHex(Color color, bool upper = false)
        {
            var hexType = upper ? "X" : "x";
            return $"#{color.R.ToString(hexType)}{color.G.ToString(hexType)}{color.B.ToString(hexType)}";
        }

        private string getColorToString(CSSColorTypeUsed colorReturnType)
        {
            switch (colorReturnType)
            {
                case CSSColorTypeUsed.Original: return _originalValue;
                case CSSColorTypeUsed.Auto: return ColorTranslator.ToHtml(Color);
                case CSSColorTypeUsed.Hex:
                case CSSColorTypeUsed.HexUpperCase:
                    return toHex(colorReturnType == CSSColorTypeUsed.HexUpperCase);
                case CSSColorTypeUsed.Rgb: return $"rgb({Color.R}, {Color.G}, {Color.B})";
                case CSSColorTypeUsed.Rgba: return FormattableString.Invariant($"rgba({Color.R}, {Color.G}, {Color.B}, {convertAlphaToDouble(Color.A):0.##})");
                case CSSColorTypeUsed.Hsl:
                case CSSColorTypeUsed.Hsla:
                    var hsl = new HSLColor(Color);
                    return colorReturnType == CSSColorTypeUsed.Hsl ? $"hsl({hsl})" : FormattableString.Invariant($"hsla({hsl}, {convertAlphaToDouble(Color.A):0.##})");
                case CSSColorTypeUsed.ColorName:
                    // pokud neni znama vrati se HEX format
                    if (Color.IsNamedColor) return !Color.IsSystemColor ? $"{Color.ToKnownColor().ToString().ToLower()}" : toHex();
                    return findNamedColor(Color);
                default: return ColorTranslator.ToHtml(Color);
            }
        }

        /// <summary>
        /// Vyhledá zadanou barvu zda je ve znamých barvách
        /// </summary>
        /// <param name="original"></param>
        /// <returns></returns>
        private static string findNamedColor(Color original)
        {
            var colorLookup = Enum.GetValues(typeof(KnownColor))
               .Cast<KnownColor>()
               .Select(Color.FromKnownColor)
               .ToLookup(c => c.ToArgb());

            foreach (var namedColor in colorLookup[original.ToArgb()])
            {
                if (namedColor.IsSystemColor || !namedColor.IsNamedColor) continue;
                return namedColor.Name.ToLower();
            }

            return toHex(original);
        }

        private static double convertAlphaToDouble(byte value)
        {
            return value / 2.55d / 100d;
        }

        public override string ToString()
        {
            return getColorToString(OutputColorUsed);
        }

        public string ToString(CSSColorTypeUsed displyColorType)
        {
            return getColorToString(displyColorType);
        }

        #region HSLColor

        #endregion
    }

    public enum CSSColorTypeUsed
    {
        Original,
        Rgb,
        Rgba,
        Hex,
        HexUpperCase,
        Hsl,
        Hsla,
        Auto,
        ColorName
    }
}
