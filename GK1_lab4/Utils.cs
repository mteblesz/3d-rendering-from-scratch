
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace GK1_lab4
{
    public static class Utils
    {

        public static Vector<double> normalVectorOfFace(Vertex a, Vertex b, Vertex c)
        {
            //https://math.stackexchange.com/questions/305642/how-to-find-surface-normal-of-a-triangle

            double[] N = new double[3];
            N[0] = (b.Y - a.Y) * (c.Z - a.Z) - (c.Y - a.Y) * (b.Z - a.Z);
            N[1] = (b.Z - a.Z) * (c.X - a.X) - (b.X - a.X) * (c.Z - a.Z);
            N[2] = (b.X - a.X) * (c.Y - a.Y) - (c.X - a.X) * (b.Y - a.Y);

            return DenseVector.OfArray(N).Normalize(1);
        }
        public static Vector<double> get2DVector(OSVertex osv)
        {
            double[] dv = { osv.X, osv.Y };
            return DenseVector.OfArray(dv);
        }
        public static Vector<double> toBarycentric(OSVertex P, OSVertex A, OSVertex B, OSVertex C)
        {
            Vector<double> a = get2DVector(A);
            Vector<double> b = get2DVector(B);
            Vector<double> c = get2DVector(C);
            Vector<double> p = get2DVector(P);

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
            double u = 1.0f - v - w;

            double[] result = { v, w, u };
            return DenseVector.OfArray(result);
        }
        public static double zFromBarycentric(Vector<double> bcP, Vertex A, Vertex B, Vertex C)
        {
            //if (bcP.Count() != 3) throw new ArgumentException();
            return bcP[0] * A.Z + bcP[1] * B.Z + bcP[2] * C.Z;
        }

    }
}
