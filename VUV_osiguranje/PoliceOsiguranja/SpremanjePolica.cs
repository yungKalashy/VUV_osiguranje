using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace VUV_osiguranje.PoliceOsiguranja
{
    internal class SpremanjePolica
    {
        public static void Spremi(Dictionary<int, Polica> police)
        {
            var doc = new XDocument(
                new XElement("Police",
                    police.Values.Select(p =>
                        new XElement("Polica",
                            new XElement("Sifra", p.Sifra),
                            new XElement("Osiguranik", p.Osiguranik),
                            new XElement("VrstaPolice", p.GetType().Name),
                            new XElement("DatumSklapanja", p.datumSklapanja),
                            new XElement("DatumTrajanja", p.Trajanje),
                            new XElement("OsiguranaSvota", p.OsiguranaSvota)
                        )
                    )
                )
            );

            doc.Save("Police.xml");
        }
    }
}
