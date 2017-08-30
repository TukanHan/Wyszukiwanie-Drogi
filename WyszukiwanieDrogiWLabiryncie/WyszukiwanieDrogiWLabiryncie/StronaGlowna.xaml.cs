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
    /// Interaction logic for StronaGlowna.xaml
    /// </summary>
    public partial class StronaGlowna : UserControl
    {
        
        public StronaGlowna()
        {
            InitializeComponent();
        }

        private void przyciskNowaMapa_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindowObject.OtworzOknoRozmiaru(this);
        }

        private void przyciskWczytajMape_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindowObject.OtworzOknoWczytywania(this);
        }

        private void przyciskLosujMape_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindowObject.mapa = Mapa.LosujMape();
            MainWindow.mainWindowObject.OtworzOknoEdycji(this);
        }

        private void przyciskOProgramie_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindowObject.OtworzStroneInformacyjna(this);
        }
    }
}
