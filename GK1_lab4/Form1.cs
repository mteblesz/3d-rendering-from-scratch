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
using GK1_lab4.ModelNS;

namespace GK1_lab4
{

    public partial class Form1 : Form
    {
        string modelObjName = "manytorus.obj";
        string modelObjPath;

        //todo hermetyzacja
        Model model;
        Vertex[] vertices;
        Point[] vs;

        private double A = 3; //Oddalenie  //todo na przyblizeniu uciekaja wierzcholski i ucieka czesc figury (wali error bmp)
        double alfa = Math.PI / 10;
        double alfaplus = Math.PI / 100;
        int refreshInterval = 16;
        double[] lightDir = { 0, 0, -1 , 0};

        Vector<double> lightDirVector;

        Bitmap bmpFront;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Model
            modelObjPath = "../../../3dEnvironment/" + modelObjName;
            model = new Model(modelObjPath);
            vertices = model.vertices.ToArray();
            vs = new Point[vertices.Length + 1]; //indexed by vertices.index propertly (indices start at 1 in .obj files)

            //light
            lightDirVector = DenseVector.OfArray(lightDir).Normalize(1);

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
            Parallel.ForEach(vertices, v =>
            {
                double[] Ai = { v.x, v.y, v.z, v.w };
                Vector<double> vc = M * DenseVector.OfArray(Ai);
                Vector<double> vn = vc / vc[3];
                vs[v.index].X = (int)(this.Width * (1 + vn[0]) / 2);
                vs[v.index].Y = (int)(this.Height * (1 + vn[1]) / 2);

            });

            //Light
            Parallel.ForEach(model.faces, face =>
            {
                Vector<double> lD = lightDirVector * R(-2*alfa);
                double intensity = lD[0] * face.normal[0] + lD[1] * face.normal[1] + lD[2] * face.normal[2]; //dot product lD * normal
                //intensity = Math.Max(intensity, 1);
                if (intensity >= 0)
                    face.color = Color.FromArgb((int)(255 * intensity), (int)(255 * intensity), (int)(255 * intensity));
                else
                    face.color = Color.Transparent;
            });

            //Drawing
            Graphics.FromImage(bmpFront).Clear(Color.Black);
            foreach (var face in model.faces)
            {
                if (face.color == Color.Transparent) continue;
                //draw face filled
                Point[] ind = { vs[face.indexA], vs[face.indexB], vs[face.indexC] };
                Filling.Draw(bmpFront, ind, face.color);
                //edges
                //BresehamLine.Draw(bmpFront, ind[0], ind[1], Color.Orange);
                //BresehamLine.Draw(bmpFront, ind[1], ind[2], Color.Orange);
                //BresehamLine.Draw(bmpFront, ind[2], ind[0], Color.Orange);
            }
            pictureBox1.Image = bmpFront;
        }




        //------------------------------------------------------------------------
        //todo zmienic to w/h na 1
        private Matrix<double> P(double w, double h)
        {
            return DenseMatrix.OfArray(new double[,] {
            {1 ,0,0,0},
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