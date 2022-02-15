using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace GK1_lab4
{
    public class Face
    {
        public int index { get; set; }
        public Vertex A { get; set; }
        public Vertex B { get; set; }
        public Vertex C { get; set; }
        public Vector<double> normal { get; set; }

        private Color baseColor { get; set; }
        public Color color { get; set; }


        public Face(int index, Vertex A, Vertex B, Vertex C)
        {
            this.index = index;
            this.A = A;
            this.B = B;
            this.C = C;
            normal = Utils.normalVectorOfFace(A, B, C);
            color = Color.Transparent;

            //colorful faces
            //Random rnd = new Random();
            baseColor = Color.White;// Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));

        }

        public void ApplyColorIntensity(double intensity)
        {
            if (intensity >= 0)
                this.color = Color.FromArgb(
                    (int)(this.baseColor.R * intensity),
                    (int)(this.baseColor.G * intensity),
                    (int)(this.baseColor.B * intensity));
            else
                this.color = Color.Black;
        }


    }
}
