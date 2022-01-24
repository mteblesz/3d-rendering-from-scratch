using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK1_lab4.ModelNS
{
    public  class Face
    {
        public int index { get; set; }
        public int indexA { get; set; }
        public int indexB { get; set; }
        public int indexC { get; set; }

        public Face(int index, int indexA, int indexB, int indexC)
        {
            this.index = index;
            this.indexA = indexA;    
            this.indexB = indexB;
            this.indexC = indexC;
        }
    }
}
