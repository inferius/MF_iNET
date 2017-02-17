using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace INetCore.Core.Language.HTML
{
    public partial class CoreClass
    {
        private List<HtmlTagDefinition> listOfTags = new List<HtmlTagDefinition>();

        public void LoadDefaultTags()
        {
            try
            {
                //List<HtmlTagDefinition> tags = new List<HtmlTagDefinition>();

                string path = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Core\\Language\\Definitions\\HtmlDefinition.xml");
                string xpath = "//Language/*";

                XmlDocument doc = new XmlDocument();
                doc.Load(path);
                XmlNodeList tags_list = doc.SelectNodes(xpath);

                foreach (XmlNode tag in tags_list)
                {
                    HtmlTagDefinition t = new HtmlTagDefinition();
                    foreach (XmlNode tag_info in tag.ChildNodes)
                    {
                        if (tag_info.LocalName == "Name") t.TagName = tag_info.InnerText;
                        if (tag_info.LocalName == "Pair") t.IsPair = (SettingPairTag)int.Parse(tag_info.InnerText);

                        if (tag_info.LocalName == "Support")
                        {
                            foreach (XmlNode support_info in tag_info.ChildNodes)
                            {
                                t.SupportedHtmlVersion.Add(support_info.InnerText);
                            }
                        }

                        if (tag_info.LocalName == "Attributes")
                        {
                            foreach (XmlNode attribute in tag_info.ChildNodes)
                            {
                                HtmlAttributeDefinition attr = new HtmlAttributeDefinition("");
                                foreach (XmlNode attr_info in attribute.ChildNodes)
                                {
                                    if (attr_info.LocalName == "Name") attr.AttributeName = attr_info.InnerText;
                                    if (attr_info.LocalName == "DefaultValue") attr.AttributeDefaultValue = attr_info.InnerText;
                                    if (attr_info.LocalName == "ValidateValueRegex") attr.AttributeValueValidateRegExp = attr_info.InnerText;
                                    if (attr_info.LocalName == "ValidateRequired") attr.AttributeValidateRequired = Boolean.Parse(attr_info.InnerText);
                                    if (attr_info.LocalName == "AttributeRequired") attr.AttributeRequired = Boolean.Parse(attr_info.InnerText);
                                    if (attr_info.LocalName == "NameIsRegex") attr.IsAttributeNameRegexp = Boolean.Parse(attr_info.InnerText);

                                    if (attr_info.LocalName == "Support")
                                    {
                                        foreach (XmlNode support_info in attr_info.ChildNodes)
                                        {
                                            attr.SupportedHtmlVersion.Add(support_info.InnerText);
                                        }
                                    }
                                }
                                t.ValidHtmlAttributes.Add(attr);
                            }
                        }
                    }
                    listOfTags.Add(t);
                }

                _htmlTags = listOfTags;

            }
            catch (System.IO.DirectoryNotFoundException e)
            {
                string path = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "Core\\Language\\Definitions\\HtmlDefinition.xml");
                System.Windows.Forms.MessageBox.Show("Nepodařilo se najít složku. (" + path + ")", "Chyba", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
            catch (System.IO.FileNotFoundException e)
            {
                System.Windows.Forms.MessageBox.Show("Nepodařilo se najít soubor. (" + e.FileName + ")", "Chyba", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

        }
    }

    public class HtmlTagDefinition
    {
        private List<string> _supportHtmlVersion = new List<string>();
        private List<HtmlAttributeDefinition> _validHtmlAttributes = new List<HtmlAttributeDefinition>();
        private string _tagName;
        private SettingPairTag _isPair;


        #region Property Setting
        /// <summary>
        /// Nastavení podporovaných verzí HTML
        /// </summary>
        public List<string> SupportedHtmlVersion
        {
            get { return _supportHtmlVersion; }
            set { this._supportHtmlVersion = value; }
        }

        /// <summary>
        /// Seznam podporovaných atributu
        /// </summary>
        public List<HtmlAttributeDefinition> ValidHtmlAttributes
        {
            get { return _validHtmlAttributes; }
            set { this._validHtmlAttributes = value; }
        }

        /// <summary>
        /// Jméno tagu
        /// </summary>
        public string TagName
        {
            get { return _tagName; }
            set { _tagName = value; }
        }

        /// <summary>
        /// Nastavení pokud je tag párovy
        /// </summary>
        public SettingPairTag IsPair
        {
            get { return _isPair; }
            set { _isPair = value; }
        }
        #endregion

        /// <summary>
        /// Vráti typ atributu podle jména
        /// </summary>
        /// <param name="name">Jméno atributu</param>
        /// <returns>Vrátí atribut podle zadaného jména a nebo null, pokud není validní atribut</returns>
        public HtmlAttributeDefinition GetAttributeDefinitionByAttributeName(string name)
        {
            name = name.ToLower();
            foreach (HtmlAttributeDefinition a in _validHtmlAttributes)
            {
                if (a.AttributeName.ToLower() == name)
                {
                    return a;
                }
            }

            return null;
        }

        #region Override method
        public override string ToString()
        {
            return "Tag: " + TagName;
        }
        #endregion


    }

    public enum SettingPairTag
    {
        Pair,
        NotPair,
        Both
    }

    /// <summary>
    /// Struktura pro HTML Atributy
    /// </summary>
    public class HtmlAttributeDefinition
    {
        /// <summary>
        /// Jméno atributu
        /// </summary>
        public string AttributeName { get; set; } = "";

        /// <summary>
        /// Výchozí hodnota atributu, je-li atribut povinný a není použit je nastaven s výchozi hodnotou
        /// </summary>
        public string AttributeDefaultValue { get; set; } = "";

        /// <summary>
        /// Nastavení podporovaných verzí HTML
        /// </summary>
        public List<string> SupportedHtmlVersion { get; set; } = new List<string>();

        /// <summary>
        /// Regularni výraz pro validaci hodnoty atributu
        /// </summary>
        public string AttributeValueValidateRegExp { get; set; } = "";

        /// <summary>
        /// Nastaveni povinosti validace, pokud je true, pak je validace povinna
        /// </summary>
        public bool AttributeValidateRequired { get; set; } = false;

        /// <summary>
        /// Nastavení zda je atribut pro tag povinný. True pokud je povinný
        /// </summary>
        public bool AttributeRequired { get; set; } = false;

        /// <summary>
        /// Nastavení zda je název atributu utvořen regulárním výrazem. True pokud je povinný
        /// </summary>
        public bool IsAttributeNameRegexp { get; set; } = false;

        public bool IsSupportedVersion(CoreClass.HTMLVersion htmlVersion, HtmlVersionType htmlType)
        {
            return true;
        }

        public HtmlAttributeDefinition(string attributeName, string attributeDefaultValue = "", string attributeValueRegexp = "", bool validateRequired = false, bool attributeRequired = false, bool isAttributeNameRegexp = false)
        {
            AttributeName = attributeName;
            AttributeDefaultValue = attributeDefaultValue;
            AttributeValueValidateRegExp = attributeValueRegexp;
            AttributeValidateRequired = validateRequired;
            AttributeRequired = attributeRequired;
            IsAttributeNameRegexp = isAttributeNameRegexp;
        }

        #region Override method
        public override string ToString()
        {
            return AttributeName + "='" + AttributeDefaultValue +"'";
        }
        #endregion
    }
}
