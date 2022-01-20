using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace GK1_lab4
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
        }
        private readonly double A = 20;
        double alfa = Math.PI/10;
        //  10
        //6 9 7 8
        //2 5 3 4
        //   1
        double[,] points;
        private void Form1_Load(object sender, EventArgs e)
        {
            points = new double[,]{
                {0,0,0,0 }, //0 - nieuzywane 
                { 0, -2*A, 0, 1 }, //1
                {-A, -A, -A, 1 }, //2
                { A, -A, -A, 1},
                { A, -A, A, 1},
                { -A, -A, A, 1}, //5
                { -A, A, -A, 1}, //6
                {A, A, -A, 1 },
                {A, A, A, 1 }, //8
                {-A, A, A, 1},
                {0, 2* A, 0, 1 } //10
            };
            timer1.Enabled = true;
        }

        Point[] vs = new Point[11];

        private void timer1_Tick(object sender, EventArgs e)
        {
            alfa += Math.PI/20;
            Matrix<double> M = P(this.Size.Width, this.Size.Height) * T(0, 0, 4 * A) * R(alfa);
            for(int i=1; i<=10; i++)
            {
                double[] Ai = { points[i, 0], points[i, 1], points[i, 2], points[i, 3] };
                Vector<double> vc = M * DenseVector.OfArray(Ai);
                Vector<double> vn = vc / vc[3];
                vs[i].X = (int)(this.Width * (1 + vn[0]) / 2);
                vs[i].Y = (int)(this.Height * (1 + vn[1]) / 2);
            }
            this.Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

            Pen pen = new Pen(Color.FromArgb(255, 0, 0, 0));


            //  10
            //6 9 7 8
            //2 5 3 4
            //   1
            e.Graphics.DrawLine(pen, vs[10], vs[6]);
            e.Graphics.DrawLine(pen, vs[10], vs[7]);
            e.Graphics.DrawLine(pen, vs[10], vs[8]);
            e.Graphics.DrawLine(pen, vs[10], vs[9]);

            e.Graphics.DrawLine(pen, vs[6], vs[7]);
            e.Graphics.DrawLine(pen, vs[7], vs[8]);
            e.Graphics.DrawLine(pen, vs[8], vs[9]);
            e.Graphics.DrawLine(pen, vs[9], vs[6]);

            e.Graphics.DrawLine(pen, vs[6], vs[2]);
            e.Graphics.DrawLine(pen, vs[7], vs[3]);
            e.Graphics.DrawLine(pen, vs[8], vs[4]);
            e.Graphics.DrawLine(pen, vs[9], vs[5]);

            e.Graphics.DrawLine(pen, vs[2], vs[3]);
            e.Graphics.DrawLine(pen, vs[3], vs[4]);
            e.Graphics.DrawLine(pen, vs[4], vs[5]);
            e.Graphics.DrawLine(pen, vs[5], vs[2]);

            e.Graphics.DrawLine(pen, vs[1], vs[2]);
            e.Graphics.DrawLine(pen, vs[1], vs[3]);
            e.Graphics.DrawLine(pen, vs[1], vs[4]);
            e.Graphics.DrawLine(pen, vs[1], vs[5]);
        }




        //------------------------------------------------------------------------
        private Matrix<double> P(double w, double h)
        {
            return DenseMatrix.OfArray(new double[,] {
            {w/h ,0,0,0},
            {0,1,0,0},
            {0,0,0,1},
            {0,0,-1,0}});
        }
        private Matrix<double> T(double x, double y, double z)
        {
            return DenseMatrix.OfArray(new double[,] {
            {1,0, 0,x},
            {0,1,0,y},
            {0,0,1,z},
            {0,0,0,1}});
        }
        private Matrix<double> R(double alfa)
        {
            return DenseMatrix.OfArray(new double[,] {
            {Math.Cos(alfa),0, -Math.Sin(alfa),0},
            {0,1,0,0},
            {Math.Sin(alfa),0,Math.Cos(alfa),0},
            {0,0,0,1}});
        }


    }
}
//[x, y, z, w]  w==1 docelowo
// W, H -> wys szer okna
//alfa - polowa dlogosci bok/ cos malego -> zmienia sie w trakcie działania aplikacji
//B - odleglosc od kamey np 4a