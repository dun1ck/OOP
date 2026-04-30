using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace lab2
{
    public class LineShape : Shape
    {
        public int X1, Y1, X2, Y2;
        public LineShape(int x1, int y1, int x2, int y2) =>
            (X1, Y1, X2, Y2) = (x1, y1, x2, y2);
    }

    public class RectangleShape : Shape
    {
        public int X, Y, W, H;
        public RectangleShape(int x, int y, int w, int h) =>
            (X, Y, W, H) = (x, y, w, h);
    }

    public class SquareShape : RectangleShape
    {
        public SquareShape(int x, int y, int size)
            : base(x, y, size, size) { }
    }

    public class EllipseShape : Shape
    {
        public int X, Y, W, H;
        public EllipseShape(int x, int y, int w, int h) =>
            (X, Y, W, H) = (x, y, w, h);
    }

    public class CircleShape : EllipseShape
    {
        public CircleShape(int x, int y, int size)
            : base(x, y, size, size) { }
    }

    public class TriangleShape : Shape
    {
        public Point[] Points;

        public TriangleShape(int x1, int y1, int x2, int y2, int x3, int y3)
        {
            Points = new[]
            {
                new Point(x1, y1),
                new Point(x2, y2),
                new Point(x3, y3)
            };
        }
    }
}