using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Linq;
using INetCore.Core.Language.CSS;
using INetCore.Core.Language.CSS.Styles;
using INetCore.Core.Language.HTML;

namespace INetCore.Drawing.Objects
{
    public class BaseObject : Object
    {
        Dictionary<Location, Border> _border = new Dictionary<Location, Border>(4);
        private DisplayProperty _display = new DisplayProperty();
        private BaseObject _parent = null;  // nadrazeny object
        private MeasuredUnit _width = 0;
        private MeasuredUnit _height = 0;
        //private int _realWidth = 0;
        //private int _realHeight = 0;
        //private Unit _widthUnit = Unit.Pixels;
        //private Unit _heightUnit = Unit.Pixels;
        private BrowserWindow _browser;
        private Position _position;
        //private Position _realPosition;
        private PositionType _positionType;
        private Position _padding;
        private Background _background;
        private Font _font;
        private List<BaseObject> _childrens = new List<BaseObject>();
        private string _innerText = "";
        private Float _float = Float.None;
        private int _zIndex = 0;
        private float _opacity = 1;
        private HtmlAgilityPack.HtmlNode _objType = null;
        private Core.Language.HTML.CoreClass _htmlCoreClass = null;

        internal Dictionary<string, BaseStyle> _cssProperties = new Dictionary<string, BaseStyle>(); // pouzite vlastnosti

        private string _objectName;
        private List<Style> _usedStyle = new List<Style>();

        #region Property
        /// <summary>
        /// Seznam použitých stylů
        /// </summary>
        public Style[] UsedStyles => _usedStyle.ToArray();
        public DisplayProperty Display
        {
            get { return _display; }
            set { _display = value; }
        }

        public Font Font
        {
            get { return _font; }
            set { _font = value; }
        }
        public HtmlAgilityPack.HtmlNode ObjectType
        {
            get { return _objType; }
            set { _objType = value; }
        }
        public float Opacity
        {
            get { return _opacity; }
            set
            {
                if (value < 0 || value > 1)
                {
                    throw new ArgumentOutOfRangeException("Minimalni hodnota 0. Maximální 1.");
                }
                _opacity = value;
            }
        }
        public int zIndex
        {
            get { return _zIndex; }
            set
            {
                //_parent.Childrens.Sort()
                _zIndex = value;
            }
        }
        public Float Float
        {
            get { return _float; }
            set { _float = value; }
        }
        public string InnerText
        {
            get { return _innerText; }
            set { _innerText = value; }
        }
        public string ObjectName
        {
            get { return _objectName; }
            set { _objectName = value; }
        }
        public BaseObject Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        public Border BorderLeft
        {
            get { return _border[Location.Left]; }
            set { _border[Location.Left] = value; }
        }
        public Border BorderRight
        {
            get { return _border[Location.Right]; }
            set { _border[Location.Right] = value; }
        }
        public Border BorderTop
        {
            get { return _border[Location.Top]; }
            set { _border[Location.Top] = value; }
        }
        public Border BorderBottom
        {
            get { return _border[Location.Bottom]; }
            set { _border[Location.Bottom] = value; }
        }
        public MeasuredUnit Width
        {
            get { return _width; }
            set { _width = value; }
        }
        public MeasuredUnit Height
        {
            get { return _height; }
            set { _height = value; }
        }
        //public Unit WidthUnit
        //{
        //    get { return _widthUnit; }
        //    set { _widthUnit = value; }
        //}
        //public Unit HeightUnit
        //{
        //    get { return _heightUnit; }
        //    set { _heightUnit = value; }
        //}
        public Position Padding
        {
            get { return _padding; }
            set { _padding = value; }
        }
        public BrowserWindow Browser
        {
            get { return _browser; }
            set { _browser = value; }
        }
        public Position ObjectPosition
        {
            get { return _position; }
            set { _position = value;
                _position.Top.RealValue = value.Top?.Value ?? float.NaN;
                _position.Left.RealValue = value.Left?.Value ?? float.NaN;
                _position.Right.RealValue = value.Right?.Value ?? float.NaN;
                _position.Bottom.RealValue = value.Bottom?.Value ?? float.NaN;
            }
        }
        //public Position ObjectRealPosition
        //{
        //    get { return _realPosition; }
        //    set { _realPosition = value; }
        //}
        public PositionType PositionType
        {
            get { return _positionType; }
            set { _positionType = value; }
        }
        public Background Background
        {
            get { return _background; }
            set { _background = value; }
        }
        public List<BaseObject> Childrens
        {
            get { return _childrens; }
            set { _childrens = value; }
        }
        #endregion

