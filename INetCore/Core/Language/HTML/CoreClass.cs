using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using INetCore.Core.Exception;
using INetCore.Core.Language.CSS.Styles;
using INetCore.Drawing;
using INetCore.Drawing.Objects;

namespace INetCore.Core.Language.HTML
{
    public partial class CoreClass
    {
        private string _htmlVersion = "5";
        private HtmlVersionType _htmlVersionType = HtmlVersionType.Transitional;
        private string _xhtmlVersion = "1.1";
        private List<HtmlTagDefinition> _htmlTags = new List<HtmlTagDefinition>();

        #region Property Setting
        public HtmlVersionType TypeOfVersionHtml
        {
            set { _htmlVersionType = value; }
            get { return _htmlVersionType; }
        }

        public string HtmlVersion
        {
            set
            {
                switch (value)
                {
                    case "3.2":
                    case "4.0":
                    case "4.01":
                    case "5":
                        _htmlVersion = value; break;
                    default: throw new NotValidPropertyException("Invalid or unsupport HTML Version");
                }
            }
            get
            {
                return _htmlVersion;
            }
        }

        public string XHtmlVersion
        {
            set
            {
                switch (value)
                {
                    case "1.0":
                    case "1.1":
                    case "2.0":
                        _xhtmlVersion = value; break;
                    default: throw new NotValidPropertyException("Invalid or unsupport XHTML Version");
                }
            }
            get
            {
                return _xhtmlVersion;
            }
        }
        #endregion

        public CoreClass()
        {
            LoadDefaultTags();
        }

        public HtmlTagDefinition GetTagDefinitionByTagName(string name)
        {
            name = name.ToLower();

            return _htmlTags.FirstOrDefault(h => h.TagName.ToLower() == name);
        }

        public bool ValidationHtml(string input, string htmlVersion)
        {
            return false;
        }

        /// <summary>
        /// Vyčistí nadbytečné mezery
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string CleanString(string input)
        {
            StringBuilder ret = new StringBuilder();

            bool was_char = false;
            bool was_wch = false;
            bool str = false;
            char str_char = '\0';

            for (int i = 0; i < input.Length; i++)
            {

                if (str)
                {
                    if (input[i] == '\'' || input[i] == '"')
                    {
                        if (input[i] == str_char)
                        {
                            if (input[i - 1] == '\\')
                            {
                                ret.Append(input[i]);
                                continue;
                            }
                            else
                            {
                                ret.Append(input[i]);
                                str = false;
                            }
                        }
                        continue;
                    }

                    ret.Append(input[i]);
                    continue;
                }
                if (char.IsLetterOrDigit(input[i]))
                {
                    if (was_wch && was_char)
                    {
                        ret.Append(" ");
                        was_wch = false;
                    }
                    was_char = true;
                    ret.Append(input[i]);
                }
                else if (char.IsWhiteSpace(input[i]))
                {
                    was_wch = true;
                    continue;
                }
                else
                {
                    if (input[i] == '\'' || input[i] == '"')
                    {
                        str_char = input[i];
                        str = true;
                        was_char = true;
                    }
                    else
                    {
                        was_char = false;
                        was_wch = false;
                    }
                    ret.Append(input[i]);
                }
            }

            return ret.ToString();
        }

        public BaseObject ToBaseObjects(BrowserWindow parent, HtmlAgilityPack.HtmlDocument _doc)
        {
            return ToBaseObjects(parent, _doc.DocumentNode);
        }

        public BaseObject ToBaseObjects(BrowserWindow parent, HtmlAgilityPack.HtmlNode tag)
        {
            var bo = new BaseObject(parent);
            tagProcessing(tag, bo);
            ToBaseObjects(bo, tag.ChildNodes);

            return bo;
        }
        public void ToBaseObjects(BaseObject parent, IEnumerable<HtmlAgilityPack.HtmlNode> tags)
        {
            foreach (var tag in tags)
            {
                var bo = new BaseObject(parent);
                tagProcessing(tag, bo);
                //parent.Childrens.Add(bo);

                ToBaseObjects(bo, tag.ChildNodes);
            }
        }

