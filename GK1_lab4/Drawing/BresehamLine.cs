using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK1_lab4
{
    //todo zrobic wirtualne pole bitmapowe i z tego wycinac bitmape zeby jak punkt pojdzie poza nia to zeby rysowało
    internal class BresehamLine
    {
        public static void Draw(Bitmap bmp, Point start, Point end)
            => Draw(bmp, start, end, Color.Black);
        public static void Draw(Bitmap bmp, Point start, Point end, Color color)
        {
            if (start.X >= bmp.Width || start.Y >= bmp.Height) return;
            if (end.X >= bmp.Width || end.Y >= bmp.Height) return;
            if (start.X < 0 || start.Y < 0 || end.X < 0 || end.Y < 0) return;
                
            int x1 = start.X;
            int y1 = start.Y;
            int x2 = end.X;
            int y2 = end.Y;
            //      NW  |  NE
            //   WN_____|_____EN
            //   WS     |     ES
            //      SW  |  SE
            if (x1 <= x2 && y1 <= y2)
            {
                if (x2 - x1 > y2 - y1)
                    DrawES(bmp, x1, y1, x2, y2, color);
                else
                    DrawSE(bmp, x1, y1, x2, y2, color);
            }
            else if (x1 > x2 && y1 <= y2)
            {
                if (x1 - x2 > y2 - y1)
                    DrawWS(bmp, x1, y1, x2, y2, color);
                else
                    DrawSW(bmp, x1, y1, x2, y2, color);
            }
            else if (x1 > x2 && y1 > y2)
            {
                if (x1 - x2 > y1 - y2)
                    DrawWN(bmp, x1, y1, x2, y2, color);
                else
                    DrawNW(bmp, x1, y1, x2, y2, color);
            }
            else
            {
                if (x2 - x1 > y1 - y2)
                    DrawEN(bmp, x1, y1, x2, y2, color);
                else
                    DrawNE(bmp, x1, y1, x2, y2, color);
            }
        }
        private static void DrawES(Bitmap bmp, int x1, int y1, int x2, int y2, Color color)
        {
            bmp.SetPixel(0, 0, Color.Red);

            //bresenham jak z wykładu
            int dx = x2 - x1;
            int dy = y2 - y1;
            int d = 2 * dy - dx; //initial value of d
            int incrE = 2 * dy; //increment used for move to E
            int incrSE = 2 * (dy - dx); //increment used for move to NE
            int x = x1;
            int y = y1;
            bmp.SetPixel(x, y, color);
            while (x < x2)
            {
                if (d < 0) //choose E
                {
                    d += incrE;
                    x++;
                }
                else //choose NE
                {
                    d += incrSE;
                    x++;
                    y++;
                }
                bmp.SetPixel(x, y, color);
            }

        }
        private static void DrawSE(Bitmap bmp, int x1, int y1, int x2, int y2, Color color)
        {
            bmp.SetPixel(0, 0, Color.OrangeRed);

            //bresenham 
            int dx = x2 - x1;
            int dy = y2 - y1;
            int d = dy - 2 * dx; //initial value of d
            int incrS = 2 * -dx; //increment used for move to E
            int incrSE = 2 * (dy - dx); //increment used for move to NE
            int x = x1;
            int y = y1;
            bmp.SetPixel(x, y, color);
            while (y < y2)
            {
                if (d > 0) //choose S
                {
                    d += incrS;
                    y++;
                }
                else //choose SE
                {
                    d += incrSE;
                    x++;
                    y++;
                }
                bmp.SetPixel(x, y, color);
            }

        }
        private static void DrawSW(Bitmap bmp, int x1, int y1, int x2, int y2, Color color)
        {
            bmp.SetPixel(0, 0, Color.Orange);

            //bresenham 
            int dx = -(x2 - x1);
            int dy = y2 - y1;
            int d = dy - 2 * dx; //initial value of d
            int incrS = 2 * -dx; //increment used for move to E
            int incrSE = 2 * (dy - dx); //increment used for move to NE
            int x = x1;
            int y = y1;
            bmp.SetPixel(x, y, color);
            while (y < y2)
            {
                if (d > 0) //choose S
                {
                    d += incrS;
                    y++;
                }
                else //choose SE
                {
                    d += incrSE;
                    x--;
                    y++;
                }
                bmp.SetPixel(x, y, color);
            }

        }
        private static void DrawWS(Bitmap bmp, int x1, int y1, int x2, int y2, Color color)
        {
            bmp.SetPixel(0, 0, Color.Yellow);

            //bresenham
            int dx = -(x2 - x1);
            int dy = y2 - y1;
            int d = -2 * dy - dx; //initial value of d
            int incrW = 2 * dy; //increment used for move to E
            int incrSW = 2 * (dy - dx); //increment used for move to NE
            int x = x1;
            int y = y1;
            bmp.SetPixel(x, y, color);
            while (x > x2)
            {
                if (d < 0) //choose W
                {
                    d += incrW;
                    x--;
                }
                else //choose SW
                {
                    d += incrSW;
                    x--;
                    y++;
                }
                bmp.SetPixel(x, y, color);
            }

        }
        private static void DrawWN(Bitmap bmp, int x1, int y1, int x2, int y2, Color color)
        {
            bmp.SetPixel(0, 0, Color.GreenYellow);

            //bresenham
            int dx = -(x2 - x1);
            int dy = -(y2 - y1);
            int d = 2 * dy - dx; //initial value of d
            int incrW = 2 * dy; //increment used for move to E
            int incrNW = 2 * (dy - dx); //increment used for move to NE
            int x = x1;
            int y = y1;
            bmp.SetPixel(x, y, color);
            while (x > x2)
            {
                if (d < 0) //choose W
                {
                    d += incrW;
                    x--;
                }
                else //choose NW
                {
                    d += incrNW;
                    x--;
                    y--;
                }
                bmp.SetPixel(x, y, color);
            }

        }
        private static void DrawNW(Bitmap bmp, int x1, int y1, int x2, int y2, Color color)
        {
            bmp.SetPixel(0, 0, Color.Green);

            //bresenham 
            int dx = -(x2 - x1);
            int dy = -(y2 - y1);
            int d = dy - 2 * dx; //initial value of d
            int incrS = 2 * -dx; //increment used for move to E
            int incrSE = 2 * (dy - dx); //increment used for move to NE
            int x = x1;
            int y = y1;
            bmp.SetPixel(x, y, color);
            while (y > y2)
            {
                if (d > 0) //choose S
                {
                    d += incrS;
                    y--;
                }
                else //choose SE
                {
                    d += incrSE;
                    x--;
                    y--;
                }
                bmp.SetPixel(x, y, color);
            }

        }
        private static void DrawNE(Bitmap bmp, int x1, int y1, int x2, int y2, Color color)
        {
            bmp.SetPixel(0, 0, Color.Blue);

            //bresenham 
            int dx = x2 - x1;
            int dy = -(y2 - y1);
            int d = dy - 2 * dx; //initial value of d
            int incrS = 2 * -dx; //increment used for move to E
            int incrSE = 2 * (dy - dx); //increment used for move to NE
            int x = x1;
            int y = y1;
            bmp.SetPixel(x, y, color);
            while (y > y2)
            {
                if (d > 0) //choose S
                {
                    d += incrS;
                    y--;
                }
                else //choose SE
                {
                    d += incrSE;
                    x++;
                    y--;
                }
                bmp.SetPixel(x, y, color);
            }

        }
        private static void DrawEN(Bitmap bmp, int x1, int y1, int x2, int y2, Color color)
        {
            bmp.SetPixel(0, 0, Color.Purple);

            //bresenham
            int dx = x2 - x1;
            int dy = -(y2 - y1);
            int d = 2 * dy - dx; //initial value of d
            int incrE = 2 * dy; //increment used for move to E
            int incrNE = 2 * (dy - dx); //increment used for move to NE
            int x = x1;
            int y = y1;
            bmp.SetPixel(x, y, color);
            while (x < x2)
            {
                if (d < 0) //choose E
                {
                    d += incrE;
                    x++;
                }
                else //choose NE
                {
                    d += incrNE;
                    x++;
                    y--;
                }
                bmp.SetPixel(x, y, color);
            }
        }
    }
}
