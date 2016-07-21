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

        public BaseObject ToBaseObjects(BrowserWindow parent, string html, List<BaseObject> fullList = null)
        {
            var p = ParseString(html);
            if (p.Length > 1 || p.Length == 0) throw new FormatException("Web page have'nt root element");
            return ToBaseObjects(parent, p[0], fullList);
        }
        public BaseObject ToBaseObjects(BrowserWindow parent, HtmlTag tag, List<BaseObject> fullList = null)
        {
            var bo = new BaseObject(parent);
            tagProcessing(tag, bo);
            ToBaseObjects(bo, tag.InnerTags, fullList);

            return bo;
        }
        public void ToBaseObjects(BaseObject parent, HtmlTag[] tags, List<BaseObject> fullList = null)
        {
            foreach (var tag in tags)
            {
                var bo = new BaseObject(parent);
                tagProcessing(tag, bo);
                //parent.Childrens.Add(bo);
                fullList?.Add(bo);

                ToBaseObjects(bo, tag.InnerTags);
            }
        }

        private static void tagProcessing(HtmlTag tag, BaseObject bo)
        {
            foreach (var attribute in tag.HtmlAttributes)
            {
                if (attribute.AttributeName == "style")
                {
                    bo.ApplyStyles(attribute.AttributeValue);
                }
                else if (attribute.AttributeName == "class")
                {
                    tag.ClassList.AddRange(attribute.AttributeValue.Trim().Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).Select(item => item.Trim()).Where(item => item.Length > 0));
                }
            }
            bo.ObjectType = tag;
        }

        public HtmlTag[] ParseString(string input)
        {
            input = CleanString(input);

            List<HtmlTag> ret = new List<HtmlTag>();
            List<string> warning_list = new List<string>();

            HtmlTag buf_tag = new HtmlTag();
            HtmlAttribute buf_attr = new HtmlAttribute();

            StringBuilder buf = new StringBuilder();
            var textBuf = new StringBuilder();
            var commentBuf = new StringBuilder();


            int begin_start_tag = -1;
            int end_start_tag = -1;
            int begin_end_tag = -1;
            int end_end_tag = -1;
            int equal_tag_count = 0;

            int buf_begin_tag = -1;
            int buf_end_tag = -1;

            bool begin_tag = false;
            bool end_tag = false;
            bool char_request = false;
            bool tag_name_waiting = false;
            bool attr_value_waiting = false;
            bool isBuf_end_tag = false;
            var inComment = false;

            char begin_str = '\0';

            int count_line = 0;

            for (int i = 0; i < input.Length; i++)
            {
                if (inComment)
                {
                    if (input[i] == '-' && i + 1 < input.Length && i + 2 < input.Length && input[i + 1] == '-' && input[i + 2] == '>')
                    {
                        inComment = false;
                        i += 2;

                        var commentTag = new HtmlTag
                        {
                            TagName = "#comment",
                            InnerHtml = commentBuf.ToString()
                        };

                        buf_tag._innerTags.Add(commentTag);
                        commentBuf.Clear();
                    }
                    else commentBuf.Append(input[i]);
                    continue;
                }
                if (char_request)
                {
                    if (char.IsLetter(input[i]))
                    {
                        char_request = false;
                    }
                    else
                    {
                        if (_htmlVersionType == HtmlVersionType.Strict)
                        {
                            throw new HtmlSyntaxException("Byly očekávány pismena", count_line, "");
                        }

                        warning_list.Add($"Byly očekávány pismena|{count_line}");
                        continue;
                    }
                }

                // zapsaní prostého textu nalezeného v attributu
                if (!tag_name_waiting && !begin_tag)
                {
                    if (input[i] == '<')
                    {
                        if (textBuf.Length > 0)
                        {
                            var textTag = new HtmlTag
                            {
                                TagName = "#text",
                                InnerHtml = textBuf.ToString()
                            };

                            //buf_tag._innerTags.Add(textTag);
                            ret.Add(textTag);
                            textBuf.Clear();
                        }
                    }
                    else
                    {
                        textBuf.Append(input[i]);
                        continue;
                    }
                }

                if (char.IsLetterOrDigit(input[i]))
                {
                    if (tag_name_waiting)
                    {
                        buf.Append(input[i]);
                    }
                    else if (begin_tag && !tag_name_waiting && end_start_tag == -1 && !attr_value_waiting) // nacitani názvu atributu.
                    {
                        buf.Append(input[i]);
                    }
                    else if (begin_tag && end_start_tag == -1 && attr_value_waiting) // nacitani názvu atributu
                    {
                        buf.Append(input[i]);
                    }
                    else if (buf_begin_tag >= 0)
                    {
                        buf.Append(input[i]);
                    }
                }
                else if (char.IsWhiteSpace(input[i]))
                {
                    // pokud narazi na bily znak a je ocekavan nazev tagu. Tak je to nazev tagu
                    if (tag_name_waiting && end_start_tag == -1)
                    {
                        buf_tag.TagName = buf.ToString().ToLower();
                        buf.Clear();
                        tag_name_waiting = false;
                        buf_tag.TagType = GetTagDefinitionByTagName(buf_tag.TagName);
                        continue;
                    }
                    else if (buf_begin_tag >= 0 && end_start_tag >= 0)
                    {
                        if (buf.ToString() == buf_tag.TagName)
                        {
                            equal_tag_count++;
                            buf.Clear();
                        }
                    }
                    else if (attr_value_waiting && begin_str == '\0')
                    {
                        attr_value_waiting = false;
                        buf_attr.AttributeValue = buf.ToString();
                        buf.Clear();
                        buf_tag.HtmlAttributes.Add(buf_attr);
                        buf_attr = new HtmlAttribute();
                        begin_str = '\0';
                    }
                    else if (attr_value_waiting && begin_str != '\0')
                    {
                        buf.Append(input[i]);
                        continue;
                    }
                }

                else if (input[i] == '"' || input[i] == '\'')
                {
                    if (attr_value_waiting)
                    {
                        if (begin_str == '\0')
                        {
                            begin_str = input[i];
                            continue;
                        }
                        else if (begin_str == input[i])
                        {
                            if (input[i - 1] == '\\')
                            {
                                buf.Append(input[i]);
                            }
                            else
                            {
                                buf_attr.AttributeValue = buf.ToString();
                                buf.Clear();
                                if (buf_tag.TagType != null)
                                {
                                    buf_attr.AttributeType = buf_tag.TagType.GetAttributeDefinitionByAttributeName(buf_attr.AttributeName);
                                }
                                buf_tag.HtmlAttributes.Add(buf_attr);
                                buf_attr = new HtmlAttribute();
                                begin_str = '\0';
                                attr_value_waiting = false;
                                continue;
                            }
                        }
                        else
                        {
                            buf.Append(input[i]);
                        }
                    }
                }

                else if (input[i] == '=')
                {
                    if (end_start_tag == -1)
                    {
                        if (attr_value_waiting)
                        {
                            buf.Append(input[i]);
                            continue;
                        }
                        else
                        {
                            attr_value_waiting = true;
                        }
                    }
                    //if (begin_tag && !end_tag && !attr_value_waiting)
                    //{
                    //    if (_htmlVersionType == HtmlVersionType.Strict)
                    //    {
                    //        throw new HtmlSyntaxException("Neplatný znak '='", count_line, "");
                    //    }
                    //}
                    if (begin_tag && !end_tag && attr_value_waiting) // nacitani názvu atributu
                    {
                        buf_attr.AttributeName = buf.ToString();
                        buf.Clear();
                    }
                }

                else if (input[i] == '<')
                {
                    // kontrola na komentar
                    if (i + 1 < input.Length && i + 2 < input.Length && i + 3 < input.Length)
                    {
                        if (input[i + 1] == '!' && input[i + 2] == '-' && input[i + 3] == '-')
                        {
                            inComment = true;
                            i += 3;
                            continue;
                        }
                    }
                    if (begin_tag && end_start_tag == -1)
                    {
                        if (_htmlVersionType == HtmlVersionType.Strict)
                        {
                            throw new HtmlSyntaxException("Chyba v syntaxi HTML. Nelze použít znak '<'", count_line, "");
                        }
                        warning_list.Add($"Chyba v syntaxi HTML. Nelze použít znak '<'|{count_line}");
                    }
                    else if (begin_tag && end_start_tag >= 0)
                    {
                        if (input[i + 1] == '/')
                        {
                            buf_begin_tag = i;
                            isBuf_end_tag = true;
                            i++;
                        }
                        else
                        {
                            buf_begin_tag = i;
                        }
                    }
                    else if (!begin_tag)
                    {
                        begin_tag = true;
                        begin_start_tag = i;
                        char_request = true;
                        tag_name_waiting = true;
                    }

                }
                else if (input[i] == '>')
                {
                    // pokud narazi na bily znak a je ocekavan nazev tagu. Tak je to nazev tagu
                    if (tag_name_waiting && end_start_tag == -1)
                    {
                        end_start_tag = i;
                        buf_tag.TagName = buf.ToString().ToLower();
                        buf.Clear();
                        tag_name_waiting = false;
                        buf_tag.TagType = GetTagDefinitionByTagName(buf_tag.TagName);
                        if (buf_tag.TagType != null && buf_tag.TagType.IsPair == SettingPairTag.NotPair)
                        {
                            ret.Add(buf_tag);
                            buf_tag = new HtmlTag();
                            ret.AddRange(ParseString(input.Remove(begin_start_tag, end_start_tag - begin_start_tag + 1)));
                        }
                        continue;
                    }
                    else if (end_start_tag == -1)
                    {
                        end_start_tag = i;
                        // pokud je tag ukoncen takhle je neparovy
                        if (input[i - 1] == '/' || (buf_tag.TagType != null && buf_tag.TagType.IsPair == SettingPairTag.NotPair))
                        {
                            ret.Add(buf_tag);
                            buf_tag = new HtmlTag();
                            ret.AddRange(ParseString(input.Remove(begin_start_tag, end_start_tag - begin_start_tag + 1)));
                        }
                    }
                    else if (buf_begin_tag >= 0 && end_start_tag >= 0)
                    {
                        if (buf.ToString() == buf_tag.TagName)
                        {
                            if (equal_tag_count > 0)
                            {
                                equal_tag_count--;
                            }
                            else
                            {
                                if (!isBuf_end_tag)
                                {
                                    equal_tag_count++;
                                    buf.Clear();
                                }
                                begin_end_tag = buf_begin_tag;
                                end_end_tag = i;
                                //inner = input.Remove(begin_start_tag, end_start_tag - begin_start_tag + 1);
                                //int offset = input.Length - inner.Length;
                                //inner = inner.Remove(begin_end_tag - offset, end_end_tag - begin_end_tag + 1);
                                var inner = input.Remove(begin_end_tag);
                                inner = inner.Remove(begin_start_tag, end_start_tag - begin_start_tag + 1);
                                buf_tag.InnerHtml = inner;
                                buf_tag.InnerTags = ParseString(inner);
                                ret.Add(buf_tag);
                                buf_tag = new HtmlTag();
                                input = input.Remove(begin_start_tag, end_end_tag - begin_start_tag + 1);
                                if (input.Length > 0)
                                {
                                    ret.AddRange(ParseString(input));
                                }
                            }
                        }
                        buf_begin_tag = -1;
                        buf.Clear();
                    }
                }
                else
                {
                    if (begin_tag && end_start_tag == -1 && attr_value_waiting) // nacitani názvu atributu
                    {
                        buf.Append(input[i]);
                    }
                }
            }


            return ret.ToArray();
        }


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
