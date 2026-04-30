using System;
using System.Collections.Generic;
using System.Drawing;

namespace lab2
{
    public class ShapeRenderer
    {
        private Dictionary<Type, Action<Graphics, Shape>> drawMethods;

        public ShapeRenderer()
        {
            drawMethods = new Dictionary<Type, Action<Graphics, Shape>>
            {
                [typeof(LineShape)] = (g, s) =>
                {
                    var shape = (LineShape)s;
                    g.DrawLine(Pens.Black, shape.X1, shape.Y1, shape.X2, shape.Y2);
                },

                [typeof(RectangleShape)] = (g, s) =>
                {
                    var shape = (RectangleShape)s;
                    g.DrawRectangle(Pens.Blue, shape.X, shape.Y, shape.W, shape.H);
                },

                [typeof(SquareShape)] = (g, s) =>
                {
                    var shape = (RectangleShape)s;
                    g.DrawRectangle(Pens.Blue, shape.X, shape.Y, shape.W, shape.H);
                },

                [typeof(EllipseShape)] = (g, s) =>
                {
                    var shape = (EllipseShape)s;
                    g.DrawEllipse(Pens.Red, shape.X, shape.Y, shape.W, shape.H);
                },

                [typeof(CircleShape)] = (g, s) =>
                {
                    var shape = (EllipseShape)s;
                    g.DrawEllipse(Pens.Red, shape.X, shape.Y, shape.W, shape.H);
                },

                [typeof(TriangleShape)] = (g, s) =>
                {
                    var shape = (TriangleShape)s;
                    g.DrawPolygon(Pens.Green, shape.Points);
                }
            };
        }

        public void Draw(Graphics g, Shape shape)
        {
            Type t = shape.GetType();
            if (drawMethods.ContainsKey(t))
                drawMethods[t](g, shape);
        }
    }
}