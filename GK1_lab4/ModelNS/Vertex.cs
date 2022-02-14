using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK1_lab4
{
    public class Vertex
    {
        public int index { get; set; }
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
        public double w { get; set; }
        public Vertex(int index, double x, double y, double z, double w)
        {
            this.index = index;
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
        public Vertex()
        {
            //empty
        }
    }
}
