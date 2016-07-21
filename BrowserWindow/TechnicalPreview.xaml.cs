using System.Windows;
using INetCore.Drawing.Objects;

namespace BrowserWindow
{
    /// <summary>
    /// Interaction logic for TechnicalPreview.xaml
    /// </summary>
    public partial class TechnicalPreview : Window
    {
        public TechnicalPreview(BaseObject _baseObject)
        {
            InitializeComponent();

            var p = new BrowserPage(_baseObject);
            pageFrame.DataContext = p;

            sourceFrame.DataContext = new SourceExplorer(_baseObject.ToHTML());
        }
    }
}
