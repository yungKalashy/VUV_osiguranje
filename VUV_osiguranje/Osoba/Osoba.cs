using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VUV_osiguranje.Osoba
{
    abstract class Osoba
    {
        private string _oib;
        private string _ime;
        private string _prezime;
        public Osoba() { }

        public Osoba(string oib, string ime, string prezime)
        {
            _oib = oib;
            _ime = ime;
            _prezime = prezime;
        }

        public string Oib
        {
            get { return _oib; }
        }

        public string Ime
        {
            get { return _ime; }
            set { _ime = value; }
        }

        public string Prezime
        {
            get { return _prezime; }
            set { _prezime = value; }
        }

    }
}
