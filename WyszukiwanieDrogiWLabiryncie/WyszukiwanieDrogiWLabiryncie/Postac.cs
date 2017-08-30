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
    class Postac
    {
        public PozycjaNaMapie pozycja;
        public List<Pozycja> ruchy;
        public bool wDrodze;

        public string obrazekFrontem;
        public string obrazekZlapany;
        public string[] obrazkiPoruszania;
        private uint aktualnaKlatka = 0;
        private uint coIleAnimowac = 0;
        public DispatcherTimer zdarzeniePoruszania;
        public Image obrazekPostaci;
        public int obszarNaMapieX;
        public int obszarNaMapieY;

        public Postac(Image obrazekPostaci, int obszarNaMapieX, int obszarNaMapieY,string frontem, string zlapany, params string[] poruszanie)
        {
            wDrodze = false;
            pozycja = new PozycjaNaMapie();
            this.obszarNaMapieX = obszarNaMapieX;
            this.obszarNaMapieY = obszarNaMapieY;
            this.obrazekPostaci = obrazekPostaci;
            obrazekFrontem = frontem;
            obrazekZlapany = zlapany;
            obrazkiPoruszania = poruszanie;
        }

        public bool WyszukiwanieDrogi(Mapa mapa, PozycjaNaMapie cel)
        {
            List<List<Pozycja>> mozliweRuchy = new List<List<Pozycja>>();

            if (PrzeszukajMape(mapa, cel, mozliweRuchy))
            {
                ruchy = new List<Pozycja>(RuchyDoCelu(mozliweRuchy, cel));

                return true;
            }
            else
                return false;
        }

        private bool PrzeszukajMape(Mapa mapa, PozycjaNaMapie cel, List<List<Pozycja>> mozliweRuchy)
        {
            //nowa tablica z oznaczeniami ścierzek
            int[,] tab = new int[mapa.Rozmiar, mapa.Rozmiar];

            //zaznaczam przeszkody
            for (int i = 0; i < mapa.Rozmiar; ++i)            
                for (int j = 0; j < mapa.Rozmiar; ++j)                
                    if (mapa.tablica[i, j] == Klocek.Sciana) tab[i, j] = -1;

            //zaznaczam pozycję startową
            mozliweRuchy.Add(new List<Pozycja>());
            mozliweRuchy[0].Add(new Pozycja(pozycja));
            tab[pozycja.x, pozycja.y] = 1;


            bool znalezionoDroge = false;
            bool brakDojscia = false;
            int index = 0;
            //pętla poszukiwawcza
            while (!znalezionoDroge && !brakDojscia)
            {
                //sprawdzam czy znalazłem cel
                for (int i = 0; i < mozliweRuchy[index].Count && !znalezionoDroge; ++i)                
                    if (mozliweRuchy[index][i].Equals(cel))                    
                        znalezionoDroge = true;  
                
                //szukam dalej
                if (!znalezionoDroge)
                {
                    mozliweRuchy.Add(new List<Pozycja>());

                    brakDojscia = true;
                    for (int i = 0; i < mozliweRuchy[index].Count; ++i)
                    {
                        //sprawdzam czy mogę przesunąć się na pole obok
                        for(int j=0; j<4; ++j)
                        {
                            int testowanePoleX = mozliweRuchy[index][i].x;
                            int testowanePoleY = mozliweRuchy[index][i].y;

                            switch (j)
                            {
                                case 0: --testowanePoleX; break;
                                case 1: ++testowanePoleX; break;
                                case 2: --testowanePoleY; break;
                                case 3: ++testowanePoleY; break;
                            }

                            if(testowanePoleX > -1 && testowanePoleX < mapa.Rozmiar && testowanePoleY > -1 && testowanePoleY < mapa.Rozmiar && tab[testowanePoleX, testowanePoleY] == 0)
                            {
                                tab[testowanePoleX, testowanePoleY] = index + 2;
                                mozliweRuchy[index + 1].Add(new Pozycja(testowanePoleX, testowanePoleY));
                                brakDojscia = false;
                            }
                        }
                    }
                }
                ++index;
            }

            return znalezionoDroge;
        }

        private Pozycja[] RuchyDoCelu(List<List<Pozycja>> mozliweRuchy, PozycjaNaMapie cel)
        {
            Pozycja[] zwyciezkieRuchy = new Pozycja[mozliweRuchy.Count];
            zwyciezkieRuchy[mozliweRuchy.Count - 1] = new Pozycja(cel);
            for (int index = mozliweRuchy.Count - 2; index >= 0; --index)
            {
                for (int j = 0; j < mozliweRuchy[index].Count; ++j)
                {
                    if ((mozliweRuchy[index][j].x == zwyciezkieRuchy[index + 1].x && mozliweRuchy[index][j].y + 1 ==zwyciezkieRuchy[index + 1].y) ||
                        (mozliweRuchy[index][j].x == zwyciezkieRuchy[index + 1].x && mozliweRuchy[index][j].y - 1 ==zwyciezkieRuchy[index + 1].y) ||
                        (mozliweRuchy[index][j].x + 1 == zwyciezkieRuchy[index + 1].x && mozliweRuchy[index][j].y ==zwyciezkieRuchy[index + 1].y) ||
                        (mozliweRuchy[index][j].x - 1 == zwyciezkieRuchy[index + 1].x && mozliweRuchy[index][j].y ==zwyciezkieRuchy[index + 1].y))
                    {
                        zwyciezkieRuchy[index] = mozliweRuchy[index][j];
                        break;
                    }
                }
            }
            return zwyciezkieRuchy;
        }

        public void Animuj()
        {
            coIleAnimowac %= 4;
            if (coIleAnimowac==0)
            {
                aktualnaKlatka %= (uint)obrazkiPoruszania.Length;
                obrazekPostaci.Source = new BitmapImage(new Uri(obrazkiPoruszania[aktualnaKlatka], UriKind.Relative));
                ++aktualnaKlatka;
            }
            ++coIleAnimowac;
        }
    }
}
