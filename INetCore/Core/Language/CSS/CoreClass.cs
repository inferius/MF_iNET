using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using INetCore.Drawing.Objects;

namespace INetCore.Core.Language.CSS
{
    public partial class CoreClass
    {
        private List<CssDefinition> _classesStyle = new List<CssDefinition>();
        private List<CssProperty> _inlineStyle = new List<CssProperty>();


        public void ApplyStyle(BaseObject obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException();
            }
            #region Aplikace
            foreach (var prop in _classesStyle)
            {
            }
            #endregion

        }

        public static void ApplyProperty(BaseObject obj, CssProperty prop)
        {
        }

    }

    public class CssDefinition
    {
        private List<string> _cssNames = new List<string>();
        private List<CssProperty> _cssProperty = new List<CssProperty>();
    }

    public class CssProperty
    {
        private string _name;
        private string _value;

        #region Properties
        public string Name
        {
            get { return this._name; }
            set { this._name = value; }
        }

        public string Value
        {
            get { return this._value; }
            set { this._value = value; }
        }
        #endregion

        #region Private methods
        private int CovertCalcToPx(string calc_string, BaseObject parentObject)
        {
            int ret_px = 0;
            string buf = "";

            if (calc_string.StartsWith("calc("))
            {
                buf = calc_string.Substring("calc(".Length);
                buf = buf.Remove(buf.Length - 1);
            }

            return ret_px;
        }
        #endregion

        public CssProperty(string name, string value)
        {
            this._name = name;
            this._value = value;
        }
    }
}
