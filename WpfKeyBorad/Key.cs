using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfKeyBorad
{
    class Key
    {
        const int DEFAULT_HEIGHT = 50;
        const int DEFAULT_WIDTH = 50;

        public Key(KeybordKey key, double height = DEFAULT_HEIGHT, double width = DEFAULT_WIDTH)
        {
            Height = height;
            Width = width;
            SomeKey = key;
        }

        public double Height { get; set; }

        public double Width { get; set; }

        public KeybordKey SomeKey { get; set; }
    }
}
