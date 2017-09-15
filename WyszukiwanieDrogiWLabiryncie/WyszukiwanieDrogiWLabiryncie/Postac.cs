using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WyszukiwanieDrogiWLabiryncie
{
    /// <summary>
    /// Klasa przechowująca położenie / obrazki / wszystkie dane związane z postaciami
    /// Udostępnia całą logikę związaną z wyszukiwaniem drogi
    /// </summary>
    class Postac : IPrzemieczalne
    {
        public Image Obrazek { get; }
        public Punkt PozycjaStartowa { get; }
        public Punkt PozycjaNaMapie { get; private set; }
        public bool CzyJestNaMapie { get; private set; }

        public List<Punkt> ruchy;

        public string obrazekFrontem;
        public string obrazekZlapany;
        public string[] obrazkiPoruszania;
        private uint aktualnaKlatka = 0;
        private uint coIleAnimowac = 0;
        public DispatcherTimer zdarzeniePoruszania;
        
        public Postac(Image obrazek, Punkt pozycjaStartowa, string frontem, string zlapany, params string[] poruszanie)
        {
            Obrazek = obrazek;
            PozycjaStartowa = pozycjaStartowa;
            
            obrazekFrontem = frontem;
            obrazekZlapany = zlapany;
            obrazkiPoruszania = poruszanie;
            
            zdarzeniePoruszania = new DispatcherTimer();
            zdarzeniePoruszania.Interval = new TimeSpan(0, 0, 0, 0, 15);
        }

        public void UstawPunktNaMapie(Punkt punkt)
        {
            if (punkt.X == -1 && punkt.Y == -1)
                CzyJestNaMapie = false;
            else
                CzyJestNaMapie = true;

            PozycjaNaMapie = punkt;
        }

        public void Animuj()
        {
            coIleAnimowac %= 4;
            if (coIleAnimowac==0)
            {
                aktualnaKlatka %= (uint)obrazkiPoruszania.Length;
                Obrazek.Source = new BitmapImage(new Uri(obrazkiPoruszania[aktualnaKlatka], UriKind.Relative));
                ++aktualnaKlatka;
            }
            ++coIleAnimowac;
        }
    }
}
