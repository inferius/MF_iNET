using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using INetCore.Core.Exception;
using INetCore.Core.Language.CSS.Styles;
using INetCore.Drawing.Objects;

namespace INetCore.Core.Language.CSS
{
    public partial class CoreClass
    {

        /// <summary>
        /// Naparsuje jednu sekci stylu tykajici se konkrentniho prvku
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        public static BaseStyle[] ParseFromString(string style)
        {
            //var baseSeparate = style.Split(new[] {";"}, StringSplitOptions.RemoveEmptyEntries);
            var commentedStyles = getCommentedStyle(style);
            var baseSeparateCommented = commentedStyles.Item1.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            var baseSeparateUncommented = commentedStyles.Item2.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            var commentedBstyles = baseSeparateCommented.Select(s => ParseStringToken(s, false)).Where(temp => temp != null);
            var uncommentedBstyles = baseSeparateUncommented.Select(s => ParseStringToken(s, true)).Where(temp => temp != null);

            return commentedBstyles.Concat(uncommentedBstyles).ToArray();
        }

        /// <summary>
        /// Vrati tuple, kde v prvni polozce jsou zakomentovane styly a v druhe nezakomentovane
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        private static Tuple<string, string> getCommentedStyle(string style)
        {
            var commented = new StringBuilder();
            var uncommented = new StringBuilder();

            var inComment = false;
            var inString = false;
            char startStringChar = '\0';
            for (var i = 0; i < style.Length; i++)
            {
                var c = style[i];
                if ((c == '"' || c == '\'') && !inComment)
                {
                    if (inString)
                    {
                        if (startStringChar == c)
                        {
                            if (style[i - 1] == '\\') continue;

                            inString = false;
                        }
                    }
                    else
                    {
                        inString = true;
                        startStringChar = c;
                    }
                }
                else if (c == '/' && !inString)
                {
                    if ((i + 1 < style.Length) && style[i + 1] == '*')
                    {
                        i++;
                        inComment = true;
                        continue;
                    }
                }
                else if (c == '*' && !inString && inComment)
                {
                    if ((i + 1 < style.Length) && style[i + 1] == '/')
                    {
                        i++;
                        inComment = false;
                        continue;
                    }
                }

                if (inComment) commented.Append(c);
                else uncommented.Append(c);
            }

            return new Tuple<string, string>(commented.ToString().Trim(), uncommented.ToString().Trim());
        }

        /// <summary>
        /// Rozparsuje stringovy token na BaseStyle
        /// </summary>
        /// <param name="styleToken">Stringovy token(ex. background-color: black or background: black no-repeat url(test.png)</param>
        /// <param name="isUsed">Informace o tom, zda je styl pouzity nebo ne</param>
        /// <returns></returns>
        private static BaseStyle ParseStringToken(string styleToken, bool isUsed = true)
        {
            //TODO: Doladit validaci podle definice
            var token = styleToken.Trim();
            if (token.Length == 0) return null;

            BaseStyle bs = null;
            var sepString = token.Split(new[] {":"}, StringSplitOptions.RemoveEmptyEntries);
            
            if (sepString.Length < 2) throw new NotValidCSSPropertyException(styleToken);

            var clName = StylesRegistrator.GetStyle(sepString[0].Trim());

            if (clName != null)
            {
                bs = Activator.CreateInstance(clName) as BaseStyle;
                bs?.ParseValue(sepString[1]);
                if (bs != null) bs.IsUsed = isUsed;
            }

            return bs;
        }

        public static void ApplyStyles(BaseObject bo, string styles)
        {
            ApplyStyles(bo, ParseFromString(styles));
        }
        public static void ApplyStyles(BaseObject bo, BaseStyle[] styles)
        {
            foreach (var baseStyle in styles)
            {
                if (baseStyle.IsUsed) baseStyle.ApplyStyle(bo);
            }
        }
    }
}
