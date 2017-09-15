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

namespace WyszukiwanieDrogiWLabiryncie
{
    /// <summary>
    /// Interaction logic for StronaWyboruRozmiaruMapy.xaml
    /// Strona odpowiedzialna wyłącznie za wybór rozmiaru mapy
    /// </summary>
    public partial class StronaWyboruRozmiaruMapy : UserControl
    {
        public StronaWyboruRozmiaruMapy()
        {
            InitializeComponent();
        }

        private void przyciskUtworzMape_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindowObject.mapa = new Mapa((uint)pasekRozmiaruMapy.Value);
            MainWindow.mainWindowObject.OtworzOkno(MainWindow.mainWindowObject.stronaEdycjiMapy, this);
        }

        private void przyciskCofnij_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindowObject.OtworzOkno(MainWindow.mainWindowObject.stronaGlowna, this);
        }
    }
}
