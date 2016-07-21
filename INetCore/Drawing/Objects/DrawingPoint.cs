using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace INetCore.Drawing.Objects
{
    public struct DrawingPoint
    {
        public int X { get; set; }
        public int Y { get; set; }

        public DrawingPoint(float x = 0, float y = 0): this((int)x, (int)y) { }
        public DrawingPoint(int x = 0, int y = 0)
        {
            X = x;
            Y = y;
        }
       

        public static implicit operator Point(DrawingPoint p)
        {
            return new Point(p.X, p.Y);
        }
        public static implicit operator DrawingPoint(Point p)
        {
            return new DrawingPoint(p.X, p.Y);
        }

        public static implicit operator System.Windows.Point(DrawingPoint p)
        {
            return new System.Windows.Point(p.X, p.Y);
        }
        public static implicit operator DrawingPoint(System.Windows.Point p)
        {
            return new DrawingPoint((int)p.X, (int)p.Y);
        }

        public static implicit operator DrawingPoint(int[] p)
        {
            if (p.Length < 2) throw new ArgumentException("Neplatné vstupní argumenty");
            return new DrawingPoint(p[0], p[1]);
        }


        public override string ToString()
        {
            return $"x: {X}, y: {Y}";
        }
    }
}