        #region Static Methods
        public static BaseObject ParseFromTag(HtmlAgilityPack.HtmlNode tag, BaseObject parentObject = null)
        {
            BaseObject b = new BaseObject(parentObject);

            #region Default Setting
            b.Height.Unit = Unit.Pixels;
            b.Width.Unit = Unit.Percentage;
            b.Width = 100;
            #endregion

            if (tag.Name == "div")
            {
                b.ObjectType = tag;
            }

            return b;
        }

        public static Unit ParseUnit(string input)
        {
            string i = input.ToLower().Trim();
            if (i == "px") return Unit.Pixels;
            if (i == "pt") return Unit.Poshort;
            if (i == "%") return Unit.Percentage;
            if (i == "ex") return Unit.Ex;
            if (i == "em") return Unit.Em;
            if (i == "pc") return Unit.Pica;
            if (i == "mm") return Unit.Milimeter;
            if (i == "cm") return Unit.Centimeter;
            if (i == "rem") return Unit.RootEm;


            return Unit.Pixels;
        }
        #endregion

        public BaseObject(BrowserWindow browserWindow, BaseObject parent = null)
        {
            _htmlCoreClass = new Core.Language.HTML.CoreClass();
            _parent = parent;
            _browser = browserWindow;
            Init();
        }

        public BaseObject(BaseObject parent)
        {
            _parent = parent;
            _browser = parent.Browser;
            _htmlCoreClass = _parent._htmlCoreClass;
            parent.Childrens.Add(this);
            Init();
        }

        
        private void Init()
        {
            // nastaveni vychozích dat
            _border[Location.Top] = new Border();
            _border[Location.Left] = new Border();
            _border[Location.Bottom] = new Border();
            _border[Location.Right] = new Border();
            _padding = new Position(0,0,0,0);
            _position = new Position();
            _positionType = PositionType.Static;
            _position.Left = 0;
            //_position.Left.Unit = Unit.Pixels;
            _position.Top = 0;
            //_position.Top.Unit = Unit.Pixels;
            _background = new Background();
            _background.Color = Color.Transparent;
            _browser.HorizontalScroll.Enabled = true;
            _browser.VerticalScroll.Enabled = true;
            _font = new Font();

            //_objType = new Core.Language.HTML.HtmlTag
            //{
            //    TagName = "div",
            //    TagType = _htmlCoreClass.GetTagDefinitionByTagName("div")
            //};
        }

        public void ApplyProperty(Core.Language.CSS.Styles.BaseStyle prop)
        {
            prop.ApplyStyle(this);
        }

        public void ApplyStyles(string styles)
        {
            Core.Language.CSS.CoreClass.ApplyStyles(this, styles);
        }

        public void ApplyStyles(Style style)
        {
            _usedStyle.Add(style);

            foreach (var s in style.Styles)
            {
                ApplyProperty(s);
            }
        }


        public string ToHTML()
        {
            StringBuilder ret = new StringBuilder();

            ret.AppendFormat("<{0}", _objType.Name);

            var style = GenerateStyleString();
            if (style.Length > 0)
            {
                _objType.Attributes.Add("style", style);
                //_objType.Attributes["style"].Value = style;
                //_objType.HtmlAttributes.Add(new Core.Language.HTML.HtmlAttribute("style", style));
            }

            return _objType.OuterHtml;
        }

        #region Generovani style stringu
        public string GenerateStyleString()
        {
            StringBuilder s = new StringBuilder();

            _browser.HorizontalScroll.Enabled = true;
            _browser.VerticalScroll.Enabled = true;
            _font = new Font();

            // pripojeni border style
            s.Append(_generateBorderStyle());
            if (_positionType != PositionType.Static) s.Append($"position: {_positionType.ToString().ToLower()}; ");
            if (!float.IsNaN(_position.Top.Value)) s.Append($"top: {_position.Top}; ");
            if (!float.IsNaN(_position.Left.Value)) s.Append($"left: {_position.Left}; ");
            if (!float.IsNaN(_position.Bottom.Value)) s.Append($"bottom: {_position.Bottom}; ");
            if (!float.IsNaN(_position.Right.Value)) s.Append($"right: {_position.Right}; ");

            if (!Width.IsDynamicSet) s.Append($"width: {Width}; ");
            if (!Height.IsDynamicSet) s.Append($"height: {Height}; ");

            if (Float != Float.None) s.Append($"float: {Float.ToString().ToLower()}; ");
            

            if (!Background.IsDefault) s.Append(Background);

            return s.ToString();
        }

