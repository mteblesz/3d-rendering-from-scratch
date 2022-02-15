using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK1_lab4
{
    public class Camera
    {
        public Vector<double> Position { get; set; }
        public Vector<double> Target { get; set; }
        public readonly Vector<double> UpWorld;

        public Vector<double> DirectionAtSelf { get; set; }



        public Camera(Vertex position, Vertex target)
        {
            Position = position.to4DVector();
            Target = target.to4DVector();
            double[] uw = { 0, 1, 0 };
            UpWorld = DenseVector.OfArray(uw);

           // DirectionAtSelf = 
        }
    }
}
