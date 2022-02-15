using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK1_lab4
{
    public static class Transformations
    {
        public static Matrix<double> ProjectionFOV(double fov, double f, double n, double w, double h)
        {
            double asp = h/w;
            double ctg = 1 / Math.Tan(fov / 2);
            double denomfn = 1 / (f - n);
            return DenseMatrix.OfArray(new double[,] {
            {ctg*asp, 0, 0,                 0},
            {0, ctg,     0,                 0},
            {0, 0,       (f+n)*denomfn,     -2*f*n*denomfn},
            {0, 0,       1,                 0}});
        }
        public static Matrix<double> Projection(double w, double h)
        {
            return DenseMatrix.OfArray(new double[,] {
            {w/h ,0,0,0},
            {0,1,0,0},
            {0,0,0,1},
            {0,0,-1,0}});
        }
        public static Matrix<double> Translation(double x, double y, double z)
        {
            return DenseMatrix.OfArray(new double[,] {
            {1, 0, 0, x},
            {0, 1, 0, y},
            {0, 0, 1, z},
            {0, 0, 0, 1}});
        }
        public static Matrix<double> RotationY(double alfa)
        {
            return DenseMatrix.OfArray(new double[,] {
            {Math.Cos(alfa),    0,      -Math.Sin(alfa),    0},
            {0,                 1,      0,                  0},
            {Math.Sin(alfa),    0,      Math.Cos(alfa),     0},
            {0,                 0,      0,                  1}});
        }
        public static Matrix<double> RotationX(double alfa)
        {
            return DenseMatrix.OfArray(new double[,] {
            {1,     0,                  0,                  0},
            {0,     Math.Cos(alfa),     -Math.Sin(alfa),    0},
            {0,     Math.Sin(alfa),     Math.Cos(alfa),     0},
            {0,     0,                  0,                  1}});
        }
        public static Matrix<double> RotationZ(double alfa)
        {
            return DenseMatrix.OfArray(new double[,] {
            {Math.Cos(alfa),    -Math.Sin(alfa),    0,  0},
            {Math.Sin(alfa),    Math.Cos(alfa),     0,  0},
            {0,                 0,                  1,  0},
            {0,                 0,                  0,  1}});
        }

        public static Matrix<double> Scaling(double sx, double sy, double sz)
        {
            return DenseMatrix.OfArray(new double[,] {
            {sx, 0, 0, 0},
            {0, sy, 0, 0},
            {0, 0, sz, 0},
            {0, 0, 0, 1}});
        }
    }
}
