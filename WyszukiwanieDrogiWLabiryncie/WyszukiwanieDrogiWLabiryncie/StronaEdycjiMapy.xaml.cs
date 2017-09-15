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
using Microsoft.Win32;

namespace WyszukiwanieDrogiWLabiryncie
{
    /// <summary>
    /// Interaction logic for StronaEdycjiMapy.xaml
    /// Strona odpowiedzialna za edycje mapy ( przeszkód ) oraz jej zapisu
    /// </summary>
    public partial class StronaEdycjiMapy : UserControl
    {
        Klocek aktualnyKlocek;

        public StronaEdycjiMapy()
        {
            InitializeComponent();
        }

        private void canvasEdycji_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if(canvasEdycji.Visibility==Visibility.Visible)
            {
                double skala = 400.0 / MainWindow.mainWindowObject.mapa.Tablica.GetLength(0);
                tlo.Width = MainWindow.mainWindowObject.mapa.Rozmiar * (int)skala;
                tlo.Height = MainWindow.mainWindowObject.mapa.Rozmiar * (int)skala;

                var obiektyDoUsuniecia = canvasEdycji.Children.OfType<Image>().Where((obrazek) => obrazek.Tag.Equals("Pole")).ToArray();
                foreach (Image img in obiektyDoUsuniecia)
                {
                    canvasEdycji.Children.Remove(img);
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

                        int x=i, y=j;
                        img.MouseDown += (s, args) => ZmianaKlocka(s, args, x, y);

                        Canvas.SetTop(img, i * (int)skala);
                        Canvas.SetLeft(img, j * (int)skala);

                        canvasEdycji.Children.Add(img);
                    }
                }
            }
        }

        private void ZmianaKlocka(object sender, MouseButtonEventArgs e, int x, int y)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Image pole = sender as Image;

                if (aktualnyKlocek == Klocek.Sciana)
                    pole.Source = new BitmapImage(new Uri("obrazki/o_sciana.png", UriKind.Relative));                               
                else             
                    pole.Source = new BitmapImage(new Uri("obrazki/o_podloga.png", UriKind.Relative));                 
                
                MainWindow.mainWindowObject.mapa.ZamienKlocek(aktualnyKlocek, x, y);
            }
        }

        private void przyciskUstawSciane_Click(object sender, RoutedEventArgs e)
        {
            aktualnyKlocek = Klocek.Sciana;
        }

        private void przyciskUstawPoglode_Click(object sender, RoutedEventArgs e)
        {
            aktualnyKlocek = Klocek.Podloga;
        }

        private void przyciskUruchomRozgrywke_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindowObject.OtworzOkno(MainWindow.mainWindowObject.stronaRozgrywki, this);
        }

        private void przyciskCofnij_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindowObject.OtworzOkno(MainWindow.mainWindowObject.stronaGlowna, this);
        }

        private void przyciskZapisz_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Plik zapisu (*.zapis)|*.zapis";
            saveFileDialog.InitialDirectory = Environment.CurrentDirectory;
            if (saveFileDialog.ShowDialog() == true)
            {
                OdczytZapis.Zapis(saveFileDialog.FileName);
            }
                
        }
    }
}
