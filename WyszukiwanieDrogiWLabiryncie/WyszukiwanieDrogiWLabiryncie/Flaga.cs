using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WyszukiwanieDrogiWLabiryncie
{
    /// <summary>
    /// Klasa przechowująca informacje o fladze
    /// </summary>
    public class Flaga : IPrzemieczalne
    {
        public Image Obrazek { get; }
        public Punkt PozycjaStartowa { get; }
        public Punkt PozycjaNaMapie { get; private set; }
        public bool CzyJestNaMapie { get; private set; }

        public Flaga(Image obrazek, Punkt pozycjaStartowa)
        {
            Obrazek = obrazek;
            PozycjaStartowa = pozycjaStartowa;
        }

        public void UstawPunktNaMapie(Punkt punkt)
        {
            if (punkt.X == -1 && punkt.Y == -1)
                CzyJestNaMapie = false;
            else
                CzyJestNaMapie = true;

            PozycjaNaMapie = punkt;
        }
    }
}