        private static void tagProcessing(HtmlAgilityPack.HtmlNode tag, BaseObject bo)
        {
            foreach (var attribute in tag.Attributes)
            {
                if (attribute.Name == "style")
                {
                    //TODO: Zrušit aplikování stylu přímo tady, provést jej až v CSSLinq
                    //bo.ApplyStyles(attribute.AttributeValue);
                }
                else if (attribute.Name == "class")
                {
                    tag.ClassList.AddRange(attribute.Value.Trim().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).Select(item => item.Trim()).Where(item => item.Length > 0));
                }
            }
            bo.ObjectType = tag;
            tag.BaseObject = bo;
        }

        // TODO: Udelat vlastni parsovani dokumentu



    }

    public enum HtmlVersionType
    {
        None,
        Strict,
        Transitional,
        Frameset
    }

    public class HtmlTag
    {
        internal List<HtmlTag> _innerTags = new List<HtmlTag>();

        #region Property
        /// <summary>
        /// Nastaveni typu tagu
        /// </summary>
        public HtmlTagDefinition TagType { get; set; }

        /// <summary>
        /// Seznam atributu
        /// </summary>
        public List<HtmlAttribute> HtmlAttributes { get; set; } = new List<HtmlAttribute>();

        /// <summary>
        /// Seznam stylu v attributu class
        /// </summary>
        public List<string> ClassList { get; private set; } = new List<string>();

        /// <summary>
        /// Identifikator
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Jméno tagu
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// Obsah tagu
        /// </summary>
        public string InnerHtml { get; set; }
        /// <summary>
        /// Seznam defaultních stylů pro typ elementu daný definici
        /// </summary>
        public BaseStyle[] DefaultStyles { get; set; }

        /// <summary>
        /// Obsah tagu
        /// </summary>
        public HtmlTag[] InnerTags
        {
            get { return _innerTags.ToArray(); }
            set { _innerTags = new List<HtmlTag>(value); }
        }
        #endregion

        #region Override Methods
        public override string ToString()
        {
            if (TagType != null && TagType.IsPair == SettingPairTag.NotPair)
            {
                return $"<{TagName} />";
            }
            else
            {
                string txt = InnerHtml;
                if (txt.Length > 10) txt = txt.Remove(8) + "...";
                return $"<{TagName}>{txt}</{TagName}>";
            }
        }
        #endregion

        public void SetAttribute(string attrName, string attrValue)
        {
            SetAttribute(new HtmlAttribute(attrName, attrValue));
        }

        public void SetAttribute(HtmlAttribute attribute)
        {
            var attr = HtmlAttributes.FirstOrDefault(item => item.AttributeName == attribute.AttributeName);
            if (attr != null)
            {
                attr.AttributeValue = attribute.AttributeValue;
            }
            else HtmlAttributes.Add(attribute);
        }

        public HtmlAttribute GetAttribute(string attrName)
        {
            return HtmlAttributes.FirstOrDefault(item => item.AttributeName == attrName);
        }

        public bool ContainsAttribute(string attrName)
        {
            return HtmlAttributes.Any(item => item.AttributeName == attrName);
        }

        public void RemoveAttribute(string attrName)
        {
            var attr = GetAttribute(attrName);
            if (attr != null) HtmlAttributes.Remove(attr);
        }

        /// <summary>
        /// Rozprasuje string na HTML Tagy
        /// </summary>
        /// <param name="input">Html Kod</param>
        /// <returns>Seznam tříd HtmlTag</returns>
        public static List<HtmlTag> Parse(string input)
        {
            return null;
        }
    }

    public class HtmlAttribute
    {
        private string _attributeName;
        private string _attributeValue;
        private HtmlAttributeDefinition _attrType;

        #region Property
        /// <summary>
        /// Jméno atributu
        /// </summary>
        public string AttributeName
        {
            get { return _attributeName; }
            set { _attributeName = value; }
        }

        /// <summary>
        /// Výchozí hodnota atributu, je-li atribut povinný a není použit je nastaven s výchozi hodnotou
        /// </summary>
        public string AttributeValue
        {
            get { return _attributeValue; }
            set { _attributeValue = value; }
        }

        public HtmlAttributeDefinition AttributeType
        {
            get { return _attrType; }
            set { _attrType = value; }
        }
        #endregion

        #region Override Methods
        public override string ToString()
        {
            return string.Format("{0}='{1}'", _attributeName, _attributeValue);
        }
        #endregion


        public HtmlAttribute(string attributeName = "", string attributeValue = "", HtmlAttributeDefinition attributeType = null)
        {
            _attributeName = attributeName.ToLower();
            _attributeValue = attributeValue;
            _attrType = attributeType;
        }
    }
}
