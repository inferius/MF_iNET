using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Forms;
using INetCore.Drawing.Objects;

namespace INetCore.BrowserTools
{
    public partial class DeveloperToolsAlpha : Form
    {
        private BaseObject _object;
        public DeveloperToolsAlpha(BaseObject _bObject)
        {
            InitializeComponent();
            _object = _bObject;
            ReloadObject();
            _loadEvents();
        }

        public void ReloadObject()
        {
            //htmlViewer
            //_object
            htmlViewer.Nodes.Add(_writeToTree(_object));
            htmlViewer.ExpandAll();
        }

        private TreeNode _writeToTree(BaseObject _obj)
        {
            var t = new TreeNode();
            t.Text = string.Format("{0}{1}", _obj.ObjectType.TagName, string.IsNullOrEmpty(_obj.InnerText) ? "" : "[" + _obj.InnerText + "]");
            t.Tag = _obj;

            foreach (var o in _obj.Childrens)
            {
                t.Nodes.Add(_writeToTree(o));
            }

            return t;
        }

        private void _loadEvents()
        {
            htmlViewer.AfterSelect += (sender, args) =>
            {
                var bo = htmlViewer.SelectedNode.Tag as BaseObject;
                styleBox.Items.Clear();
                styleBox.Items.Add(string.Format("{0}: {1}", "background-color", bo.Background.Color.ToString()));
                styleBox.Items.Add(string.Format("{0}: {1}", "background-position", bo.Background.Position.ToString()));
                styleBox.Items.Add(string.Format("{0}: {1}", "background-repeat", bo.Background.RepeatBackground.ToString()));
                styleBox.Items.Add(string.Format("{0}: {1}", "background-size", bo.Background.SizeBackground.ToString()));

                styleBox.Items.Add($"border-top: {bo.BorderTop.Width} {bo.BorderTop.Color.ToString()} {bo.BorderTop.Style.ToString()}");
                styleBox.Items.Add($"border-left: {bo.BorderLeft.Width} {bo.BorderLeft.Color.ToString()} {bo.BorderLeft.Style.ToString()}");
                styleBox.Items.Add($"border-bottom: {bo.BorderBottom.Width} {bo.BorderBottom.Color.ToString()} {bo.BorderBottom.Style.ToString()}");
                styleBox.Items.Add($"border-right: {bo.BorderRight.Width} {bo.BorderRight.Color.ToString()} {bo.BorderRight.Style.ToString()}");

                styleBox.Items.Add($"width: {bo.Width.ToString()}");
                styleBox.Items.Add($"height: {bo.Height.ToString()}");

                styleBox.Items.Add(string.Format("{0}: {1}", "position", bo.PositionType.ToString()));
                styleBox.Items.Add($"top: {bo.ObjectPosition.Top}");
                styleBox.Items.Add($"left: {bo.ObjectPosition.Left}");
                styleBox.Items.Add($"bottom: {bo.ObjectPosition.Bottom}");
                styleBox.Items.Add($"right: {bo.ObjectPosition.Right}");

                styleBox.Items.Add(string.Format("{0}: {1}", "display", bo.Display.Type.ToString()));
            };
        }
    }
}
