using System;
using System.Collections.Generic;
using System.Globalization;
using INetCore.Core.Language.CSS.Styles;

namespace INetCore.Core.Language.CSS
{
    public static class StylesRegistrator
    {
        private static Dictionary<string, Type> _registredStyles = new Dictionary<string, Type>(50);

        public static void RegisterStyle(string styleName, Type styleParserBase)
        {
            var name = styleName.ToLower(CultureInfo.InvariantCulture);

            if (_registredStyles.ContainsKey(name)) _registredStyles[name] = styleParserBase;
            else _registredStyles.Add(name, styleParserBase);
        }

        public static Type GetStyle(string styleName)
        {
            var name = styleName.ToLower(CultureInfo.InvariantCulture);

            return _registredStyles.ContainsKey(name) ? _registredStyles[name] : null;
        }

        static StylesRegistrator()
        {
            RegisterStyle("position", typeof(Styles.Position));
            RegisterStyle("left", typeof(Styles.Left));
            RegisterStyle("right", typeof(Styles.Right));
            RegisterStyle("top", typeof(Styles.Top));
            RegisterStyle("bottom", typeof(Styles.Bottom));
            RegisterStyle("width", typeof(Styles.Width));
            RegisterStyle("height", typeof(Styles.Height));
            RegisterStyle("float", typeof(Styles.Float));
            RegisterStyle("background", typeof(Styles.Background));
            RegisterStyle("background-color", typeof(Styles.BackgroundColor));
        }
    }
}