        private string _generateBorderStyle()
        {
            StringBuilder s = new StringBuilder();

            if (!_border[Location.Top].IsDefault) s.AppendFormat("border-top: {0}; ", _border[Location.Top].ToBorderNormalString());
            if (!_border[Location.Left].IsDefault) s.AppendFormat("border-left: {0}; ", _border[Location.Left].ToBorderNormalString());
            if (!_border[Location.Bottom].IsDefault) s.AppendFormat("border-bottom: {0}; ", _border[Location.Bottom].ToBorderNormalString());
            if (!_border[Location.Right].IsDefault) s.AppendFormat("border-right: {0}; ", _border[Location.Right].ToBorderNormalString());

            return s.ToString();
        }
        #endregion

        public bool IsPointInObject(DrawingPoint p)
        {
            Dictionary<string, DrawingPoint> _p = getRealPosition();
            //Console.WriteLine("Base object position: " + _p["lt"].ToString() + ", " + _p["rt"].ToString() + ", " + _p["lb"].ToString());
            // TODO: Doladit pro nepravouhle tvary
            if (p.X >= _p["lt"].X && p.X <= _p["rt"].X &&
                p.Y >= _p["lt"].Y && p.Y <= _p["lb"].Y)
            {
                return true;
            }

            return false;
        }

        public DrawingPoint GetRealStartPositionInnerText()
        {
            //return new DrawingPoint(ConvertUnit(Padding.Left, Padding.Left.Unit) + ConvertUnit(ObjectPosition.Left, ObjectPosition.Left.Unit), ConvertUnit(Padding.Top, Padding.Top.Unit) + ConvertUnit(ObjectPosition.Top, ObjectPosition.Top.Unit));
            return new DrawingPoint(ConvertUnit(Padding.Left, Padding.Left.Unit) + ObjectPosition.Left.RealValue, ConvertUnit(Padding.Top, Padding.Top.Unit) + ObjectPosition.Top.RealValue);
        }

