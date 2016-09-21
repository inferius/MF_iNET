using System;
using System.Drawing;
using System.Globalization;

namespace INetCore.Core.Language.CSS.Values
{
    public class HSLColor
    {
        public double Hue { get; set; }
        public double Luminosity { get; set; }
        public double Saturation { get; set; }

        public Color Color { get; set; }

        private static double degre60 = (Math.PI/180)*60;

        public HSLColor(Color color)
        {
            FromRGB(color.R, color.G, color.B);
        }

        public HSLColor(byte R, byte G, byte B)
        {
            FromRGB(R, G, B);
        }
        public HSLColor(double hue, double saturation, double luminosity)
        {
            Hue = hue;
            Luminosity = luminosity;
            Saturation = saturation;
            Color = ToRgb(); //ToRGB();
        }

        private void FromRGB(byte R, byte G, byte B)
        {
            var _R = (R / 255d);
            var _G = (G / 255d);
            var _B = (B / 255d);

            var _Min = Math.Min(Math.Min(_R, _G), _B);
            var _Max = Math.Max(Math.Max(_R, _G), _B);
            var _Delta = _Max - _Min;

            var H = 0d;
            var S = 0d;
            var L = (_Max + _Min) / 2.0d;

            if (_Delta != 0)
            {
                if (L < 0.5d)
                {
                    S = _Delta / (_Max + _Min);
                }
                else
                {
                    S = _Delta / (2.0f - _Max - _Min);
                }


                if (_R == _Max)
                {
                    //H = (_G - _B) / _Delta;
                    H = (((_G - _B) / _Delta) % 6) * degre60;
                }
                else if (_G == _Max)
                {
                    H = (2f + (_B - _R) / _Delta) * degre60;
                }
                else if (_B == _Max)
                {
                    H = (4f + (_R - _G) / _Delta) * degre60;
                }
            }

            Hue = H;
            Saturation = S;
            Luminosity = L;
            Color = Color.FromArgb(R, G, B);
        }

        public Color ToRGB()
        {
            byte r, g, b;
            if (Saturation == 0)
            {
                r = (byte)Math.Round(Luminosity * 255d);
                g = (byte)Math.Round(Luminosity * 255d);
                b = (byte)Math.Round(Luminosity * 255d);
            }
            else
            {
                double t2;
                var th = Hue / 6.0d;

                if (Luminosity < 0.5d)
                {
                    t2 = Luminosity * (1d + Saturation);
                }
                else
                {
                    t2 = (Luminosity + Saturation) - (Luminosity * Saturation);
                }
                var t1 = 2d * Luminosity - t2;

                var tr = th + (1.0d / 3.0d);
                var tg = th;
                var tb = th - (1.0d / 3.0d);

                tr = ColorCalc(tr, t1, t2);
                tg = ColorCalc(tg, t1, t2);
                tb = ColorCalc(tb, t1, t2);
                r = (byte)Math.Round(tr * 255d);
                g = (byte)Math.Round(tg * 255d);
                b = (byte)Math.Round(tb * 255d);
            }
            return Color.FromArgb(r, g, b);
        }

        public Color ToRgb()
        {
            if (Saturation == 0)
            {
                return ccalc(Luminosity, Luminosity, Luminosity, 0);
            }

            var C = (1 - Math.Abs(2*Luminosity - 1)*Saturation);
            var X = C*(1 - Math.Abs((Hue/degre60)%2 - 1));
            var m = Luminosity - C/2;
            var degree = Hue * 180 / Math.PI;

            if (degree >= 0 && degree < 60) return ccalc(C, X, 0, m);
            if (degree >= 60 && degree < 120) return ccalc(X, C, 0, m);
            if (degree >= 120 && degree < 180) return ccalc(0, C, X, m);
            if (degree >= 180 && degree < 240) return ccalc(0, X, C, m);
            if (degree >= 240 && degree < 300) return ccalc(X, 0, C, m);
            if (degree >= 300 && degree < 360) return ccalc(C, 0, X, m);

            return ToRGB();
        }

        private static Color ccalc(double r, double g, double b, double m)
        {
            return Color.FromArgb((int) Math.Round((r + m)*255), (int)Math.Round((g + m)*255), (int)Math.Round((b + m)*255));
        }
        public override string ToString()
        {
            var h = Hue*180/Math.PI;
            FormattableString fs = $"{h:0.#}, {Saturation:0.#%}, {Luminosity:0.#%}";
            return fs.ToString(CultureInfo.InvariantCulture);
        }

        private static double ColorCalc(double c, double t1, double t2)
        {

            if (c < 0) c += 1d;
            if (c > 1) c -= 1d;
            if (6.0d * c < 1.0d) return t1 + (t2 - t1) * 6.0d * c;
            if (2.0d * c < 1.0d) return t2;
            if (3.0d * c < 2.0d) return t1 + (t2 - t1) * (2.0d / 3.0d - c) * 6.0d;
            return t1;
        }
    }
}
