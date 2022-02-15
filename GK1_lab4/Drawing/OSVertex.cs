using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK1_lab4
{
    public struct OSVertex //OnScreenVertex - x, y and depth for z-buffer
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public Point toPoint()
            => new Point((int)X, (int)Y);
    }
}
