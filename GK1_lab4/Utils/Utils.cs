
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
    public static class Utils
    {
        public static Vector<double> Cross3d(Vector<double> left, Vector<double> right)
        {//https://stackoverflow.com/questions/11759720/cross-product-using-math-net-numerics-with-c-sharp
            if ((left.Count != 3 || right.Count != 3))
            {
                string message = "Vectors must have a length of 3.";
                throw new Exception(message);
            }
            Vector<double> result = new DenseVector(3);
            result[0] = left[1] * right[2] - left[2] * right[1];
            result[1] = -left[0] * right[2] + left[2] * right[0];
            result[2] = left[0] * right[1] - left[1] * right[0];

            return result;
        }
        public static Vector<double> normalVectorOfFace(Vertex a, Vertex b, Vertex c)
        {
            //https://math.stackexchange.com/questions/305642/how-to-find-surface-normal-of-a-triangle

            double[] N = new double[4];
            N[0] = (b.Y - a.Y) * (c.Z - a.Z) - (c.Y - a.Y) * (b.Z - a.Z);
            N[1] = (b.Z - a.Z) * (c.X - a.X) - (b.X - a.X) * (c.Z - a.Z);
            N[2] = (b.X - a.X) * (c.Y - a.Y) - (c.X - a.X) * (b.Y - a.Y);
            N[3] = 0;
            return DenseVector.OfArray(N).Normalize(1);
        }
        public static Vector<double> normalVectorOfFace(Face f)
        {
            return normalVectorOfFace(f.A, f.B, f.C);
        }


        public static Vector<double> get2DVector(Point P)
        {
            double[] arrayP = { P.X, P.Y };
            return DenseVector.OfArray(arrayP);
        }
        public static Vector<double> get2DVector(OSVertex osv)
        {
            double[] arrayOSV = { osv.X, osv.Y };
            return DenseVector.OfArray(arrayOSV);
        }
        public static double zValue(Point P, OSVertex A, OSVertex B, OSVertex C) 
        {
            //get barycentric coordinates of P
            Vector<double> p = get2DVector(P);
            Vector<double> a = get2DVector(A);
            Vector<double> b = get2DVector(B);
            Vector<double> c = get2DVector(C);

            Vector<double> v0 = b - a;
            Vector<double> v1 = c - a;
            Vector<double> v2 = p - a;
            double d00 = v0.DotProduct(v0);
            double d01 = v0.DotProduct(v1);
            double d11 = v1.DotProduct(v1);
            double d20 = v2.DotProduct(v0);
            double d21 = v2.DotProduct(v1);
            double invDenom = 1.0 / (d00 * d11 - d01 * d01);

            double v = (d11 * d20 - d01 * d21) * invDenom;
            double w = (d00 * d21 - d01 * d20) * invDenom;
            double u = 1.0 - v - w;

            //calculate z value
            return u * A.Z + v * B.Z + w * C.Z;
        }
    }
}
