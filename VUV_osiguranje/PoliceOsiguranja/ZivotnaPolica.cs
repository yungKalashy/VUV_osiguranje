using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VUV_osiguranje.PoliceOsiguranja
{
    internal class ZivotnaPolica : Polica
    {

        public ZivotnaPolica(int sifra, string osiguranik, DateTime datumSklapanja, DateTime trajanje, double osiguranaSvota) : base(sifra, osiguranik, datumSklapanja, trajanje, osiguranaSvota)
        {

        }

        public override double izracunajGodisnjuPremiju()
        {
            double izracun = 0;

            if (OsiguranaSvota > 0)
            {
                izracun = OsiguranaSvota * 0.03;
            }

            return izracun;
        }
    }
}
