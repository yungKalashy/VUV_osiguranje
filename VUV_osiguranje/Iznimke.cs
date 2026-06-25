using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace VUV_osiguranje
{
    internal class Iznimke : Exception
    {
        public string greska { get; }

        public Iznimke(string Greska, string message) : base ("" + Greska + "   " + message)
        {
            greska = Greska;
        }
    }
}
