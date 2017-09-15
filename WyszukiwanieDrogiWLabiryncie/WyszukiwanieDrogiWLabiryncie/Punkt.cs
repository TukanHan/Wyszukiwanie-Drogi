using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WyszukiwanieDrogiWLabiryncie
{   
    /// <summary>
    /// Klasa przechowująca informacje o pozycji
    /// </summary>
    public struct Punkt
    {
        public double X { get; } 
        public double Y { get; }

        public Punkt(double x, double y)
        {
            X = x;
            Y = y;
        }

        public Punkt(Punkt punkt)
        {
            X = punkt.X;
            Y = punkt.Y;
        }

        public static Punkt PozaMapa = new Punkt(-1, -1);
    }
}
