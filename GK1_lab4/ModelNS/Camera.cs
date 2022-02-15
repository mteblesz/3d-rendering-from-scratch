using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace GK1_lab4
{
    public class Camera
    {
        private Vector<double> position; //original
        private Vector<double> target;//original
        public Vector<double> Position { get; set; }
        public Vector<double> Target { get; set; }
        private readonly Vector<double> UpWorld;
        private Vector<double> DirAtSelf;
        private Vector<double> Right;
        private Vector<double> Up;

        public Matrix<double> ViewMatrix { get; set; }

        public Camera(Vector<double> position, Vector<double> target)
        {
            this.position = Position = position;
            this.target = Target = target;

            double[] uw = { 0, 1, 0 };
            UpWorld = DenseVector.OfArray(uw);

            Recalculate();
        }
        public void Recalculate(Vector<double> position, Vector<double> target)
        {
            Position = position;
            Target = target;
            Recalculate();
        }

        public void Recalculate()
        {
            DirAtSelf = -(Position - Target).Normalize(1);
            Right = Utils.Cross3d(UpWorld, DirAtSelf).Normalize(1);
            Up = Utils.Cross3d(DirAtSelf, Right).Normalize(1);

            double[,] array1 = {
                {Right[0],      Right[1],       Right[2],       0},
                {Up[0],         Up[1],          Up[2],          0},
                {DirAtSelf[0],  DirAtSelf[1],   DirAtSelf[2],   0},
                {0,             0,              0,              1 }};
            double[,] array2 = {
                {1, 0, 0, -Position[0]},
                {0, 1, 0, -Position[1]},
                {0, 0, 1, -Position[2]},
                {0, 0, 0, 1}};

            ViewMatrix = DenseMatrix.OfArray(array1) * DenseMatrix.OfArray(array2);
        }


        public void TransformPosition(Matrix<double> M)
        {
            double[] aPos4 = { position[0], position[1], position[2], 1 };
            Vector<double> Pos4 = M * DenseVector.OfArray(aPos4);
            double[] aPos3 = { Pos4[0], Pos4[1], Pos4[2] };
            Position = DenseVector.OfArray(aPos3);
            Recalculate();
        }
        public void TransformTarget(Matrix<double> M)
        {
            double[] aTar4 = { target[0], target[1], target[2], 1 };
            Vector<double> Tar4 = M * DenseVector.OfArray(aTar4);
            double[] aTar3 = { Tar4[0], Tar4[1], Tar4[2] };
            Target = DenseVector.OfArray(aTar3);
            Recalculate();
        }
    }
}
