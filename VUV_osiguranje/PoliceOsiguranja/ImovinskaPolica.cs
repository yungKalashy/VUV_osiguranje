using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VUV_osiguranje.PoliceOsiguranja
{
    internal class ImovinskaPolica : Polica
    {

        public ImovinskaPolica(int sifra, string osiguranik, DateTime datumSklapanja, DateTime trajanje, double osiguranaSvota) : base(sifra, osiguranik, datumSklapanja, trajanje, osiguranaSvota)
        {

        }

        public override double izracunajGodisnjuPremiju()
        {
            double izracun = 0;

            if(OsiguranaSvota > 0)
            {
               izracun = OsiguranaSvota * 0.015;
            }

            return izracun;
            
        }
    }
}
