using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2
{
    public class ShapeFactoryRegistry
    {
        private Dictionary<string, Func<int[], Shape>> factories =
            new Dictionary<string, Func<int[], Shape>>();

        public void Register(string key, Func<int[], Shape> creator)
        {
            factories[key] = creator;
        }

        public Shape Create(string key, int[] args)
        {
            return factories[key](args);
        }

        public IEnumerable<string> Keys => factories.Keys;
    }
}