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
    /// Interaction logic for StronaInformacyjna.xaml
    /// </summary>
    public partial class StronaInformacyjna : UserControl
    {
        public StronaInformacyjna()
        {
            InitializeComponent();
        }

        private void Hiperlacze(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start(@"http://" + ((Button)sender).Tag);
        }

        private void przyciskCofnij_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.mainWindowObject.OtworzOkno(MainWindow.mainWindowObject.stronaGlowna,this);
        }
    }
}
