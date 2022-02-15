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
        string modelObjName = "manytorus.obj";
        string modelObjPath;

        //starting conditions
        private double A = 3; //Oddalenie  //todo na przyblizeniu uciekaja wierzcholski i ucieka czesc figury (wali error bmp)
        double alfa = 0;
        double alfaplus = Math.PI / 400;
        int refreshInterval = 16;
        double[] lightDir = { 1, 0, -1, 0};
        bool drawEdges = false;


        Model model;
        Vertex[] vertices;
        OSVertex[] oSVertices; //screen vertices (x, y coords, while z is used in z-buffer)

        Vector<double> lightDirVector;
        Bitmap bmpFront;
        ZBuffer zBuffer;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Height = pictureBox1.Height;
            this.Width = pictureBox1.Width;
            // Model
            modelObjPath = "../../../3dEnvironment/" + modelObjName;
            model = new Model(modelObjPath);
            vertices = model.vertices.ToArray();
            oSVertices = new OSVertex[vertices.Length + 1]; //indexed by vertices.index propertly (indices start at 1 in .obj files)

            //light
            lightDirVector = DenseVector.OfArray(lightDir).Normalize(1);

            // Graphics
            bmpFront = new Bitmap(pictureBox1.Image);
            zBuffer = new ZBuffer(pictureBox1.Width, pictureBox1.Height);

            // timer
            timer1.Interval = refreshInterval;
            timer1.Enabled = true;
        }



        private void timer1_Tick(object sender, EventArgs e)
        {
            alfa += alfaplus;
            Matrix<double> M = P(1,1) * T(0, 0, 4 * A) * R(alfa);
            Parallel.ForEach(vertices, v =>
            {
                double[] Ai = { v.X, v.Y, v.Z, v.W };
                Vector<double> vc = M * DenseVector.OfArray(Ai);
                Vector<double> vn = vc / vc[3]; //normalization
                //pacing on screen
                oSVertices[v.index].X = (int)(this.Width * (1 + vn[0]) / 2);
                oSVertices[v.index].Y = (int)(this.Height * (1 + vn[1]) / 2);
                oSVertices[v.index].Z = (this.Height * (1 + vn[2]) / 2); //remains double for zbuffer

            });

            //Light
            Vector<double> lD = lightDirVector * R(alfa);
            Parallel.ForEach(model.faces, face =>
            {
                double intensity = lD[0] * face.normal[0] + lD[1] * face.normal[1] + lD[2] * face.normal[2]; //dot product lD * normal
                //intensity = Math.Max(intensity, 1);
                if (intensity >= 0)
                    face.color = Color.FromArgb((int)(255 * intensity), (int)(255 * intensity), (int)(255 * intensity));
                else
                    face.color = Color.Black;
            });

            //Drawing
            Graphics.FromImage(bmpFront).Clear(Color.Black);
            zBuffer.Reset();
            foreach (var face in model.faces)
            {
                //if (face.color == Color.Black) continue;
                //draw face filled
                OSVertex[] faceOnScreen = { oSVertices[face.A.index], oSVertices[face.B.index], oSVertices[face.C.index] };
                Filling.Draw(bmpFront, zBuffer, faceOnScreen, face.color);
            }
            //edges for testing
            if (drawEdges)
                foreach (var face in model.faces)
                { 
                    OSVertex[] faceOnScreen = { oSVertices[face.A.index], oSVertices[face.B.index], oSVertices[face.C.index] };
                    BresehamLine.Draw(bmpFront, faceOnScreen[0].toPoint(), faceOnScreen[1].toPoint(), Color.Orange);
                    BresehamLine.Draw(bmpFront, faceOnScreen[1].toPoint(), faceOnScreen[2].toPoint(), Color.Orange);
                    BresehamLine.Draw(bmpFront, faceOnScreen[2].toPoint(), faceOnScreen[0].toPoint(), Color.Orange);
                }
            pictureBox1.Image = bmpFront;
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
            {1, 0, 0, x},
            {0, 1, 0, y},
            {0, 0, 1, z},
            {0, 0, 0, 1}});
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