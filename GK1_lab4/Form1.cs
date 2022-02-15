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
        string modelObjName1 = "manytorus.obj";
        string modelObjName2 = "cube.obj";
        Model[] models;


        //starting conditions 
        double alfa = 0;
        double alfaplus = Math.PI / 100;
        int refreshInterval = 33;
        double[] lightDir = { -1, 0, -1, 0 };


        int cameraType = 0; //0 - still,  1- tracing, 2- following
        Camera camera1;
        double[] camera1Position = { 6, 0, -12 };
        double[] camera1Target = { 0, 0, 0 };
        Camera camera2;
        double[] camera2Position = {-15, 7, 15 };


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
            models = new Model[2];
            models[0] = new Model("../../../3dEnvironment/" + modelObjName1);
            models[1] = new Model("../../../3dEnvironment/" + modelObjName2);

            // Cameras
            double[] camera2Target = { models[1].vertices[1].X, models[1].vertices[1].Y, models[1].vertices[1].Z };
            camera1 = new Camera(DenseVector.OfArray(camera1Position), DenseVector.OfArray(camera1Target));
            camera2 = new Camera(DenseVector.OfArray(camera2Position), DenseVector.OfArray(camera2Target));

            // light
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

            Matrix<double> M = Transformations.Identity();
            Matrix<double> M2 = Transformations.Identity();
            switch (cameraType)
            {
                default:
                case 0: //camera is still
                    alfa += alfaplus;
                    M = Transformations.Translation(0, 0, 20) * Transformations.RotationY(alfa);
                    M2 = Transformations.Translation(0, 0, 20) * Transformations.RotationY(-alfa);
                    break;
                case 1: //camera traces a point
                    M = Transformations.Translation(0, 20 * Math.Sin(alfa), 0);
                    camera1.TransformTarget(M);
                    M = camera1.ViewMatrix * M;
                    M2 = camera1.ViewMatrix * M2;
                    break;
                case 2: //camera follows point
                    M2 = Transformations.Translation(8 * Math.Sin(2*alfa), 0, 0);
                    camera2.TransformTarget(M2);
                    camera2.TransformPosition(M2);
                    M = camera2.ViewMatrix * M;
                    M2 = camera2.ViewMatrix * M2;
                    break;
            }

            //Vertices positions
            Parallel.ForEach(models[0].vertices, v =>
            {
                v.Transform(Transformations.Projection(1, 1) * M);
                //placing on screen (OnScreenVertices)
                models[0].oSVertices[v.index].X = (int)(this.Width * (1 + v.X) / 2);
                models[0].oSVertices[v.index].Y = (int)(this.Height * (1 + v.Y) / 2);
                models[0].oSVertices[v.index].Z = (int)(this.Height * (1 + v.Z) / 2);

            });
            //Vertices positions
            Parallel.ForEach(models[1].vertices, v =>
            {
                v.Transform(Transformations.Projection(1, 1)* M2);
                //placing on screen (OnScreenVertices)
                models[1].oSVertices[v.index].X = (int)(this.Width * (1 + v.X) / 2);
                models[1].oSVertices[v.index].Y = (int)(this.Height * (1 + v.Y) / 2);
                models[1].oSVertices[v.index].Z = (int)(this.Height * (1 + v.Z) / 2);

            });


            //Light
            foreach (Model model in models)
                Parallel.ForEach(model.faces, face =>
                {
                //liczy oswietlenie jeszcze bez obrotu
                double intensity = lightDirVector.DotProduct(Utils.normalVectorOfFace(face));
                    face.ApplyColorIntensity(intensity);
                });

            //Drawing
            zBuffer.Reset();
            Graphics.FromImage(bmpFront).Clear(Color.Black);
            foreach (Model model in models)
                foreach (var face in model.faces)
                {
                    OSVertex[] faceOnScreen = {
                    model.oSVertices[face.A.index],
                    model.oSVertices[face.B.index],
                    model.oSVertices[face.C.index] };
                    Filling.Draw(bmpFront, zBuffer, faceOnScreen, face.color);

                }

            pictureBox1.Image = bmpFront;
        }




        //CONTROLS
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                alfa = 0;
                cameraType = (cameraType + 1) % 3;
            }
            else if (e.Button == MouseButtons.Right)
                ;//shadingType = (shadingType + 1) % 3; //todo

        }
    }
}