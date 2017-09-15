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
    /// Rdzeń programu, przechodzenie między okienkami
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

        public void OtworzOkno(UserControl otwierane, UserControl zamykane)
        {
            otwierane.Visibility = Visibility.Visible;
            zamykane.Visibility = Visibility.Hidden;
        }
    }
}
