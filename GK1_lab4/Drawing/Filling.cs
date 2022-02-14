using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace GK1_lab4
{
    internal static class Filling
    {
        #region Border class
        protected class Border
        {
            public readonly OSVertex a; //higher
            public readonly OSVertex b; //lower
            private double currx;
            private double dx;
            public Border(OSVertex v, OSVertex w)
            {
                if (v.Y <= w.Y)
                {
                    a = v;
                    b = w;
                }
                else
                {
                    a = w;
                    b = v;
                }
                currx = a.X;
                dx = (double)(b.X - a.X) / (b.Y - a.Y); //TODO pewnie lepiej by bylo bresenhamowac(??) ://
            }
            public int getNextX()
            {
                currx += dx;
                return getCurrX();
            }
            public int getCurrX()
                => (int)currx;
            //todo jakby sie okazalo ze gradienty potrzebne to wtedy tez zamiast Point -> ColoredPoint
            //public Color getXColor(int y)
            //{
            //    double fraction = (double)(y - a.Y) / (b.Y - a.Y);
            //    double dfraction = (1 - fraction);
            //    int R = (int)(a.color.R * dfraction + b.color.R * fraction);
            //    int G = (int)(a.color.G * dfraction + b.color.G * fraction);
            //    int B = (int)(a.color.B * dfraction + b.color.B * fraction);

            //    return Color.FromArgb(R, G, B);
            //}
            public bool Equals(Border b)
            {
                return b.a == this.a && b.b == this.b;
            }
        }
        #endregion
        public static void Draw(Bitmap bmp, OSVertex[] abc, Color fillingColor) //abc - vertices of a tringle to fill
        {
            abc = PointYvalInsertionSort(abc);
            int ymin = abc[0].Y;
            int ymax = abc[2].Y;
            List<Border> aet = new List<Border>(); //here edge has point of lower y as v1

            for (int y = ymin + 1; y <= ymax; y++)
            {
                for (int i = 0; i < 3; i++) //for each point in triangle
                {
                    OSVertex v = abc[i];
                    if (v.Y == y - 1)
                    {
                        OSVertex prevv = abc[(i + 2) % 3];
                        Border bPrev = new Border(v, prevv);
                        if (prevv.Y > v.Y)
                            aet.Add(bPrev);
                        else if (prevv.Y < v.Y)
                            aet.Remove(aet.Find((Border b) => b.Equals(bPrev)));

                        OSVertex nextv = abc[(i + 1) % 3];
                        Border bNext = new Border(v, nextv);
                        if (nextv.Y > v.Y)
                            aet.Add(bNext);
                        else if (nextv.Y < v.Y)
                            aet.Remove(aet.Find((Border b) => b.Equals(bNext)));
                    }
                }
                aet.Sort((Border b1, Border b2) => { return b1.getCurrX().CompareTo(b2.getCurrX()); });


                int x1 = aet[0].getNextX();
                int x2 = aet[1].getNextX();
                for (int x = x1; x <= x2; x++)
                {
                    //Z-buffor todo
                    //to barycentric

                    //extract z

                    //check z buffor condition

                    //draw
                    bmp.SetPixel(x, y, fillingColor);
                }


            }
        }
        private static OSVertex[] PointYvalInsertionSort(OSVertex[] inputArray) //todo change to only sort 3 numbers
        {
            for (int i = 0; i < inputArray.Length - 1; i++)
            {
                for (int j = i + 1; j > 0; j--)
                {
                    if (inputArray[j - 1].Y > inputArray[j].Y)
                    {
                        OSVertex temp = inputArray[j - 1];
                        inputArray[j - 1] = inputArray[j];
                        inputArray[j] = temp;
                    }
                }
            }
            return inputArray;
        }



    }
}
