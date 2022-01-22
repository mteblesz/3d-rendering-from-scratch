using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK1_lab4
{
    internal class Model
    {
        public List<(int index, double x, double y, double z)> vertices = new List<(int index, double x, double y, double z)>();
        public List<(int index, List<int> vertexIndices)> faces = new List<(int index, List<int> vertexIndices)>();
        public Model(string fileName)
        {
            int vCount = 0;
            int fCount = 0;
            //FileStream file = File.Open(fileName, FileMode.Open);
            foreach (string line in File.ReadLines(fileName))
            { 
                //vertices (x, y, z)
                if (line[0] == 'v' && line[1] == ' ')
                {
                    string[] parts = line.Split(' ');
                    vertices.Add((++vCount, Double.Parse(parts[1], CultureInfo.InvariantCulture), 
                                            Double.Parse(parts[2], CultureInfo.InvariantCulture), 
                                            Double.Parse(parts[3], CultureInfo.InvariantCulture)));
                }
                //faces - triagles A-B-C
                else if (line[0] == 'f' && line[1] == ' ') 
                {
                    string[] parts = line.Split(' ');
                    var face = new List<int>();
                    for(int i=1; i<parts.Length; i++)
                    {
                        string[] slashParts = parts[i].Split('/');
                        face.Add(int.Parse(slashParts[0], CultureInfo.InvariantCulture));
                    }
                    faces.Add((++fCount, face));
                }
            }
            foreach (var v in vertices)
                System.Console.WriteLine(v);
            foreach (var f in faces)
            {
                System.Console.Write("f" + f.index + " "); 
                foreach (var i in f.vertexIndices)
                    System.Console.Write(i + " ");
                Console.WriteLine();
            }
        }
    }
}
 