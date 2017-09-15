using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WyszukiwanieDrogiWLabiryncie
{
    interface IPrzemieczalne
    {
        Image Obrazek { get; }
        Punkt PozycjaStartowa { get; }
        void UstawPunktNaMapie(Punkt punkt);
    }
}
