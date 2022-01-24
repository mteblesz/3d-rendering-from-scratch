using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;

namespace GK1_lab4.ModelNS
{
    public class Model
    {
        public List<Vertex> vertices = new List<Vertex>();
        public List<Face> faces = new List<Face>();
        public Model(string fileName)
        {
            Random rand = new Random();

            int vCount = 0;
            int fCount = 0;
            //FileStream file = File.Open(fileName, FileMode.Open);
            foreach (string line in File.ReadLines(fileName))
            {
                //vertices (x, y, z)
                if (line[0] == 'v' && line[1] == ' ')
                {
                    string[] parts = line.Split(' ');
                    vertices.Add(new Vertex(++vCount, Double.Parse(parts[1], CultureInfo.InvariantCulture),
                                            Double.Parse(parts[2], CultureInfo.InvariantCulture),
                                            Double.Parse(parts[3], CultureInfo.InvariantCulture),
                                            1
                                            ));
                }
                //faces - triagles A-B-C
                else if (line[0] == 'f' && line[1] == ' ')
                {
                    string[] parts = line.Split(' ');
                    var indexabc = new int[3];
                    for (int i = 0; i < 3; i++)
                    {
                        string[] slashParts = parts[i + 1].Split('/'); //ignore parts[0] := "f"
                        indexabc[i] = int.Parse(slashParts[0], CultureInfo.InvariantCulture);
                    }
                    faces.Add(new Face(++fCount, indexabc[0], indexabc[1], indexabc[2],
                                        Color.FromArgb(rand.Next(256), rand.Next(256), rand.Next(256))));
                }

            }
            foreach(Face face in faces)
            {
                Vertex? a = vertices.Find((Vertex v) => v.index == face.indexA);
                Vertex? b = vertices.Find((Vertex v) => v.index == face.indexB);
                Vertex? c = vertices.Find((Vertex v) => v.index == face.indexC);
                face.normal = normal(a, b, c);
            }

        }

        private Vector<double> normal(Vertex a, Vertex b, Vertex c)
        {
            //https://math.stackexchange.com/questions/305642/how-to-find-surface-normal-of-a-triangle
            double[] N = new double[3];
            N[0] = (b.y - a.y) * (c.z - a.z) - (c.y - a.y) * (b.z - a.z);
            N[1] = (b.z - a.z) * (c.x - a.x) - (b.x - a.x) * (c.z - a.z);
            N[2] = (b.x - a.x) * (c.y - a.y) - (c.x - a.x) * (b.y - a.y);
            return DenseVector.OfArray(N);
        }
    }
}
