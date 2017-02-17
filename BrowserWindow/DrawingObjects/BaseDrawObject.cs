using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using INetCore.Drawing.Objects;
using Color = System.Windows.Media.Color;

namespace BrowserWindow.DrawingObjects
{
    public class BaseDrawObject
    {
        protected BaseObject baseObject;
        protected Shape _basePolygon;
        protected Canvas drawCanvas;

        public BaseDrawObject(BaseObject baseObject, Canvas drawCanvas)
        {
            this.baseObject = baseObject;
            this.drawCanvas = drawCanvas;

            
        }

        public virtual void Draw(BaseObject _bo = null)
        {
            if (_bo == null) _bo = baseObject;

            var _lt = new DrawingPoint();
            var _lb = new DrawingPoint();
            var _rt = new DrawingPoint();
            var _rb = new DrawingPoint();

            RealObject r;
            var _p = _bo.getRealPositionWithRealObject(out r);

            _lt = new[] { 0,0 };
            _lb = new[] { 0, r.Height };
            _rt = new[] { r.Width, 0 };
            _rb = new[] { r.Width, r.Height };

            _basePolygon = new Polygon();
            
            ((Polygon)_basePolygon).Points = new PointCollection {_lt, _lb, _rb, _rt };
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

            if (!string.IsNullOrEmpty(_bo.InnerText))
            {
                new TextDrawObject(_bo, drawCanvas).Draw();
            }

            foreach (var o in _bo.Childrens)
            {
                Draw(o);
            }
            _bo.RefreshAfterDraw();
            
        }
    }
}
