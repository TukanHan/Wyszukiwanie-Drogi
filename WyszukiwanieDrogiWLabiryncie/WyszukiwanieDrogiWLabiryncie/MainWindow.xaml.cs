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
    /// Interaction logic for MainWindow.xaml
    /// Rdzeń programu
    /// </summary>
    public partial class MainWindow : Window
    {
        //statyczna zmienna do odwoływania się do tego obiektu z innych klas
        public static MainWindow mainWindowObject;

        public Mapa mapa;


        public MainWindow()
        {
            InitializeComponent();
            mainWindowObject = this;
        }

        #region Przejścia

        public void OtworzOknoRozmiaru(UserControl obecneOkno)
        {
            stronaWyboruRozmiaruMapy.Visibility = Visibility.Visible;
            obecneOkno.Visibility = Visibility.Hidden;
        }

        public void OtworzOknoWczytywania(UserControl obecneOkno)
        {
            stronaWczytwaniaMapy.Visibility = Visibility.Visible;
            obecneOkno.Visibility = Visibility.Hidden;
        }

        public void OtworzOknoEdycji(UserControl obecneOkno)
        {
            stronaEdycjiMapy.Visibility = Visibility.Visible;
            obecneOkno.Visibility = Visibility.Hidden;
        }

        public void OtworzOknoRozgrywki(UserControl obecneOkno)
        {
            stronaRozgrywki.Visibility = Visibility.Visible;
            obecneOkno.Visibility = Visibility.Hidden;
        }

        public void OtworzStroneGlowna(UserControl obecneOkno)
        {
            stronaGlowna.Visibility = Visibility.Visible;
            obecneOkno.Visibility = Visibility.Hidden;
        }

        public void OtworzStroneInformacyjna(UserControl obecneOkno)
        {
            stronaInformacyjna.Visibility = Visibility.Visible;
            obecneOkno.Visibility = Visibility.Hidden;
        }

        #endregion
    }
}
