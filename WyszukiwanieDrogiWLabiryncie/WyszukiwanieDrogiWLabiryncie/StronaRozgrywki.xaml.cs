using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WyszukiwanieDrogiWLabiryncie
{
    /// <summary>
    /// Strona odpowiedzialna za włąściwą rozgrywkę.
    /// Przesówanie postaci i flagi oraz przemieszczanie się po planszy.
    /// </summary>
    public partial class StronaRozgrywki : UserControl
    {
        enum TrybProgramu { Oczekuje, WTrakcie};

        private Flaga flaga;
        private Postac[] postacie;
        private Object zlapanyObiekt = null;
        private TrybProgramu trybProgramu;

        private double skala;

        public StronaRozgrywki()
        {
            InitializeComponent();

            postacie = new Postac[]
            {
                 new Postac(Postac0 , new Punkt(475, 64),"o_postacie/p1_front.png", "o_postacie/p1_jump.png", "o_postacie/p1_walk01.png", "o_postacie/p1_walk02.png",
                "o_postacie/p1_walk03.png", "o_postacie/p1_walk04.png", "o_postacie/p1_walk05.png", "o_postacie/p1_walk06.png", "o_postacie/p1_walk07.png",
                "o_postacie/p1_walk08.png", "o_postacie/p1_walk09.png", "o_postacie/p1_walk10.png", "o_postacie/p1_walk11.png"),
                 new Postac(Postac1, new Punkt(418, 64), "o_postacie/p2_front.png", "o_postacie/p2_jump.png", "o_postacie/p2_walk01.png", "o_postacie/p2_walk02.png",
                "o_postacie/p2_walk03.png", "o_postacie/p2_walk04.png", "o_postacie/p2_walk05.png", "o_postacie/p2_walk06.png", "o_postacie/p2_walk07.png",
                "o_postacie/p2_walk08.png", "o_postacie/p2_walk09.png", "o_postacie/p2_walk10.png", "o_postacie/p2_walk11.png"),
                 new Postac(Postac2, new Punkt(533, 64), "o_postacie/p3_front.png", "o_postacie/p3_jump.png", "o_postacie/p3_walk01.png", "o_postacie/p3_walk02.png",
                "o_postacie/p3_walk03.png", "o_postacie/p3_walk04.png", "o_postacie/p3_walk05.png", "o_postacie/p3_walk06.png", "o_postacie/p3_walk07.png",
                "o_postacie/p3_walk08.png", "o_postacie/p3_walk09.png", "o_postacie/p3_walk10.png", "o_postacie/p3_walk11.png")
            };

            foreach(Postac postac in postacie)
            {
                postac.zdarzeniePoruszania.Tick += (s, args) => ZdarzeniePoruszania(postac);
                postac.Obrazek.MouseUp += (s, args) => PuscObiekt(postac);
                postac.Obrazek.MouseMove += PrzesowajObiekt;
                postac.Obrazek.MouseDown += (s, args) => ZlapObiekt( postac);
            }

            flaga = new Flaga(obrazekFlagi, new Punkt(474,194));
            flaga.Obrazek.MouseUp += (s, args) => PuscObiekt(flaga);
            flaga.Obrazek.MouseMove += PrzesowajObiekt;
            flaga.Obrazek.MouseDown += (s, args) => ZlapObiekt(flaga);
        }

        private void OdswierzenieOkna(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (canvasRozgrywki.Visibility == Visibility.Visible)
            {
                skala = 400.0 / MainWindow.mainWindowObject.mapa.Rozmiar;
                tlo.Width = MainWindow.mainWindowObject.mapa.Rozmiar * (int)skala;
                tlo.Height = MainWindow.mainWindowObject.mapa.Rozmiar * (int)skala;

                var obiektyDoUsuniecia = canvasRozgrywki.Children.OfType<Image>().Where((obrazek) => obrazek.Tag!=null && obrazek.Tag.Equals("Pole")).ToArray();
                foreach (Image img in obiektyDoUsuniecia)
                {
                    canvasRozgrywki.Children.Remove(img);
                }

                for (int i = 0; i < MainWindow.mainWindowObject.mapa.Rozmiar; ++i)
                {
                    for (int j = 0; j < MainWindow.mainWindowObject.mapa.Rozmiar; ++j)
                    {
                        Image img = new Image();

                        if (MainWindow.mainWindowObject.mapa.Tablica[i, j] == Klocek.Sciana)
                            img.Source = new BitmapImage(new Uri("obrazki/o_sciana.png", UriKind.Relative));
                        else
                            img.Source = new BitmapImage(new Uri("obrazki/o_podloga.png", UriKind.Relative));

                        img.Height = img.Width = skala;
                        img.Tag = "Pole";

                        Canvas.SetTop(img, i * (int)skala);
                        Canvas.SetLeft(img, j * (int)skala);

                        canvasRozgrywki.Children.Add(img);
                    }
                }
            }
        }

        #region Przemieszczanie obieków
        private void PrzesowajObiekt(object sender, MouseEventArgs e)
        {
            if (zlapanyObiekt == sender )
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    if (Mouse.OverrideCursor == Cursors.Hand)
                    {
                        Image objTextbox = sender as Image;

                        Point mousePoint = e.GetPosition(this);

                        Mouse.Capture(sender as UIElement);
                        Canvas.SetTop(objTextbox, mousePoint.Y - objTextbox.ActualHeight / 2);
                        Canvas.SetLeft(objTextbox, mousePoint.X - objTextbox.ActualWidth / 2);
                    }
                }
            }          
        }

        private void PuscObiekt(IPrzemieczalne obiekt)
        {
            if (zlapanyObiekt == obiekt.Obrazek)
            {
                Mouse.Capture(null);
                Mouse.OverrideCursor = Cursors.Arrow;

                Canvas.SetZIndex(obiekt.Obrazek, 1);

                if(obiekt is Postac)
                    obiekt.Obrazek.Source = new BitmapImage(new Uri((obiekt as Postac).obrazekFrontem, UriKind.Relative));

                Point mousePoint = Mouse.GetPosition(Application.Current.MainWindow);

                int x = (int)(mousePoint.Y / skala);
                int y = (int)(mousePoint.X / skala);

                if (y >= MainWindow.mainWindowObject.mapa.Rozmiar || x >= MainWindow.mainWindowObject.mapa.Rozmiar || y < 0 || x < 0 || MainWindow.mainWindowObject.mapa.Tablica[x, y] == Klocek.Sciana)
                {
                    UstawObiekt(obiekt);
                }
                else
                {
                    Canvas.SetTop(obiekt.Obrazek, x * (int)skala);
                    Canvas.SetLeft(obiekt.Obrazek, y * (int)skala + ((obiekt.Obrazek.Source.Height - obiekt.Obrazek.Source.Width) / 2) * skala / obiekt.Obrazek.Source.Height);
                    obiekt.UstawPunktNaMapie(new Punkt(x, y));
                }
                zlapanyObiekt = null;
            }
        }

        private void ZlapObiekt(IPrzemieczalne obiekt)
        {
            if (zlapanyObiekt == null)
            {
                Mouse.OverrideCursor = Cursors.Hand;

                Canvas.SetZIndex(obiekt.Obrazek, 2);
                obiekt.Obrazek.Height = obiekt.Obrazek.Source.Height * skala / obiekt.Obrazek.Source.Height;
                obiekt.Obrazek.Width = obiekt.Obrazek.Source.Width * skala / obiekt.Obrazek.Source.Height;
                if(obiekt is Postac)
                    obiekt.Obrazek.Source = new BitmapImage(new Uri((obiekt as Postac).obrazekZlapany, UriKind.Relative));

                Point mousePoint = Mouse.GetPosition(Application.Current.MainWindow);

                Canvas.SetTop(obiekt.Obrazek, mousePoint.Y - obiekt.Obrazek.Height / 2);
                Canvas.SetLeft(obiekt.Obrazek, mousePoint.X - obiekt.Obrazek.Width / 2);
                zlapanyObiekt = obiekt.Obrazek;
            }
        }

        private void UstawObiekt(IPrzemieczalne obiekt)
        {
            Canvas.SetTop(obiekt.Obrazek, obiekt.PozycjaStartowa.Y);
            Canvas.SetLeft(obiekt.Obrazek, obiekt.PozycjaStartowa.X);
            obiekt.Obrazek.Height = obiekt.Obrazek.Source.Height * 0.8;
            obiekt.Obrazek.Width = obiekt.Obrazek.Source.Width * 0.8;
            obiekt.UstawPunktNaMapie(Punkt.PozaMapa);
        }
        #endregion

        public void CzyDoszliDoCelu()
        {
            bool wszyscySkonczyli = true;
            foreach(Postac postac in postacie)
            {
                if (postac.ruchy != null)
                {
                    wszyscySkonczyli = false;
                    break;
                }                   
            }
            if (wszyscySkonczyli)
            {
                trybProgramu = TrybProgramu.Oczekuje;
                MessageBox.Show("Podróż zakończona");
            }
        }

        private void ZdarzeniePoruszania(Postac postac)
        {
            if (postac.ruchy.Count == 0)
            {
                postac.ruchy = null;
                postac.zdarzeniePoruszania.Stop();
                CzyDoszliDoCelu();
                postac.Obrazek.Source = new BitmapImage(new Uri(postac.obrazekFrontem, UriKind.Relative));
            }
            else
            {
                postac.Animuj();

                double x1 = postac.Obrazek.TransformToAncestor(canvasRozgrywki).Transform(new Point(0, 0)).X;
                double y1 = postac.Obrazek.TransformToAncestor(canvasRozgrywki).Transform(new Point(0, 0)).Y;
                double x2 = postac.ruchy[0].Y * skala + ((postac.Obrazek.Source.Height - postac.Obrazek.Source.Width) / 2) * skala / postac.Obrazek.Source.Height;
                double y2 = postac.ruchy[0].X * skala;

                if (Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2)) < 2)
                {
                    postac.UstawPunktNaMapie( new Punkt(postac.ruchy[0].X, postac.ruchy[0].Y));
                    postac.ruchy.RemoveAt(0);
                }
                else
                {
                    if (x1 < x2) Canvas.SetLeft(postac.Obrazek, x1 + 1);
                    else Canvas.SetLeft(postac.Obrazek, x1 - 1);
                    if (y1 < y2) Canvas.SetTop(postac.Obrazek, y1 + 1);
                    else Canvas.SetTop(postac.Obrazek, y1 - 1);
                }
            }
        }

        private void przyciskEdycjiMapy_Click(object sender, RoutedEventArgs e)
        {
            if(trybProgramu == TrybProgramu.Oczekuje)
            {
                MainWindow.mainWindowObject.OtworzOkno(MainWindow.mainWindowObject.stronaEdycjiMapy, this);
                UstawObiekt(flaga);
                foreach (Postac postac in postacie)
                    UstawObiekt(postac);
            }
        }

        private void przyciskStartuAplikacji_Click(object sender, RoutedEventArgs e)
        {
            if (flaga.CzyJestNaMapie && trybProgramu != TrybProgramu.WTrakcie)
            {
                trybProgramu = TrybProgramu.WTrakcie;

                WyszukiwanieDrogi wyszukiwanieDrogi = new WyszukiwanieDrogi(MainWindow.mainWindowObject.mapa);
                foreach(Postac postac in postacie)
                {                 
                    if (postac.CzyJestNaMapie)
                    {
                        postac.ruchy = wyszukiwanieDrogi.WyszukajTrase(postac.PozycjaNaMapie, flaga.PozycjaNaMapie);
                        if (postac.ruchy != null)                       
                            postac.zdarzeniePoruszania.Start();                       
                    }
                }
                CzyDoszliDoCelu();
            }
        }
    }
}
