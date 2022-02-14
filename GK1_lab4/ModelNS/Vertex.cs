
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
        //public Vertex()
        //{
        //    //empty
        //}
    }
    public struct OSVertex //OnScreen
    {
        public int X { get; set; }
        public int Y { get; set; }
        public double Z { get; set; }


        public OSVertex(int x, int y, int z = 0)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public Point toPoint()
            => new Point(X, Y);
        public static bool operator ==(OSVertex v, OSVertex w)
        {
            return w.X == v.X && w.Y == v.Y && w.Z == v.Z;
        }
        public static bool operator !=(OSVertex v, OSVertex w)
        {
            return !(v == w);
        }
    }
}
