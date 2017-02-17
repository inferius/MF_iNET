using INetCore.Drawing.Objects;
using System.Windows.Controls;
using System.Windows.Media;

namespace BrowserWindow.DrawingObjects
{
    public class TextDrawObject : BaseDrawObject
    {
        public string Content { get; set; } = string.Empty;
        public TextDrawObject(BaseObject baseObject, Canvas drawCanvas) : base(baseObject, drawCanvas)
        {
            Content = baseObject.InnerText;
        }

        public override void Draw(BaseObject _bo = null)
        {
            //base.Draw(_bo);
            if (_bo == null) _bo = baseObject;
            var p_text = _bo.GetRealStartPositionInnerText();


            TextBlock textBlock = new TextBlock
            {
                Text = Content,
                Foreground = Brushes.Black
            };

            Canvas.SetLeft(textBlock, p_text.X);
            Canvas.SetTop(textBlock, p_text.Y);

            drawCanvas.Children.Add(textBlock);
        }
    }
}
