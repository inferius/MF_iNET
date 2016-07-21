using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FastColoredTextBoxNS;

namespace BrowserWindow
{
    /// <summary>
    /// Interaction logic for SourceExplorer.xaml
    /// </summary>
    public partial class SourceExplorer : Page
    {
        public SourceExplorer(string data = "")
        {
            InitializeComponent();

            initSource();

            sourceViewer.Text = data.Replace("<", "\n<");
            //sourceViewer.AppendText(data.Replace(">", ">\n"));
        }

        private void initSource()
        {
            //sourceViewer.Language = FastColoredTextBoxNS.Language.HTML;
            //sourceViewer.SyntaxHighlighter = new SyntaxHighlighter();
            sourceViewer.TextChanged += SourceViewer_TextChanged;
            sourceViewer.TextChangedDelayed += SourceViewer_TextChangedDelayed;
            sourceViewer.AutoIndent = true;
            sourceViewer.TabLength = 3;
            sourceViewer.AutoIndentNeeded += SourceViewer_AutoIndentNeeded;
        }

        private void SourceViewer_TextChangedDelayed(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            var tb = (FastColoredTextBox)sender;

            //highlight html
            tb.SyntaxHighlighter.InitStyleSchema(FastColoredTextBoxNS.Language.HTML);
            tb.SyntaxHighlighter.HTMLSyntaxHighlight(tb.Range);
            tb.Range.ClearFoldingMarkers();
            e.ChangedRange.SetFoldingMarkers(@"<div", "</div>");
            //find PHP fragments
            foreach (var r in tb.GetRanges(@"<script.*?</script>", RegexOptions.Singleline))
            {
                //remove HTML highlighting from this fragment
                r.ClearStyle(StyleIndex.All);
                tb.SyntaxHighlighter.InitStyleSchema(FastColoredTextBoxNS.Language.JS);
                tb.SyntaxHighlighter.JScriptSyntaxHighlight(r);
            }
        }

        private void SourceViewer_AutoIndentNeeded(object sender, FastColoredTextBoxNS.AutoIndentEventArgs e)
        {
            if (Regex.IsMatch(e.LineText.Trim(), "<([A-Z][A-Z0-9]*)\b[^>]*>(.*?)</$1>",
                RegexOptions.CultureInvariant | RegexOptions.IgnoreCase))
            {
                return;
            }

            if (Regex.IsMatch(e.LineText.Trim(), "<([A-Z][A-Z0-9]*)\b[^>]*>",
                RegexOptions.CultureInvariant | RegexOptions.IgnoreCase))
            {
                e.ShiftNextLines = e.TabLength;
                return;
            }

            if (Regex.IsMatch(e.LineText.Trim(), "</([A-Z][A-Z0-9]*)>",
                RegexOptions.CultureInvariant | RegexOptions.IgnoreCase))
            {
                e.ShiftNextLines = -e.TabLength;
                e.Shift = -e.TabLength;
                return;
            }
        }

        private void SourceViewer_TextChanged(object sender, FastColoredTextBoxNS.TextChangedEventArgs e)
        {
            //sourceViewer.SyntaxHighlighter.HTMLSyntaxHighlight(e.ChangedRange);
            //clear folding markers of changed range
            //e.ChangedRange.ClearFoldingMarkers();
            //set folding markers
            //e.ChangedRange.SetFoldingMarkers(@"<div", "</div>");
            //e.ChangedRange.SetFoldingMarkers(@"#region\b", @"#endregion\b");

            //<([A-Z][A-Z0-9]*)\b[^>]*>(.*?)</\1>
            

            //var range = e.ChangedRange;

            ////clear style of changed range
            //range.ClearStyle(GreenStyle);
            ////comment highlighting
            //range.SetStyle(GreenStyle, @"//.*$", RegexOptions.Multiline);
            //range.SetStyle(GreenStyle, @"(/\*.*?\*/)|(/\*.*)", RegexOptions.Singleline);
            //range.SetStyle(GreenStyle, @"(/\*.*?\*/)|(.*\*/)", RegexOptions.Singleline |
            //            RegexOptions.RightToLeft);
        }
    }
}
