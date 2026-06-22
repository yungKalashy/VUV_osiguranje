using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VUV_osiguranje.Osoba
{
    internal class Osiguranik : Osoba
    {

        private bool _deleted;
        public Osiguranik() : base() { }
        public Osiguranik(string oib, string ime, string prezime, bool deleted) : base(oib, ime, prezime)
        {
            _deleted = deleted;
        }

        public bool Deleted
        {
            get { return _deleted; }
            set { _deleted = value; }
        }
    }
}
