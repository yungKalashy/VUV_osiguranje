using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace VUV_osiguranje.Osoba
{
    internal class SpremanjeOsiguranika
    {
        public static void Spremi(List<Osiguranik> osiguranici)
        {
            XDocument doc = new XDocument(
                new XElement("Osiguranici",
                    osiguranici.Select(o =>
                        new XElement("Osiguranik",
                            new XElement("OIB", o.Oib),
                            new XElement("Ime", o.Ime),
                            new XElement("Prezime", o.Prezime),
                            new XElement("Deleted", o.Deleted)
                        )
                    )
                )
            );

            doc.Save("Osiguranici.xml");
        }
    }
}
