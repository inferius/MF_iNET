using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using INetCore.Core.Exception;
using INetCore.Drawing.Objects;

namespace INetCore.Core.Language.CSS
{
    public class CSSLinq
    {
        private BaseObject rootElement;
        private readonly List<BaseObject> objects = new List<BaseObject>();

        private Dictionary<string, BaseObject> _idMap = new Dictionary<string, BaseObject>(20);
        private Dictionary<string, List<BaseObject>> _tagsNameMap = new Dictionary<string, List<BaseObject>>(20);
        private Dictionary<string, List<BaseObject>> _tagsClassMap = new Dictionary<string, List<BaseObject>>(20);

        private readonly List<Style> styles = new List<Style>(20);
        public CSSLinq(BaseObject obj)
        {
            rootElement = obj;
            _findAllSubelements(obj, ref objects);
            loadMapObjects();
        }

        private void loadMapObjects()
        {
            foreach (var obj in objects)
            {
                var tagName = obj.ObjectType.Name;
                var classesList = obj.ObjectType.ClassList;
                var id = obj.ObjectType.Id;

                if (!string.IsNullOrEmpty(id) && !_idMap.ContainsKey(id)) _idMap.Add(id, obj);
                if (classesList.Count > 0)
                {
                    foreach (var cls in classesList)
                    {
                        if (!_tagsClassMap.ContainsKey(cls)) _tagsClassMap.Add(cls, new List<BaseObject>(5));
                        _tagsClassMap[cls].Add(obj);
                    }
                }

                if (!_tagsNameMap.ContainsKey(tagName)) _tagsNameMap.Add(tagName, new List<BaseObject>(5));
                _tagsNameMap[tagName].Add(obj);
            }
        }
        public void ApplyStyle()
        {
            var _styles = styles.OrderBy(item => item.Priority).ToArray();

            foreach (var style in _styles)
            {
                var objs = _findElements(style.Name, objects);

                foreach (var o in objs)
                {
                    o.ApplyStyles(style);
                }
            }

            foreach (var o in objects)
            {
                o.ApplyStyles(o.ObjectType.GetAttributeValue("style", ""));
            }
        }

        private IEnumerable<BaseObject> _findElements(CSSNameToken name, IEnumerable<BaseObject> objs = null)
        {
            var _objs = new List<BaseObject>();

            foreach (var o in objs)
            {
                if (name.ID != null)
                {
                    if (o.ObjectType.Id != name.ID) continue;
                }
                if (name.TagName != null)
                {
                    if (!(o.ObjectType.Name == name.TagName || name.TagName == "*")) continue;
                }
                if (name.ClassList != null && name.ClassList.Length > 0)
                {
                    if (!name.ClassList.Any(s => o.ObjectType.ClassList.Contains(s)))
                    {
                        continue;
                    }
                }
                if (name.Attrs != null && name.Attrs.Length > 0)
                {
                    if (name.Attrs.Any(s => o.ObjectType.Attributes.Any(item => CSSAttributeInfo.CheckStatus(s, item.Name, item.Name))))
                        continue;
                }
                if (name.PseudoTokens != null && name.PseudoTokens.Length > 0)
                {
                    if (name.PseudoTokens.Any(token => !token.Method(o)))
                    {
                        continue;
                    }
                }

                if (name.Child == null) _objs.Add(o);
                else
                {
                    var innerObjs = _findElementsByParent(name.Child, o);
                    if (innerObjs != null) _objs.AddRange(innerObjs);
                }
            }

            return _objs;
        }

        private static void _findAllSubelements(BaseObject obj, ref List<BaseObject> _objs)
        {
            if (obj.Childrens == null) return;

            foreach (var o in obj.Childrens)
            {
                _objs.AddRange(o.Childrens);
                _findAllSubelements(o, ref _objs);
            }
        }

        public static void FindAllSubelements(BaseObject obj, ref List<BaseObject> _objs)
            => _findAllSubelements(obj, ref _objs);

