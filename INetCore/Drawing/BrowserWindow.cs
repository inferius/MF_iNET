using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace INetCore.Drawing
{
    public partial class BrowserWindow : UserControl
    {
        private List<string> _warning = new List<string>();
        private List<string> _error = new List<string>();

        public List<string> Warning
        {
            get { return this._warning; }
        }

        public BrowserWindow()
        {
            InitializeComponent();
        }

        public void AddWarning(string text)
        {

        }
    }

    public class AlertData
    {
        string type;
    }
}
