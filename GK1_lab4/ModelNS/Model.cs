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

namespace GK1_lab4
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

                    Vertex? a = vertices.Find((Vertex v) => v.index == indexabc[0]);
                    Vertex? b = vertices.Find((Vertex v) => v.index == indexabc[1]);
                    Vertex? c = vertices.Find((Vertex v) => v.index == indexabc[2]);
                    if(a != null && b != null && c != null)
                        faces.Add(new Face(++fCount, a, b, c));
                }

            }

        }

        public Model(Model m) //todo for making a scene maybe
        {
            throw new NotImplementedException();
        }

    }
}
