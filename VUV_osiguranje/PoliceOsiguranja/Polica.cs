using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VUV_osiguranje.PoliceOsiguranja
{
    abstract class Polica : IObracunljivo
    {
        private int _sifra;
        private string _osiguranik;
        private DateTime _datumSklapanja;
        private DateTime _trajanje;
        private double _osiguranaSvota;

        public Polica(int sifra, string osiguranik, DateTime datumSklapanja, DateTime trajanje, double osiguranaSvota)
        {
            _sifra = sifra;
            _osiguranik = osiguranik;
            _datumSklapanja = datumSklapanja;
            _trajanje = trajanje;
            _osiguranaSvota = osiguranaSvota;
        }


        public int Sifra
        {
            get { return _sifra; }
        }

        public string Osiguranik
        {
            get { return _osiguranik; }
            set { _osiguranik = value; }
        }

        public DateTime datumSklapanja
        {
            get { return _datumSklapanja; }
            set { _datumSklapanja = value; }
        }

        public DateTime Trajanje
        {
            get { return _trajanje; }
            set { _trajanje = value; }
        }

        public double OsiguranaSvota
        {
            get { return _osiguranaSvota; }
            set { _osiguranaSvota = value; }
        }


        public abstract double izracunajGodisnjuPremiju();
    }

}