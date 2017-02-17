using BrowserWindow.HelperClass;
using INetCore.Drawing.Objects;
using System.Windows.Controls;
using System.Windows.Media;

namespace BrowserWindow.DrawingObjects
{
    public class BlockDrawObject : BaseDrawObject
    {
        public BlockDrawObject(BaseObject baseObject, Canvas drawCanvas) : base(baseObject, drawCanvas)
        {
            
        }

        public override void Draw(BaseObject _bo = null)
        {
            if (_bo == null) _bo = baseObject;

            //var _lt = new DrawingPoint();
            //var _lb = new DrawingPoint();
            //var _rt = new DrawingPoint();
            //var _rb = new DrawingPoint();

            RealObject r;
            var _p = _bo.getRealPositionWithRealObject(out r);

            //_lt = new[] { 0, 0 };
            //_lb = new[] { 0, r.Height };
            //_rt = new[] { r.Width, 0 };
            //_rb = new[] { r.Width, r.Height };

            _basePolygon = new PartiallyRoundedRectangle
            {
                Width = r.Width,
                Height = r.Height
            };
            ((PartiallyRoundedRectangle)_basePolygon).RoundTopLeft = true;
            ((PartiallyRoundedRectangle)_basePolygon).RadiusX = 50;


            _basePolygon.Stretch = Stretch.Uniform;
            Canvas.SetLeft(_basePolygon, r.Left);
            Canvas.SetTop(_basePolygon, r.Top);
            //_basePolygon.Points.Add(_lt);
            //_basePolygon.Points.Add(_lb);
            //_basePolygon.Points.Add(_rt);
            //_basePolygon.Points.Add(_rb);

            if (_bo.Background.Color != System.Drawing.Color.Transparent)
            {
                var _bpolFill = new SolidColorBrush();
                _bpolFill.Color = Color.FromArgb(_bo.Background.Color.A, _bo.Background.Color.R, _bo.Background.Color.G, _bo.Background.Color.B);
                _basePolygon.Fill = _bpolFill;
            }

            drawCanvas.Children.Add(_basePolygon);

            //if (!string.IsNullOrEmpty(_bo.InnerText))
            //{
            //    new TextDrawObject(_bo, drawCanvas).Draw();
            //}

            foreach (var o in _bo.Childrens)
            {
                Draw(o);
            }
            _bo.RefreshAfterDraw();

        }
    }
}
