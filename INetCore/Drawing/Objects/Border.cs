using System.Drawing;

namespace INetCore.Drawing.Objects
{
    public class Border
    {
        #region Private Definition
        private MeasuredUnit _width;
        private BorderStyle _borderStyle;
        private Color _color;
        private float _radius;
        private Unit _radiusUnit;
        private string _image;
        #endregion

        #region Property
        public MeasuredUnit Width
        {
            get { return _width; }
            set { _width = value; }
        }

        public BorderStyle Style
        {
            get { return _borderStyle; }
            set { _borderStyle = value; }
        }

        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public float Radius
        {
            get { return _radius; }
            set { _radius = value; }
        }

        public Unit RadiusUnit
        {
            get { return _radiusUnit; }
            set { _radiusUnit = value; }
        }

        public bool IsDefault => _isDefault();

        #endregion

        public Border()
        {
            _width = 0;
            //_widthUnit = Unit.Pixels;
            _borderStyle = BorderStyle.None;
            //_color = new Color();
            _radius = 0;
            _radiusUnit = Unit.Pixels;
            _image = "none";
            _color = Color.Black;
        }

        private bool _isDefault()
        {
            return Width == 0 && Style == BorderStyle.None && Radius == 0 && Color == Color.Black && _image == "none";
        }

        #region Equals operator

        public static bool operator ==(Border b1, Border b2)
        {
            if (b1 == null && b2 == null) return true;
            if (b1 == null || b2 == null) return false;
            return b1.Color == b2.Color && b1.Style == b2.Style && b1.Width == b2.Width && b1.Width.Unit == b2.Width.Unit && b1.Radius == b2.Radius && b1.RadiusUnit == b2.RadiusUnit;
        }
        public static bool operator !=(Border b1, Border b2)
        {
            if (b1 == null && b2 == null) return false;
            if (b1 == null || b2 == null) return true;
            return !(b1.Color == b2.Color && b1.Style == b2.Style && b1.Width == b2.Width && b1.Width.Unit == b2.Width.Unit && b1.Radius == b2.Radius && b1.RadiusUnit == b2.RadiusUnit);
        }

        public static bool EqualRadius(Border b1, Border b2)
        {
            return b1.Radius == b2.Radius && b1.RadiusUnit == b2.RadiusUnit;
        }

        public static bool EqualWithoutRadius(Border b1, Border b2)
        {
            return b1.Color == b2.Color && b1.Style == b2.Style && b1.Width == b2.Width && b1.Width.Unit == b2.Width.Unit;
        }
        #endregion

        public string ToBorderNormalString()
        {
            return $"{Width} {Style.ToString().ToLower()} {ColorTranslator.ToHtml(Color).ToLower()}";
        }

        public enum BorderStyle
        {
            None,
            Hidden,
            Dotted,
            Dashed,
            Solid,
            Double,
            Groove,
            Ridge,
            Inset,
            Outset
        }

    }
}
