
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
        //original model:
        public int index { get; init; }
        private readonly double x;
        private readonly double y;
        private readonly double z;
        private readonly double w;

        //currently transformed:
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double W { get; set; }
        public Vertex(int index, double X, double Y, double Z, double W = 1)
        {
            this.index = index;
            this.x = this.X = X;
            this.y = this.Y = Y;
            this.z = this.Z = Z;
            this.w = this.W = W;
        }
        //public Vector<double> to3DVector()
        //{
        //    double[] array = { X, Y, Z};
        //    return DenseVector.OfArray(array);
        //}
        public Vector<double> to4DVector()
        {
            double[] array = { X, Y, Z, W };
            return DenseVector.OfArray(array);
        }

        private Vector<double> to4DVectorOrg()
        {
            double[] array = { x, y, z, w };
            return DenseVector.OfArray(array);
        }
        public Vertex Transform(Matrix<double> M)
        {
            Vector<double> vc = M * this.to4DVectorOrg();
            vc /= vc[3]; //normalization

            this.X = vc[0];
            this.Y = vc[1];
            this.Z = vc[2];
            this.W = vc[3];
            return this;
        }

    }

}
