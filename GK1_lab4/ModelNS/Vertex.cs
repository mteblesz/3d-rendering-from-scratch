using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK1_lab4.ModelNS
{
    public class Vertex
    {
        public int index;
        public double x;
        public double y;
        public double z;
        public double w;
        public Vertex(int index, double x, double y, double z, double w)
        {
            this.index = index;
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }
    }
}
