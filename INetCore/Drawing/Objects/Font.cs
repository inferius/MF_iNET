using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace INetCore.Drawing.Objects
{
    public class Font
    {
        private Color _color;
        private System.Drawing.Font _font;

        #region Property
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        public System.Drawing.Font Fonts
        {
            get { return this._font; }
            set { this._font = value; }
        }
        #endregion

        public Font()
        {
            _color = Color.Black;
            _font = new System.Drawing.Font(FontFamily.GenericSansSerif, 12f);
        }
    }
}
