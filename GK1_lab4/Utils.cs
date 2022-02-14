
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
            N[0] = (b.y - a.y) * (c.z - a.z) - (c.y - a.y) * (b.z - a.z);
            N[1] = (b.z - a.z) * (c.x - a.x) - (b.x - a.x) * (c.z - a.z);
            N[2] = (b.x - a.x) * (c.y - a.y) - (c.x - a.x) * (b.y - a.y);

            return DenseVector.OfArray(N).Normalize(1);
        }

        public static Vector<double> getBarycentric(Vertex P, Vertex A, Vertex B, Vertex C)
        {
            throw new NotImplementedException();
        }
        public static Vector<double> zFromBarycentric(Vector<double> bcP, Vertex A, Vertex B, Vertex C)
        {
            throw new NotImplementedException();
        }

    }
}
