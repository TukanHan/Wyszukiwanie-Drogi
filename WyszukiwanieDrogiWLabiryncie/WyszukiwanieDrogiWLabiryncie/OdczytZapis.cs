using System;
using System.IO;
using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WyszukiwanieDrogiWLabiryncie
{
    /// <summary>
    /// Prosta statyczna klasa zwiazana z odczytem / zapisem danych
    /// </summary>
    static class OdczytZapis
    {
        static public Mapa Odczyt(string scierzka)
        {
            Klocek[,] tablica;
            try
            {
                using (StreamReader sr = new StreamReader(scierzka))
                {
                    uint rozmiar = uint.Parse(sr.ReadLine());
                    if (rozmiar < 4 || rozmiar > 10)
                        throw new Exception("Nieprawidłowe dane");

                    tablica = new Klocek[rozmiar, rozmiar];
                    for (int i = 0; i < rozmiar; ++i)
                        for (int j = 0; j < rozmiar; ++j)
                            tablica[i, j] = (Klocek)Enum.Parse(typeof(Klocek), sr.ReadLine());
                }
                return new Mapa(tablica);

            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
                throw;
            }
        }

        static public void Zapis(string scierzka)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(scierzka))
                {
                    sw.WriteLine(MainWindow.mainWindowObject.mapa.Rozmiar);

                    for (int i = 0; i < MainWindow.mainWindowObject.mapa.Rozmiar; ++i)
                        for (int j = 0; j < MainWindow.mainWindowObject.mapa.Rozmiar; ++j)
                            sw.WriteLine(MainWindow.mainWindowObject.mapa.tablica[i, j]);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
