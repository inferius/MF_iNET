using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace INetCore.Drawing.Objects
{
    public class Background
    {
        #region Private definition
        private Size _size;
        private Color _color;
        private Repeat _repeat;
        private string _img;
        private Position _position;
        #endregion

        #region Property
        public Size SizeBackground
        {
            get { return _size; }
            set { _size = value; }
        }

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }
        public Repeat RepeatBackground
        {
            get { return _repeat; }
            set { _repeat = value; }
        }
        public Position Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public bool IsDefault => _isDefault();
        #endregion

        public Background()
        {
            _size = new Size
            {
                Width = 1,
                WidthUnit = Unit.Pixels,
                Height = 1,
                HeightUnit = Unit.Pixels
            };
            _color = Color.Transparent;
            _img = "";
            _repeat = Repeat.Repeat;
            _position = new Position();
        }

        private bool _isDefault()
        {
            return false;
        }

        public override string ToString()
        {
            // TODO: Doladit kompletni background
            return $"background-color: {ColorTranslator.ToHtml(Color).ToLower()};";
        }

        public void Draw(Graphics gfx, Point leftTop, Point leftBottom, Point rightTop, Point rightBottom, float opacity = 1)
        {
            Pen p;
            if (opacity != 1)
            {
                p = new Pen(Color.FromArgb((int)(255 * opacity), _color));
            }
            else
            {
                p = new Pen(_color);
            }
            if (RepeatBackground == Repeat.Repeat)
            {
                for (int i = leftTop.Y; i < leftBottom.Y; i++)
                {
                    gfx.DrawLine(p, new Point(leftTop.X, i), new Point(rightTop.X, i));
                }
            }
        }

        public enum Repeat
        {
            NoRepeat,
            Repeat,
            Inherit,
            RepeatX,
            RepeatY
        }

        public struct Size
        {
            private float _width;
            private float _height;
            private Unit _widthUnit;
            private Unit _heightUnit;

            #region Property
            public float Width
            {
                get { return _width; }
                set { _width = value; }
            }

            public float Height
            {
                get { return _height; }
                set { _height = value; }
            }

            public Unit WidthUnit
            {
                get { return _widthUnit; }
                set { _widthUnit = value; }
            }

            public Unit HeightUnit
            {
                get { return _heightUnit; }
                set { _heightUnit = value; }
            }
            #endregion

            public Size(int s = 0)
            {
                _width = -1;
                _height = -1;
                _widthUnit = Unit.Pixels;
                _heightUnit = Unit.Pixels;
            }

            #region Static method
            public static Size Parse(string input)
            {
                Size s = new Size();
                string i = input.ToLower().Trim();
                if (i == "cover")
                {
                    s.Width = 100;
                    s.WidthUnit = Unit.Percentage;
                }
                else if (i == "contain")
                {
                    s.Height = 100;
                    s.HeightUnit = Unit.Percentage;
                }
                else
                {
                    short count = 0;
                    StringBuilder buf = new StringBuilder();

                    for (short j = 0; j < i.Length; j++)
                    {
                        if (char.IsDigit(i[j]))
                        {
                            buf.Append(i[j]);
                        }
                        else if (char.IsLetter(i[j]) || i[j] == '%')
                        {
                            // zapis width
                            if (count == 0)
                            {
                                s.Width = float.Parse(buf.ToString());
                                buf.Clear();
                                count++;
                            }
                            if (count == 2)
                            {
                                s.Height = float.Parse(buf.ToString());
                                buf.Clear();
                                count++;
                            }

                            buf.Append(i[j]);
                        }
                        else if (char.IsWhiteSpace(i[j]))
                        {
                            // zapis width
                            if (count == 0)
                            {
                                s.Width = float.Parse(buf.ToString());
                                buf.Clear();
                                count++;
                            }
                            if (count == 2)
                            {
                                s.Height = float.Parse(buf.ToString());
                                buf.Clear();
                                count++;
                            }
                            // width unit
                            if (count == 1)
                            {
                                s.WidthUnit = BaseObject.ParseUnit(buf.ToString());
                                buf.Clear();
                                count++;
                            }
                            if (count == 3)
                            {
                                s.HeightUnit = BaseObject.ParseUnit(buf.ToString());
                                buf.Clear();
                                count++;
                            }
                        }
                    }
                    if (count == 3)
                    {
                        s.HeightUnit = BaseObject.ParseUnit(buf.ToString());
                        buf.Clear();
                        count++;
                    }
                    if (count <= 2)
                    {
                        if (s.Height == -1)
                        {
                            s.Height = s.Width;
                            s.HeightUnit = s.WidthUnit;
                        }
                    }
                }

                return s;
            }
            #endregion

        }
    }
}