        private IEnumerable<BaseObject> _findElementsByParent(CSSNameToken name, BaseObject obj)
        {
            var _objs = new List<BaseObject>();

            var objs = new List<BaseObject>();
            // TODO: Dodelat jeste after a siblings, je treba na BaseObjects dodelat medotu next a previous siblings

            if (name.PreviousBinding == CSSTokenBinding.Childern)
            {
                if (obj.Childrens == null || obj.Childrens.Count == 0) return _objs;
                objs.AddRange(obj.Childrens);
            }
            if (name.PreviousBinding == CSSTokenBinding.All)
            {
                _findAllSubelements(obj, ref objs);
            }

            foreach (var o in objs)
            {
                if (name.ID != null)
                {
                    if (o.ObjectType.Id != name.ID) continue;
                }
                if (!string.IsNullOrEmpty(name.TagName))
                {
                    if (!(o.ObjectType.Name == name.TagName || name.TagName == "*")) continue;
                }
                if (name.ClassList != null && name.ClassList.Length > 0)
                {
                    if (!name.ClassList.Any(s => o.ObjectType.ClassList.Contains(s)))
                    {
                        continue;
                    }
                }
                if (name.Attrs != null && name.Attrs.Length > 0)
                {
                    if (name.Attrs.Any(s => o.ObjectType.Attributes.Any(item => CSSAttributeInfo.CheckStatus(s, item.Name, item.Name))))
                            continue;
                }
                if (name.PseudoTokens != null && name.PseudoTokens.Length > 0)
                {
                    if (name.PseudoTokens.Any(token => !token.Method(o)))
                    {
                        continue;
                    }
                }

                if (name.Child == null) _objs.Add(o);
                else
                {
                    var innerObjs = _findElementsByParent(name.Child, o);
                    if (innerObjs != null) _objs.AddRange(innerObjs);
                }
            }

            return _objs.ToArray();
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
                            Name = CSSNameToken.Parse(name),
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

        private static int getPriority(string styleName)
        {
            //https://www.w3.org/TR/selectors/#specificity
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

    public class CSSNameToken
    {
        public string TagName { get; set; }
        public string ID { get; set; }
        public string[] ClassList { get; set; }
        public CSSAttributeInfo[] Attrs { get; set; }
        public CSSTokenBinding PreviousBinding { get; set; } = CSSTokenBinding.First;
        public CSSNameToken Child { get; set; }
        public CSSNamePseudoToken[] PseudoTokens { get; set; }

        public override string ToString()
        {
            var style = new StringBuilder(30);

            style.Append(getCssTokenBindingText(PreviousBinding));

            if (!string.IsNullOrEmpty(TagName)) style.Append(TagName);
            if (!string.IsNullOrEmpty(ID)) style.Append("#" + ID);
            if (ClassList != null && ClassList.Length > 0)
            {
                foreach (var attr in ClassList)
                {
                    style.Append("." + attr);
                }
            }

            if (Attrs != null && Attrs.Length > 0)
            {
                foreach (var attr in Attrs)
                {
                    style.Append(attr);
                }
            }

            if (PseudoTokens != null && PseudoTokens.Length > 0)
            {
                foreach (var token in PseudoTokens)
                {
                    style.Append(token);
                }
            }

            if (Child != null) style.Append(Child);

            return style.ToString().Trim();
        }

        private static string getCssTokenBindingText(CSSTokenBinding token)
        {
            switch (token)
            {
                case CSSTokenBinding.All:
                    return " ";
                case CSSTokenBinding.After:
                    return " + ";
                case CSSTokenBinding.AllNextSiblings:
                    return " ~ ";
                case CSSTokenBinding.Childern:
                    return " > ";
                default:
                    return "";
            }
        }

        private static CSSTokenBinding parseBinding(string binding)
        {
            switch (binding.Trim())
            {
                case "": return CSSTokenBinding.All;
                case "+": return CSSTokenBinding.After;
                case "~": return CSSTokenBinding.AllNextSiblings;
                case ">": return CSSTokenBinding.Childern;
                default:
                    return CSSTokenBinding.All;
            }
        }

        private static string cleanStyleName(string styleName)
        {
            var sname = styleName.Trim();

            var r = new StringBuilder(sname.Length);
            var skipSpace = false;

            for (int i = 0, j; i < sname.Length; i++)
            {
                var ch = sname[i];

                if (char.IsWhiteSpace(ch))
                {
                    if (skipSpace) continue;
                    for (j = i; j < sname.Length; j++)
                    {
                        var ch2 = sname[j];
                        if (ch2 == '+' || ch2 == '>' || ch2 == '~')
                        {
                            skipSpace = true;
                            r.Append(ch2);
                            break;
                        }

                        if (!char.IsWhiteSpace(ch2))
                        {
                            skipSpace = true;
                            r.Append(" ");
                            break;
                        }
                    }
                    i = j;
                }
                else
                {
                    r.Append(ch);
                }
            }

            return r.ToString();
        }

        public static CSSNameToken Parse(string styleName)
        {
            return parse(cleanStyleName(styleName));
        }

        private static CSSNameToken parse(string styleName)
        {
            styleName = styleName.Trim();
            CSSNameToken name = new CSSNameToken();
            var pseudo = false; // informace o tom, zda cteny text odpovida pseudo tride, nebo classe
            var inBracket = 0; // informace pokud uz cte znaky
            var inAttr = false; // pokud je v atributu
            var temp = new StringBuilder();
            var startChar = '\0';
            CSSNamePseudoToken pseudoTemp = new CSSNamePseudoToken();
            //var prevNametoken = new CSSNameToken();
            //CSSNameToken rootToken = prevNametoken;

            string tagName = null;
            string id = null;
            var classList = new List<string>();
            var attrList = new List<CSSAttributeInfo>();
            var pseudoList = new List<CSSNamePseudoToken>();
            var i = -1;

            Func<bool> fncEndid = () =>
            {
                name.Attrs = attrList.ToArray();
                name.ClassList = classList.ToArray();
                name.PseudoTokens = pseudoList.ToArray();
                name.ID = id;
                name.TagName = tagName;

                attrList.Clear();
                classList.Clear();
                pseudoList.Clear();

                return false;
            };

            Func<bool> fncAppendByType = () =>
            {
                if (startChar == '.') classList.Add(temp.ToString());
                if (startChar == '#') id = temp.ToString();
                //if (startChar == ':') classList.Add(temp.ToString());
                if (startChar == '\0') tagName = temp.ToString();
                temp.Clear();

                return false;
            };

            foreach (var ch in styleName)
            {
                i++;

                if (ch == ']')
                {
                    attrList.Add(CSSAttributeInfo.Parse(temp.ToString()));
                    temp.Clear();
                    inAttr = false;
                    continue;
                }

                if (ch == ')')
                {
                    inBracket--;
                    pseudoTemp.Parameter = temp.ToString();
                    pseudoList.Add(pseudoTemp);
                    temp.Clear();
                    continue;
                }

                if (inAttr || inBracket > 0)
                {
                    temp.Append(ch);
                    continue;
                }

                if (ch == '+' || ch == '>' || ch == '~' || char.IsWhiteSpace(ch))
                {
                    fncAppendByType();
                    fncEndid();

                    name.Child = i + 1 < styleName.Length ? CSSNameToken.parse(styleName.Substring(i + 1)) : null;
                    name.Child.PreviousBinding = parseBinding(ch.ToString());

                    return name;
                    //continue;
                }


                if (ch == '.' || ch == '#' || ch == ':' || (ch == '['))
                {
                    fncAppendByType();

                    startChar = ch;

                    if (ch == '[') inAttr = true;
                    continue;
                }
                if (ch == '(')
                {
                    inBracket++;
                    if (startChar == ':')
                    {
                        pseudoTemp = new CSSNamePseudoToken { Name = temp.ToString() };
                        temp.Clear();
                        continue;
                    }
                }
                if (char.IsLetterOrDigit(ch) || ch == '-' || ch == '_')
                {
                    temp.Append(ch);
                }

            }

            fncAppendByType();
            fncEndid();

            return name;
        }
    }

    public class CSSNamePseudoToken
    {
        public bool IsFunction { get; set; } = false;
        public string Name { get; set; } = "";
        public string Parameter { get; set; } = "";
        //public BaseObject RootElement { get; set; }
        public Func<BaseObject, bool> Method { get; set; }

        public override string ToString()
        {
            var str = new StringBuilder(Name.Length + 5);
            str.Append($":{Name}");
            if (IsFunction) str.Append($"({Parameter})");

            return str.ToString();
        }
    }

    public class CSSAttributeInfo
    {
        public string Name { get; set; } = "";
        public string Value { get; set; } = null;
        public CSSAttributeValueInfo ValueRelationship { get; set; } = CSSAttributeValueInfo.Exist;

        public static bool CheckStatus(CSSAttributeInfo attr, string name, string value)
        {
            if (attr.Name != name) return false;
            if (attr.ValueRelationship == CSSAttributeValueInfo.Exist)
            {
                return true;
            }
            if (attr.ValueRelationship == CSSAttributeValueInfo.Equal)
            {
                if (value == attr.Value) return true;
            }
            if (attr.ValueRelationship == CSSAttributeValueInfo.StartWith)
            {
                if (value.StartsWith(attr.Value)) return true;
            }
            if (attr.ValueRelationship == CSSAttributeValueInfo.StartWithWord)
            {
                var r = new Regex($"^{attr.Value}[\\s|\\-|$]");
                if (r.IsMatch(value)) return true;
            }
            if (attr.ValueRelationship == CSSAttributeValueInfo.EndWith)
            {
                if (value.EndsWith(attr.Value)) return true;
            }
            if (attr.ValueRelationship == CSSAttributeValueInfo.ContainsWord)
            {
                var r = new Regex($"[\\s|\\-]{attr.Value}[\\s|\\-]");
                if (r.IsMatch(value)) return true;
            }
            if (attr.ValueRelationship == CSSAttributeValueInfo.Contains)
            {
                if (value.IndexOf(attr.Value, StringComparison.Ordinal) >= 0) return true;
            }


            return false;
        }

        public static CSSAttributeInfo Parse(string attr)
        {
            var attrr = new CSSAttributeInfo();
            var temp = new StringBuilder(attr.Length);
            foreach (var ch in attr)
            {
                if (ch == '[' || ch == ']') continue;
                if (ch == '|' || ch == '^' || ch == '~' || ch == '$' || ch == '*')
                {
                    attrr.Name = temp.ToString();
                    temp.Clear();
                }
                if (ch == '=')
                {
                    if (attrr.Name.Length == 0)
                    {
                        attrr.Name = temp.ToString();
                        temp.Clear();
                    }
                    switch (temp.ToString())
                    {
                        case "":
                            attrr.ValueRelationship = CSSAttributeValueInfo.Equal;
                            break;
                        case "|":
                            attrr.ValueRelationship = CSSAttributeValueInfo.StartWithWord;
                            break;
                        case "^":
                            attrr.ValueRelationship = CSSAttributeValueInfo.StartWith;
                            break;
                        case "~":
                            attrr.ValueRelationship = CSSAttributeValueInfo.ContainsWord;
                            break;
                        case "*":
                            attrr.ValueRelationship = CSSAttributeValueInfo.Contains;
                            break;
                        case "$":
                            attrr.ValueRelationship = CSSAttributeValueInfo.EndWith;
                            break;
                        case "$$":
                            attrr.ValueRelationship = CSSAttributeValueInfo.RegExp;
                            break;
                        default: throw new NotValidCSSName("Attribute not valid relationship sign");
                    }
                    temp.Clear();
                    continue;
                }

                temp.Append(ch);
            }
            if (attrr.Name.Length == 0) attrr.Name = temp.ToString();
            else attrr.Value = temp.ToString();

            return attrr;
        }

        public override string ToString()
        {
            string sign;
            switch (ValueRelationship)
            {
                case CSSAttributeValueInfo.Equal:
                    sign = "=";
                    break;
                case CSSAttributeValueInfo.StartWithWord:
                    sign = "|=";
                    break;
                case CSSAttributeValueInfo.StartWith:
                    sign = "^=";
                    break;
                case CSSAttributeValueInfo.ContainsWord:
                    sign = "~=";
                    break;
                case CSSAttributeValueInfo.Contains:
                    sign = "*=";
                    break;
                case CSSAttributeValueInfo.EndWith:
                    sign = "$=";
                    break;
                case CSSAttributeValueInfo.RegExp:
                    sign = "$$=";
                    break;
                case CSSAttributeValueInfo.Exist:
                    return $"[{Name}]";
                default:
                    sign = "=";
                    break;
            }
            return $"[{Name}{sign}{Value}]";
        }
    }

    public enum CSSAttributeValueInfo
    {
        Exist, // pokud proste hleda jen existujici atribut bez hodnoty
        Equal, // =
        StartWithWord, // |=
        StartWith, // ^=
        ContainsWord, // ~=
        Contains, // *=
        EndWith, // $=
        RegExp // $$= // neni ve specifikaci
    }

    public enum CSSTokenBinding
    {
        First,
        All,
        Childern, // >
        After, // +
        AllNextSiblings // ~
    }
}
