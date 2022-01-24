﻿using System;
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
using GK1_lab4.ModelNS;

namespace GK1_lab4
{

    public partial class Form1 : Form
    {
        //todo hermetyzacja
        Model model;
        Vertex[] vertices;
        Point[] vs;

        private double A = 2; //Oddalenie
        double alfa = Math.PI / 10;
        double alfaplus = Math.PI / 10;
        int refreshInterval = 100;

        Bitmap bmpFront;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Model
            model = new Model("../../../3dEnvironment/many.obj");
            vertices = model.vertices.ToArray();
            vs = new Point[vertices.Length + 1]; //indexed by vertices.index property

            // Graphics
            bmpFront = new Bitmap(pictureBox1.Image);

            // timer
            timer1.Interval = refreshInterval;
            timer1.Enabled = true;
        }



        private void timer1_Tick(object sender, EventArgs e)
        {
            alfa += alfaplus;
            Matrix<double> M = P(this.Size.Width, this.Size.Height) * T(0, 0, 4 * A) * R(alfa);
            foreach(var Vertex in vertices)
            {
                double[] Ai = { Vertex.x, Vertex.y, Vertex.z, Vertex.w };
                Vector<double> vc = M * DenseVector.OfArray(Ai);
                Vector<double> vn = vc / vc[3];
                vs[Vertex.index].X = (int)(this.Width * (1 + vn[0]) / 2);
                vs[Vertex.index].Y = (int)(this.Height * (1 + vn[1]) / 2);
            }

            //refresh
            this.Invalidate();
            Graphics.FromImage(bmpFront).Clear(Color.Black);
            pictureBox1.Image = bmpFront;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //todo to chyba powinnnobyc po prostu w refreshu
            foreach(var face in model.faces)
            {
                BresehamLine.Draw(bmpFront, vs[face.indexA], vs[face.indexB], Color.White);
                BresehamLine.Draw(bmpFront, vs[face.indexB], vs[face.indexC], Color.White);
                BresehamLine.Draw(bmpFront, vs[face.indexC], vs[face.indexA], Color.White);
            }
        }




        //------------------------------------------------------------------------
        //todo zmienic to w/h na 1
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