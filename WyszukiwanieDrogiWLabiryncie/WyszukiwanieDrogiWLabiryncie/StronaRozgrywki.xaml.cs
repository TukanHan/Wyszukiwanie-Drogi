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
    /// Interaction logic for StronaRozgrywki.xaml
    /// Strona odpowiedzialna za włąściwą rozgrywkę
    /// Przesówanie postaci i flagi oraz przemieszczanie się po planszy
    /// </summary>
    public partial class StronaRozgrywki : UserControl
    {
        enum TrybProgramu { Oczekuje, WTrakcie};

        Postac[] postacie;
        PozycjaNaMapie cel = new PozycjaNaMapie();
        Object zlapanyObiekt = null;
        TrybProgramu trybProgramu;

        public StronaRozgrywki()
        {
            InitializeComponent();

            postacie = new Postac[]
            {
                 new Postac(Postac0 ,64,475,"o_postacie/p1_front.png", "o_postacie/p1_jump.png", "o_postacie/p1_walk01.png", "o_postacie/p1_walk02.png",
                "o_postacie/p1_walk03.png", "o_postacie/p1_walk04.png", "o_postacie/p1_walk05.png", "o_postacie/p1_walk06.png", "o_postacie/p1_walk07.png",
                "o_postacie/p1_walk08.png", "o_postacie/p1_walk09.png", "o_postacie/p1_walk10.png", "o_postacie/p1_walk11.png"),
                 new Postac(Postac1, 64, 418, "o_postacie/p2_front.png", "o_postacie/p2_jump.png", "o_postacie/p2_walk01.png", "o_postacie/p2_walk02.png",
                "o_postacie/p2_walk03.png", "o_postacie/p2_walk04.png", "o_postacie/p2_walk05.png", "o_postacie/p2_walk06.png", "o_postacie/p2_walk07.png",
                "o_postacie/p2_walk08.png", "o_postacie/p2_walk09.png", "o_postacie/p2_walk10.png", "o_postacie/p2_walk11.png"),
                 new Postac(Postac2, 64, 533, "o_postacie/p3_front.png", "o_postacie/p3_jump.png", "o_postacie/p3_walk01.png", "o_postacie/p3_walk02.png",
                "o_postacie/p3_walk03.png", "o_postacie/p3_walk04.png", "o_postacie/p3_walk05.png", "o_postacie/p3_walk06.png", "o_postacie/p3_walk07.png",
                "o_postacie/p3_walk08.png", "o_postacie/p3_walk09.png", "o_postacie/p3_walk10.png", "o_postacie/p3_walk11.png")
            };

            for (int i = 0; i < postacie.Length; ++i)
            {
                int a = i;
                postacie[i].zdarzeniePoruszania = new DispatcherTimer();
                postacie[i].zdarzeniePoruszania.Tick += (s, args) => ZdarzeniePoruszania(postacie[a]);
                postacie[i].zdarzeniePoruszania.Interval = new TimeSpan(0, 0, 0, 0, 15);
                postacie[i].obrazekPostaci.MouseUp += (s, args) => Postac_MouseUp(s, args, postacie[a]);
                postacie[i].obrazekPostaci.MouseDown += (s, args) => Postac_MouseDown(s, args, postacie[a]);
            }
        }

        private void canvasRozgrywki_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (canvasRozgrywki.Visibility == Visibility.Visible)
            {
                double skala = 400.0 / MainWindow.mainWindowObject.mapa.Rozmiar;
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

                        if (MainWindow.mainWindowObject.mapa.tablica[i, j] == Klocek.Sciana)
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

        #region Przeciąganie postaci

        private void Postac_MouseMove(object sender, MouseEventArgs e)
        {
            if (zlapanyObiekt == sender )           
                PrzesowanieObiektu(sender, e);                
        }

        private void Postac_MouseDown(object sender, MouseButtonEventArgs e, Postac postac)
        {
            if (zlapanyObiekt == null)
            {
                double skala = 400.0 / MainWindow.mainWindowObject.mapa.Rozmiar;

                Mouse.OverrideCursor = Cursors.Hand;
                Image i_m_a_g_e = sender as Image;

                Canvas.SetZIndex(i_m_a_g_e, 2);
                i_m_a_g_e.Height = i_m_a_g_e.Source.Height * skala / i_m_a_g_e.Source.Height;
                i_m_a_g_e.Width = i_m_a_g_e.Source.Width * skala / i_m_a_g_e.Source.Height;
                i_m_a_g_e.Source = new BitmapImage(new Uri(postac.obrazekZlapany, UriKind.Relative));

                Point mousePoint = e.GetPosition(this);

                Canvas.SetTop(i_m_a_g_e, mousePoint.Y - i_m_a_g_e.Height / 2);
                Canvas.SetLeft(i_m_a_g_e, mousePoint.X - i_m_a_g_e.Width / 2);
                zlapanyObiekt = sender;
            }
        }

        private void Postac_MouseUp(object sender, MouseButtonEventArgs e, Postac postac)
        {
            if (zlapanyObiekt == sender)
            {
                double skala = 400.0 / MainWindow.mainWindowObject.mapa.Rozmiar;

                Mouse.Capture(null);
                Mouse.OverrideCursor = Cursors.Arrow;

                Canvas.SetZIndex(postac.obrazekPostaci, 1);
                postac.obrazekPostaci.Source = new BitmapImage(new Uri(postac.obrazekFrontem, UriKind.Relative));

                Point mousePoint = e.GetPosition(this);
                int x = (int)(mousePoint.Y / skala);
                int y = (int)(mousePoint.X / skala);

                if (y >= MainWindow.mainWindowObject.mapa.Rozmiar || x >= MainWindow.mainWindowObject.mapa.Rozmiar || y < 0 || x < 0 || MainWindow.mainWindowObject.mapa.tablica[x, y] == Klocek.Sciana)
                {
                    UstawPostacie(postac);
                }
                else
                {
                    Canvas.SetTop(postac.obrazekPostaci, x * (int)skala);
                    Canvas.SetLeft(postac.obrazekPostaci, y * (int)skala + ((postac.obrazekPostaci.Source.Height - postac.obrazekPostaci.Source.Width) / 2) * skala / postac.obrazekPostaci.Source.Height);
                    postac.pozycja.Ustaw(x, y);
                }

                zlapanyObiekt = null;
            }
        }

        #endregion

        #region Przeciąganie flagi

        private void Flaga_MouseMove(object sender, MouseEventArgs e)
        {
            if (zlapanyObiekt == sender)
                PrzesowanieObiektu(sender, e);
        }

        private void Flaga_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (zlapanyObiekt == null)
            {
                double skala = 400.0 / MainWindow.mainWindowObject.mapa.Rozmiar;

                Mouse.OverrideCursor = Cursors.Hand;
                Image i_m_a_g_e = sender as Image;
                Canvas.SetZIndex(i_m_a_g_e, 2);
                i_m_a_g_e.Height = 72 * skala / 72;
                i_m_a_g_e.Width = 64 * skala / 72;

                Point mousePoint = e.GetPosition(this);

                Canvas.SetTop(i_m_a_g_e, mousePoint.Y - i_m_a_g_e.Height / 2);
                Canvas.SetLeft(i_m_a_g_e, mousePoint.X - i_m_a_g_e.Width / 2);
                zlapanyObiekt = sender;
            }
        }

        private void Flaga_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (zlapanyObiekt == sender)
            {
                double skala = 400.0 / MainWindow.mainWindowObject.mapa.Rozmiar;
                Mouse.Capture(null);
                Mouse.OverrideCursor = Cursors.Arrow;
                Image i_m_a_g_e = sender as Image;
                Canvas.SetZIndex(i_m_a_g_e, 1);

                Point mousePoint = e.GetPosition(this);
                int x = (int)(mousePoint.Y / skala);
                int y = (int)(mousePoint.X / skala);

                if (y >= MainWindow.mainWindowObject.mapa.Rozmiar || x >= MainWindow.mainWindowObject.mapa.Rozmiar || y < 0 || x < 0 || MainWindow.mainWindowObject.mapa.tablica[x, y] == Klocek.Sciana)
                {
                    UstawFlage();
                }
                else
                {
                    Canvas.SetTop(i_m_a_g_e, x * (int)skala);
                    Canvas.SetLeft(i_m_a_g_e, y * (int)skala + 5 * skala / 92);
                    cel.Ustaw(x, y);
                }
                zlapanyObiekt = null;
            }
        }

        #endregion

        private void PrzesowanieObiektu(object sender, MouseEventArgs e)
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

        private void UstawPostacie(Postac postac)
        {
            Canvas.SetTop(postac.obrazekPostaci, postac.obszarNaMapieX);
            Canvas.SetLeft(postac.obrazekPostaci, postac.obszarNaMapieY);
            postac.obrazekPostaci.Height = postac.obrazekPostaci.Source.Height * 0.8;
            postac.obrazekPostaci.Width = postac.obrazekPostaci.Source.Width * 0.8;
            postac.pozycja = new PozycjaNaMapie();
        }

        private void UstawFlage()
        {
            Canvas.SetTop(flaga, 194);
            Canvas.SetLeft(flaga, 474);
            flaga.Height = 74;
            flaga.Width = 64;
            cel = new PozycjaNaMapie();
        }

        public void CzyDoszliDoCelu()
        {
            bool wszyscySkonczyli = true;
            for (int i = 0; i < postacie.Length && wszyscySkonczyli; ++i)
            {
                if (postacie[i].wDrodze)
                    wszyscySkonczyli = false;
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
                postac.wDrodze = false;
                postac.zdarzeniePoruszania.Stop();
                CzyDoszliDoCelu();
                postac.obrazekPostaci.Source = new BitmapImage(new Uri(postac.obrazekFrontem, UriKind.Relative));
            }
            else
            {
                double skala = 400.0 / MainWindow.mainWindowObject.mapa.tablica.GetLength(0);

                postac.Animuj();

                double x1 = postac.obrazekPostaci.TransformToAncestor(canvasRozgrywki).Transform(new Point(0, 0)).X;
                double y1 = postac.obrazekPostaci.TransformToAncestor(canvasRozgrywki).Transform(new Point(0, 0)).Y;
                double x2 = postac.ruchy[0].y * skala + ((postac.obrazekPostaci.Source.Height - postac.obrazekPostaci.Source.Width) / 2) * skala / postac.obrazekPostaci.Source.Height;
                double y2 = postac.ruchy[0].x * skala;


                if (Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2)) < 2)
                {
                    postac.pozycja.Ustaw(postac.ruchy[0].x, postac.ruchy[0].y);
                    postac.ruchy.RemoveAt(0);
                }
                else
                {
                    if (x1 < x2) Canvas.SetLeft(postac.obrazekPostaci, x1 + 1);
                    else Canvas.SetLeft(postac.obrazekPostaci, x1 - 1);
                    if (y1 < y2) Canvas.SetTop(postac.obrazekPostaci, y1 + 1);
                    else Canvas.SetTop(postac.obrazekPostaci, y1 - 1);
                }
            }
        }

        private void przyciskEdycjiMapy_Click(object sender, RoutedEventArgs e)
        {
            if(trybProgramu == TrybProgramu.Oczekuje)
            {
                MainWindow.mainWindowObject.OtworzOknoEdycji(this);
                UstawFlage();
                foreach (Postac postac in postacie)
                    UstawPostacie(postac);
            }           
        }

        private void przyciskStartuAplikacji_Click(object sender, RoutedEventArgs e)
        {
            if (cel.na_mapie && trybProgramu != TrybProgramu.WTrakcie)
            {
                trybProgramu = TrybProgramu.WTrakcie;

                for (int i = 0; i < postacie.Length; ++i)
                {
                    if (postacie[i].pozycja.na_mapie)
                    {
                        if (postacie[i].wDrodze = postacie[i].WyszukiwanieDrogi(MainWindow.mainWindowObject.mapa, cel))
                        {
                            postacie[i].zdarzeniePoruszania.Start();
                        }

                    }
                }
                CzyDoszliDoCelu();
            }
        }
    }
}
