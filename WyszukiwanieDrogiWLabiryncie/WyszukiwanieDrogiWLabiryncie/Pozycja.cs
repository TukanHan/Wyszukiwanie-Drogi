using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WyszukiwanieDrogiWLabiryncie
{
    class Pozycja
    {
        public int x;
        public int y;

        public Pozycja(int x, int y) { this.x = x; this.y = y;  }
        public Pozycja() { this.y = -1; this.x = -1;  }
        public Pozycja(Pozycja P) { this.y = P.y; this.x = P.x; }
        public Pozycja(PozycjaNaMapie P) { this.y = P.y; this.x = P.x; }
        public virtual void Ustaw(int x, int y) { this.x = x; this.y = y; }

        public override bool Equals(object obj)
        {
            Pozycja P = obj as Pozycja;
            if (x == P.x && y == P.y)
                return true;
            return false;
        }

        static public double LiczDystans(Pozycja P1, Pozycja P2)
        {
            return Math.Sqrt(Math.Pow(P1.x - P2.x, 2) + Math.Pow(P1.y - P2.y, 2));
        }
    }

    class PozycjaNaMapie : Pozycja
    {
        public bool na_mapie;

        public PozycjaNaMapie(int x, int y) : base(x,y) { na_mapie = ((y == -1 || x == -1) ? false : true); }
        public PozycjaNaMapie() : base() { na_mapie = false; }
        public PozycjaNaMapie(Pozycja P) : base(P.x, P.y) { na_mapie = false; }
        public PozycjaNaMapie(PozycjaNaMapie P) { y = P.y; x = P.x; na_mapie = P.na_mapie; }
        public override void Ustaw(int x, int y) { this.x = x; this.y = y; na_mapie = ((y == -1 || x == -1) ? false : true); }
    }
}
