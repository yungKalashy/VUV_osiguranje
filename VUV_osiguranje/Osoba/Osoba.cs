using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VUV_osiguranje.Osoba
{
    abstract class Osoba
    {
        private int _oib;
        private string _ime;
        private string _prezime;

        public Osoba(int oib, string ime, string prezime)
        {
            _oib = oib;
            _ime = ime;
            _prezime = prezime;
        }
    }
}
