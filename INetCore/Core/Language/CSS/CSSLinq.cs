using System.Collections.Generic;
using System.IO;
using System.Text;
using INetCore.Drawing.Objects;

namespace INetCore.Core.Language.CSS
{
    public class CSSLinq
    {
        private BaseObject rootElement;
        private BaseObject[] objects;
        private List<Style> styles = new List<Style>(20);
        public CSSLinq(BaseObject[] listObjects)
        {
            objects = listObjects;
        }

        public void LoadStyleFromFile(string file)
        {
            LoadStyle(File.ReadAllText(file));
        }

        public void LoadStyle(string styles)
        {
            var tempStyle = new Style();
            var tempString = new StringBuilder(100);
            var inComment = false;
            // pokud se v zápisu objeví pseudotag (třeba :hover), dostane styl +1b k priroitě
            // pokud se v zápisu objeví tag, dostane styl +2b k prioritě
            // pokud se v zápisu objeví třída, dostane styl +20b k prioritě
            // pokud se v zápisu objeví indentifikátor, dostane styl +200b k prioritě

            var styleName = false; // indikace zapisu jmena stylu
            for (int i = 0; i < styles.Length; i++)
            {
                var ch = styles[i];

                #region Kontrola komentaru
                if (ch == '*' && i + 1 < styles.Length && styles[i + 1] == '/')
                {
                    inComment = false;
                }

                if (inComment) continue;

                if (ch == '/' && i + 1 < styles.Length && styles[i + 1] == '*')
                {
                    inComment = true;
                }
                #endregion

                if (char.IsWhiteSpace(ch) && !styleName) continue;
                if (ch == '{' && i + 1 < styles.Length)
                {
                    int j;
                    var stemp = tempString.ToString().Split(','); // docasne ulozeni nazvu rozdeleni podle carek
                    tempString.Clear();
                    // začne číst obsah tagu a rozparsuje ho
                    for (j = i + 1; j < styles.Length; j++)
                    {
                        var chInner = styles[j];

                        if (chInner == '}')
                        {
                            styleName = false;
                            break;
                        }
                        tempString.Append(chInner);
                    }
                    var styleTemp = CoreClass.ParseFromString(tempString.ToString());
                    tempString.Clear();
                    // ulozeni stylu pokud jich je vic
                    foreach (var name in stemp)
                    {
                        tempStyle = new Style
                        {
                            FullName = name,
                            Priority = getPriority(name),
                            Styles = styleTemp
                        };
                        this.styles.Add(tempStyle);
                    }

                    i = j + 1;
                    continue;
                }
                if (!char.IsWhiteSpace(ch)) styleName = true;

                tempString.Append(ch);
            }
        }

        private int getPriority(string styleName)
        {
            //https://www.w3.org/TR/selectors/#specificity
            var priority = 0;
            var pseudo = false; // informace o tom, zda cteny text odpovida pseudo tride, nebo classe
            var letters = false; // informace pokud uz cte znaky
            var inAttr = false; // pokud je v atributu
            int a = 0, b = 0, c = 0, d = 0;

            foreach (var ch in styleName)
            {
                if (ch == ']') inAttr = false;
                if (inAttr) continue;

                if (ch == '+' || ch == '>' || ch == '~' || char.IsWhiteSpace(ch))
                {
                    letters = false;
                    pseudo = false;
                    continue;
                }

                if (ch == '.' || ch == '#' || ch == ':' || ch == '[')
                {
                    pseudo = true;
                    if (ch == '.') c++;
                    if (ch == '#') b++;
                    if (ch == ':') d++;
                    if (ch == '[')
                    {
                        c++;
                        inAttr = true;
                        continue;
                    }
                }
                if (char.IsLetterOrDigit(ch) || ch == '-' || ch == '_')
                {
                    if (!pseudo && !letters) d++;
                    letters = true;
                }

            }

            return a * 1000 + b * 100 + c * 10 + d;
        }
    }
}
