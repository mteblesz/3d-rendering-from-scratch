﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GK1_lab4
{
    public class ZBuffor
    {
        public int Width { get; set; }
        public int Heigth { get; set; }
        private double[,] Values { get; set; }

        public ZBuffor(int width, int heigth)
        {
            Width = width;  
            Heigth = heigth;
            Values = new double[Width, Heigth];

            for(int i = 0; i < Width; i++)
                for(int j = 0; j < Heigth; j++)
                    Values[i, j] = double.NegativeInfinity; //Anything would be closer than that
        }

        public bool tryValueAt(int x, int y, double value) 
        {
            if (x < 0 || y < 0 || x >= Width || y >= Heigth)
                return false;

            if (Values[x, y] < value)
            {
                Values[x, y] = value;
                return true;
            }
            return false;

        }

    }
}
