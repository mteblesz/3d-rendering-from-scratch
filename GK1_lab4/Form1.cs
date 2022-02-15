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
        private double A = 3; //Oddalenie  
        double alfa = 0;
        double alfaplus = Math.PI / 50;
        int refreshInterval = 33;
        double[] lightDir = { 0, -1, -1, 0};


        int cameraType = 0; //0 - still,  1- tracing, 2- following
        Camera camera1;
        double[] camera1Position = { 0, 0, 12 };
        double[] camera1Target = { 0, 0, 0 };
        Camera camera2;
        double[] camera2Position = { 0, 5, 12 };
        double[] camera2Target = { -2, 0, 0 };

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
            oSVertices = new OSVertex[vertices.Length + 1]; //todo: change indexing to posessing : indexed by vertices.index propertly (indices start at 1 in .obj files)

            camera1 = new Camera(DenseVector.OfArray(camera1Position), DenseVector.OfArray(camera1Target));
            camera2 = new Camera(DenseVector.OfArray(camera2Position), DenseVector.OfArray(camera2Target));

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

            Matrix<double> M;
            switch (cameraType)
            {
                default:
                case 0: //camera is still
                    alfa += alfaplus;
                    M = Transformations.Translation(0, 0, 12) * Transformations.RotationY(alfa);
                    break;
                case 1: //camera traces a point
                    M =  Transformations.Translation(0, 18 * Math.Sin(alfa), 0);
                    camera1.TransformTarget(M);
                    M = camera1.ViewMatrix * M;
                    break;
                case 2: //camera follows point
                    M = Transformations.Translation(4 * Math.Sin(alfa), 0,  0);
                    camera2.TransformTarget(M);
                    camera2.TransformPosition(M);
                    M = camera2.ViewMatrix;
                    break;
            }
            
            //Vertices positions
            Parallel.ForEach(vertices, v =>
            {
                v.Transform(Transformations.Projection(1, 1) * M);
                //placing on screen (OnScreenVertices)
                oSVertices[v.index].X = (int)(this.Width * (1 + v.X) / 2);
                oSVertices[v.index].Y = (int)(this.Height * (1 + v.Y) / 2);
                oSVertices[v.index].Z = (int)(this.Height * (1 + v.Z) / 2);

            });

            //Light
            Parallel.ForEach(model.faces, face =>
            {
            //liczy oswietlenie jeszcze bez obrotu
            double intensity = lightDirVector.DotProduct(Utils.normalVectorOfFace(face));
                face.ApplyColorIntensity(intensity);
            });

            //Drawing
            Graphics.FromImage(bmpFront).Clear(Color.Black);
            zBuffer.Reset();
            foreach (var face in model.faces)
            {
                OSVertex[] faceOnScreen = { 
                    oSVertices[face.A.index], 
                    oSVertices[face.B.index], 
                    oSVertices[face.C.index] };
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