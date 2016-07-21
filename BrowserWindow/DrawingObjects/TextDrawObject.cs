using INetCore.Drawing.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BrowserWindow.DrawingObjects
{
    public class TextDrawObject : BaseDrawObject
    {
        [DefaultValue("")]
        public string Content { get; set; }
        public TextDrawObject(BaseObject baseObject, Canvas drawCanvas) : base(baseObject, drawCanvas)
        {
            Content = baseObject.InnerText;
        }

        public override void Draw(BaseObject _bo = null)
        {
            //base.Draw(_bo);
            if (_bo == null) _bo = baseObject;
            var p_text = _bo.GetRealStartPositionInnerText();


            TextBlock textBlock = new TextBlock();

            textBlock.Text = Content;

            textBlock.Foreground = Brushes.Black;

            Canvas.SetLeft(textBlock, p_text.X);

            Canvas.SetTop(textBlock, p_text.Y);

            drawCanvas.Children.Add(textBlock);
        }
    }
}
