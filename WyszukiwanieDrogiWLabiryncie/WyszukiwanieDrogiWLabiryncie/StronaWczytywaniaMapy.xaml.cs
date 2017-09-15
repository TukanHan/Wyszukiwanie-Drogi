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
    /// Interaction logic for StronaWczytywaniaMapy.xaml
    /// Strona odpowiedzialna za wczytywanie mapy z pliku
    /// </summary>
    public partial class StronaWczytywaniaMapy : UserControl
    {
        public StronaWczytywaniaMapy()
        {
            InitializeComponent();
        }

        private void przyciskCofnij_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindowObject.OtworzOkno(MainWindow.mainWindowObject.stronaGlowna, this);
        }

        private void przyciskWczytaj_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Plik zapisu (*.zapis)|*.zapis";
            openFileDialog.InitialDirectory = Environment.CurrentDirectory;
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    MainWindow.mainWindowObject.mapa = OdczytZapis.Odczyt(openFileDialog.FileName);
                    MainWindow.mainWindowObject.OtworzOkno(MainWindow.mainWindowObject.stronaEdycjiMapy, this);
                }
                catch
                {
                    MainWindow.mainWindowObject.OtworzOkno(MainWindow.mainWindowObject.stronaGlowna,this);
                }
            }
        }
    }
}
