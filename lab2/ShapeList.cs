using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    public class ShapeList
    {
        private List<Shape> shapes = new List<Shape>();

        public void Add(Shape s) => shapes.Add(s);

        public IEnumerable<Shape> GetAll() => shapes;
    }
}