using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WyszukiwanieDrogiWLabiryncie
{
    public enum Klocek { Podloga, Sciana };


    /// <summary>
    /// Klasa przechowująca wszelkie informacje związane z mapą 
    /// i udostępniająca metody modyfikacji i generowania nowych map
    /// </summary>
    public class Mapa
    {
        public Klocek[,] Tablica { get; private set; }

        public int Rozmiar { get { return Tablica.GetLength(0); } }

        public Mapa(uint rozmiar)
        {
            Tablica = new Klocek[rozmiar, rozmiar];
        }

        public Mapa(Klocek[,] inneZrodlo)
        {
            Tablica = new Klocek[inneZrodlo.GetLength(0), inneZrodlo.GetLength(1)];
            Array.Copy(inneZrodlo, Tablica, inneZrodlo.Length);
        }

        public void ZamienKlocek(Klocek klocek, int x, int y)
        {
            Tablica[x, y] = klocek;
        }

        public static Mapa LosujMape()
        {
            Random generatorLosowy = new Random();

            Mapa nowaMapa = new Mapa((uint)generatorLosowy.Next(4, 11));

            int maxKostki = (int)Math.Pow(nowaMapa.Tablica.GetLength(0),2) / 2;
            int minKostki = (int)Math.Pow(nowaMapa.Tablica.GetLength(0), 2) / 8;
            int kostki = generatorLosowy.Next(minKostki, maxKostki);

            for (int i = 0; i < kostki; ++i)
            {
                int x = generatorLosowy.Next(0, nowaMapa.Tablica.GetLength(0));
                int y = generatorLosowy.Next(0, nowaMapa.Tablica.GetLength(0));

                nowaMapa.Tablica[x, y] = Klocek.Sciana;
            }

            return nowaMapa;
        }
    }
}
