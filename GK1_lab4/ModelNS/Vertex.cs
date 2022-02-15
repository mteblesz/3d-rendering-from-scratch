
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
    public class Vertex
    {
        public int index { get; init; }
        public double X { get; init; }
        public double Y { get; init; }
        public double Z { get; init; }
        public double W { get; init; }
        public Vertex(int index, double X, double Y, double Z, double W)
        {
            this.index = index;
            this.X = X;
            this.Y = Y;
            this.Z = Z;
            this.W = W;
        }
        public Vector<double> to4DVector()
        {
            double[] array = {X, Y, Z, W};
            return DenseVector.OfArray(array);
        }
        //public Vertex()
        //{
        //    //empty
        //}
    }
    public struct OSVertex //OnScreenVertex - x, y and depth for z-buffer
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public Point toPoint()
            => new Point((int)X, (int)Y);
    }
}