        // Vrati skutecnou polohu objektu ve strance v pixelech
        // ret["lt"],ret["lb"],ret["rt"],ret["rb"]
        public Dictionary<string, DrawingPoint> getRealPosition()
        {
            RealObject real;
            return getRealPositionWithRealObject(out real);
        }
        public Dictionary<string, DrawingPoint> getRealPositionWithRealObject(out RealObject realObject)
        {
            Dictionary<string, DrawingPoint> ret_p = new Dictionary<string, DrawingPoint>();
            realObject = new RealObject(this);

            float window_width = 0;
            float window_height = 0;

            #region Nastaveni rozmeru okna
            if (_parent != null)
            {
                window_height = _parent.Height.RealValue;
                window_width = _parent.Width.RealValue;
            }
            else
            {
                window_width = _browser.Width;
                window_height = _browser.Height;
            }
            #endregion

            DrawingPoint _lt = new DrawingPoint();
            DrawingPoint _lb = new DrawingPoint();
            DrawingPoint _rt = new DrawingPoint();
            DrawingPoint _rb = new DrawingPoint();

            #region Uprava velikosti podle jednotek
            if (Width.Unit == Unit.Percentage)
            {
                Width.RealValue = (int)((Width.Value / 100) * window_width);

                if (_positionType == PositionType.Relative)
                {
                    //if ()
                }
            }

            if (_parent == null)
            {
                Console.WriteLine("Šířka prvku: " + Width.RealValue);
                Console.WriteLine("Šířka prohlížeče: " + window_width);
            }

            if (Height.Unit == Unit.Percentage)
            {
                Height.RealValue = (int)((Height.Value / 100) * window_height);
            }
            realObject.Height = (int)Height.RealValue;
            realObject.Width = (int)Width.RealValue;
            #endregion

            #region Nastaveni pozice dle typu

            #region Nastaveni posunu
            bool finded = true;
            // promenne pro odecteni puvodních hodnot pri staticke pozici
            float static_left_diff = _position.Left.RealValue,
                static_top_diff = _position.Top.RealValue;
            if (Float == Float.Right)
            {
                _position.Left.RealValue = window_width;
            }
            if (_parent != null)
            {
                foreach (BaseObject bo in _parent.Childrens)
                {
                    if (finded && (bo._positionType == PositionType.Static || bo._positionType == PositionType.Relative) && bo != this && bo.Float == Float.None)
                    {
                        _position.Top.RealValue += bo.Height.Value;
                    }
                    else if (finded && bo != this && bo.Float == Float.Left && (bo._positionType == PositionType.Static || bo._positionType == PositionType.Relative))
                    {
                        if (Float == Float.Left)
                        {
                            _position.Left.RealValue += bo.Width.Value;
                        }
                    }
                    else if (finded && bo != this && bo.Float == Float.Right && (bo._positionType == PositionType.Static || bo._positionType == PositionType.Relative))
                    {
                        if (Float == Float.Right)
                        {
                            _position.Left.RealValue -= bo.Width.Value;
                        }
                    }
                    else
                    {
                        if (bo == this)
                        {
                            finded = false;
                            break;
                        }
                    }
                }
            }
            #endregion

            if (_positionType == PositionType.Static)
            {
                if (_parent != null)
                {
                    _position.Left.RealValue += _parent._position.Left.RealValue - static_left_diff;
                    _position.Top.RealValue += _parent._position.Top.RealValue - static_top_diff;

                    _lt = new DrawingPoint(_position.Left.RealValue, _position.Top.RealValue);
                    _lb = new DrawingPoint(_position.Left.RealValue, _position.Top.RealValue + Height.RealValue);
                    _rt = new DrawingPoint(_position.Left.RealValue + Width.RealValue, _position.Top.RealValue);
                    _rb = new DrawingPoint(_position.Left.RealValue + Width.RealValue, _position.Top.RealValue + Height.RealValue);
                }
                else
                {
                    _lt = new DrawingPoint(0, 0);
                    _lb = new DrawingPoint(0, Height.RealValue);
                    _rt = new DrawingPoint(Width.RealValue, 0);
                    _rb = new DrawingPoint(Width.RealValue, Height.RealValue);
                }


            }
            else if (_positionType == PositionType.Relative)
            {
                if (_parent != null)
                {
                    _position.Left.RealValue += _parent._position.Left.RealValue;
                    _position.Top.RealValue += _parent._position.Top.RealValue;

                    _lt = new DrawingPoint(_position.Left.RealValue, _position.Top.RealValue);
                    _lb = new DrawingPoint(_position.Left.RealValue, _position.Top.RealValue + Height.RealValue);
                    _rt = new DrawingPoint(_position.Left.RealValue + Width.RealValue, _position.Top.RealValue);
                    _rb = new DrawingPoint(_position.Left.RealValue + Width.RealValue, _position.Top.RealValue + Height.RealValue);
                }
                else
                {
                    _lt = new DrawingPoint(_position.Left.RealValue, _position.Top.RealValue);
                    _lb = new DrawingPoint(_position.Left.RealValue, _position.Top.RealValue + Height.RealValue);
                    _rt = new DrawingPoint(_position.Left.RealValue + Width.RealValue, _position.Top.RealValue);
                    _rb = new DrawingPoint(_position.Left.RealValue + Width.RealValue, _position.Top.RealValue + Height.RealValue);
                }

            }
            else if (_positionType == PositionType.Absolute)
            {
                _position.Left.RealValue += _parent._position.Left.RealValue;
                _position.Top.RealValue += _parent._position.Top.RealValue;

                _lt = new DrawingPoint(_position.Left.RealValue, _position.Top.RealValue);
                _lb = new DrawingPoint(_position.Left.RealValue, _position.Top.RealValue + Height.RealValue);
                _rt = new DrawingPoint(_position.Left.RealValue + Width.RealValue, _position.Top.RealValue);
                _rb = new DrawingPoint(_position.Left.RealValue + Width.RealValue, _position.Top.RealValue + Height.RealValue);
            }
            #endregion

            realObject.Top = _position.Top.RealValue;
            realObject.Left = _position.Left.RealValue;

            ret_p.Add("lt", _lt);
            ret_p.Add("lb", _lb);
            ret_p.Add("rt", _rt);
            ret_p.Add("rb", _rb);

            return ret_p;
        }

        /// <summary>
        /// Obnovi data pro vykresleni, aby dalsi vykresleni probehlo identicky
        /// </summary>
        public void RefreshAfterDraw()
        {
            //ObjectRealPosition = ObjectPosition;
            if (_position.Top == null) _position.Top = new MeasuredUnit();
            if (_position.Left == null) _position.Left = new MeasuredUnit();
            if (_position.Right == null) _position.Right = new MeasuredUnit();
            if (_position.Bottom == null) _position.Bottom = new MeasuredUnit();

            _position.Top.RealValue = ObjectPosition.Top?.Value ?? float.NaN;
            _position.Left.RealValue = ObjectPosition.Left?.Value ?? float.NaN;
            _position.Right.RealValue = ObjectPosition.Right?.Value ?? float.NaN;
            _position.Bottom.RealValue = ObjectPosition.Bottom?.Value ?? float.NaN;
        }

