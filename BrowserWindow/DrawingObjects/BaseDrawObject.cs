using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using INetCore.Drawing.Objects;
using Brushes = System.Windows.Media.Brushes;
using Color = System.Windows.Media.Color;

namespace BrowserWindow.DrawingObjects
{
    public class BaseDrawObject
    {
        protected BaseObject baseObject;
        protected Shape _basePolygon;
        protected Canvas drawCanvas;

        private bool mouseDownPressed = false;
        private Shape _lastMouseDownPressedObject = null;

        public BaseDrawObject(BaseObject baseObject, Canvas drawCanvas)
        {
            this.baseObject = baseObject;
            this.drawCanvas = drawCanvas;

            //drawCanvas.MouseUp

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

            registerEvents(_basePolygon, _bo, drawCanvas);

            foreach (var o in _bo.Childrens)
            {
                Draw(o);
            }
            _bo.RefreshAfterDraw();
        }

        private void registerEvents(Shape obj, BaseObject bo, Canvas drawWindow)
        {
            obj.MouseEnter += (sender, e) =>
            {
                bo.FireEvent("mousein", null);
            };
            obj.MouseLeave += (sender, e) =>
            {
                bo.FireEvent("mouseout", null);
            };

            obj.MouseMove += (sender, e) =>
            {
                bo.FireEvent("mousemove", null);
            };

            obj.MouseWheel += (sender, e) =>
            {
                bo.FireEvent("mousewheel", null);
            };

            obj.MouseDown += (sender, e) =>
            {
                mouseDownPressed = true;
                _lastMouseDownPressedObject = obj;
                bo.FireEvent("mousedown", null);
            };

            obj.MouseUp += (sender, e) =>
            {
                if (mouseDownPressed && _lastMouseDownPressedObject == obj)
                {
                    mouseDownPressed = false;
                    _lastMouseDownPressedObject = null;
                    bo.FireEvent("click", null);

                }
                bo.FireEvent("mouseup", null);
            };
        }

    }
}
