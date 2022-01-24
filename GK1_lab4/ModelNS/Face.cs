using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK1_lab4.ModelNS
{
    public class Face
    {
        public int index { get; set; }
        public int indexA { get; set; }
        public int indexB { get; set; }
        public int indexC { get; set; }
        public Color color { get; set; }

        public Face(int index, int indexA, int indexB, int indexC)
        {
            this.index = index;
            this.indexA = indexA;
            this.indexB = indexB;
            this.indexC = indexC;
            this.color = Color.Transparent;
        }
        public Face(int index, int indexA, int indexB, int indexC, Color color)
        {
            this.index = index;
            this.indexA = indexA;
            this.indexB = indexB;
            this.indexC = indexC;
            this.color = color;
        }
    }
}