        public float ConvertUnit(float input, Unit from, Unit to = Unit.Pixels)
        {
            return MeasuredUnit.ConvertUnit(input, from, to, _browser.CreateGraphics().DpiX);
        }

        public void Draw()
        {
            Graphics gfx = _browser.CreateGraphics();
            Pen _left = new Pen(BorderLeft.Color, BorderLeft.Width);
            Pen _right = new Pen(BorderRight.Color, BorderRight.Width);
            Pen _top = new Pen(BorderTop.Color, BorderTop.Width);
            Pen _bottom = new Pen(BorderBottom.Color, BorderBottom.Width);

            #region Nastaveni vypisu text
            SolidBrush drawBrush = new SolidBrush(_font.Color);
            string drawString = _innerText;
            System.Drawing.Font drawFont = _font.Fonts;
            #endregion

            #region Nastaveni stylu borderu
            //if (_border[Location.Left].
            #endregion

            //int window_width = 0;
            //int window_height = 0;

            #region Nastaveni rozmeru okna
            //if (_parent != null)
            //{
            //    window_height = _parent._realHeight;
            //    window_width = _parent._realWidth;
            //}
            //else
            //{
            //    window_width = _browser.Width;
            //    window_height = _browser.Height;
            //}
            #endregion

            DrawingPoint _lt = new DrawingPoint();
            DrawingPoint _lb = new DrawingPoint();
            DrawingPoint _rt = new DrawingPoint();
            DrawingPoint _rb = new DrawingPoint();

            Dictionary<string, DrawingPoint> _p = getRealPosition();
            _lt = _p["lt"];
            _lb = _p["lb"];
            _rt = _p["rt"];
            _rb = _p["rb"];

            _background.Draw(gfx, _lt, _lb, _rt, _rb, _opacity);

            gfx.DrawLine(_left, _lt, _lb);
            gfx.DrawLine(_right, _rt, _rb);
            gfx.DrawLine(_top, _lt, _rt);
            gfx.DrawLine(_bottom, _lb, _rb);
            // vypis textu
            // TODO: Doplnit padding
            gfx.DrawString(drawString, drawFont, drawBrush, _position.Left, _position.Top);

            foreach (BaseObject b in _childrens)
            {
                b.Draw();
            }
            RefreshAfterDraw();
        }

        [Obsolete("Metoda je zastaralá použíjte novou statickou metodu 'UnitToString' na třídě 'MeasuredUnit'")]
        public static string GetUnitString(Unit unit)
        {
            return MeasuredUnit.UnitToString(unit);
        }

        public BaseObject CreateStructure(IEnumerable<HtmlAgilityPack.HtmlNode> tags, BrowserWindow bw)
        {
            var obj = new BaseObject(bw);

            obj.Childrens = createStructure(obj, tags);

            return obj;
        }

        private List<BaseObject> createStructure(BaseObject parent, IEnumerable<HtmlAgilityPack.HtmlNode> tags)
        {
            var objs = new List<BaseObject>(tags.Count());

            foreach (var htmlTag in tags)
            {
                var obj = new BaseObject(parent);
                obj.ObjectType = htmlTag;
                obj.Childrens = createStructure(obj, htmlTag.ChildNodes);
            }

            return objs;
        }
    }

    public enum Float
    {
        Left,
        Right,
        Inherit,
        None
    }

    public enum PositionType
    {
        Absolute,
        Relative,
        Fixed,
        Inherit,
        Static
    }

    public class Position
    {
        public Position(float left = float.NaN, float top = float.NaN, float right = float.NaN, float bottom = float.NaN)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }

        #region Property
        public MeasuredUnit Left { get; set; }
        public MeasuredUnit Right { get; set; }
        public MeasuredUnit Top { get; set; }
        public MeasuredUnit Bottom { get; set; }
        #endregion
    }

    

    public enum Location
        {
            Top,
            Bottom,
            Left,
            Right
        }


        public enum Unit
        {
            Percentage, // %
            Inch, // in
            Centimeter, // cm
            Milimeter, // mm
            Em,
            Ex,
            Poshort, // pt
            Pica, // pc
            Pixels, // px
            RootEm // rem
        }
    }
