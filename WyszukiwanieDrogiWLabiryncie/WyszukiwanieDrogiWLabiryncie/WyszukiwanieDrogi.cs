using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WyszukiwanieDrogiWLabiryncie
{
    /// <summary>
    /// Klasa która wykonuje obliczenia trasy
    /// </summary>
    public class WyszukiwanieDrogi
    {
        private Mapa infrastruktura;

        public WyszukiwanieDrogi(Mapa mapa)
        {
            infrastruktura = mapa;
        }

        public List<Punkt> WyszukajTrase(Punkt start, Punkt koniec)
        {
            List<List<Punkt>> mozliweRuchy = new List<List<Punkt>>();

            if (PrzeszukajMape(start, koniec, mozliweRuchy))
                return new List<Punkt>(RuchyDoCelu(mozliweRuchy, koniec));
            else
                return null;
        }

        private bool PrzeszukajMape(Punkt start, Punkt koniec, List<List<Punkt>> mozliweRuchy)
        {
            //nowa Tablica z oznaczeniami ścierzek
            int[,] tab = new int[infrastruktura.Rozmiar, infrastruktura.Rozmiar];

            //zaznaczam przeszkody
            for (int i = 0; i < infrastruktura.Rozmiar; ++i)
                for (int j = 0; j < infrastruktura.Rozmiar; ++j)
                    if (infrastruktura.Tablica[i, j] == Klocek.Sciana)
                        tab[i, j] = -1;

            //zaznaczam pozycję startową
            mozliweRuchy.Add(new List<Punkt>());
            mozliweRuchy[0].Add(start);
            tab[(int)start.X, (int)start.Y] = 1;

            bool znalezionoDroge = false;
            bool brakDojscia = false;
            int index = 0;
            //pętla poszukiwawcza
            while (!znalezionoDroge && !brakDojscia)
            {
                //sprawdzam czy znalazłem cel
                for (int i = 0; i < mozliweRuchy[index].Count && !znalezionoDroge; ++i)
                    if (mozliweRuchy[index][i].Equals(koniec))
                        znalezionoDroge = true;

                //szukam dalej
                if (!znalezionoDroge)
                {
                    mozliweRuchy.Add(new List<Punkt>());

                    brakDojscia = true;
                    for (int i = 0; i < mozliweRuchy[index].Count; ++i)
                    {
                        //sprawdzam czy mogę przesunąć się na pole obok
                        for (int j = 0; j < 4; ++j)
                        {
                            int testowanePoleX = (int)mozliweRuchy[index][i].X;
                            int testowanePoleY = (int)mozliweRuchy[index][i].Y;

                            if (j == 0) --testowanePoleX;
                            else if (j == 1) ++testowanePoleX;
                            else if (j == 2) --testowanePoleY;
                            else if (j == 3) ++testowanePoleY;

                            if (testowanePoleX > -1 && testowanePoleX < infrastruktura.Rozmiar && testowanePoleY > -1 && testowanePoleY < infrastruktura.Rozmiar && tab[testowanePoleX, testowanePoleY] == 0)
                            {
                                tab[testowanePoleX, testowanePoleY] = index + 2;
                                mozliweRuchy[index + 1].Add(new Punkt(testowanePoleX, testowanePoleY));
                                brakDojscia = false;
                            }
                        }
                    }
                }
                ++index;
            }

            return znalezionoDroge;
        }

        private Punkt[] RuchyDoCelu(List<List<Punkt>> mozliweRuchy, Punkt koniec)
        {
            Punkt[] zwyciezkieRuchy = new Punkt[mozliweRuchy.Count];
            zwyciezkieRuchy[mozliweRuchy.Count - 1] = koniec;
            for (int index = mozliweRuchy.Count - 2; index >= 0; --index)
            {
                for (int j = 0; j < mozliweRuchy[index].Count; ++j)
                {
                    if ((mozliweRuchy[index][j].X == zwyciezkieRuchy[index + 1].X && mozliweRuchy[index][j].Y + 1 == zwyciezkieRuchy[index + 1].Y) ||
                        (mozliweRuchy[index][j].X == zwyciezkieRuchy[index + 1].X && mozliweRuchy[index][j].Y - 1 == zwyciezkieRuchy[index + 1].Y) ||
                        (mozliweRuchy[index][j].X + 1 == zwyciezkieRuchy[index + 1].X && mozliweRuchy[index][j].Y == zwyciezkieRuchy[index + 1].Y) ||
                        (mozliweRuchy[index][j].X - 1 == zwyciezkieRuchy[index + 1].X && mozliweRuchy[index][j].Y == zwyciezkieRuchy[index + 1].Y))
                    {
                        zwyciezkieRuchy[index] = mozliweRuchy[index][j];
                        break;
                    }
                }
            }
            return zwyciezkieRuchy;
        }
    }
}
