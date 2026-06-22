using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace VUV_osiguranje.Stete
{
    internal class SpremanjeSteta
    {
        public static void Spremi(List<Steta> stete)
        {
            var doc = new XDocument(
                new XElement("Stete",
                    stete.Select(s =>
                        new XElement("Steta",
                            new XElement("Datum", s.datum),
                            new XElement("Opis", s.opis),
                            new XElement("Iznos", s.iznos),
                            new XElement("Status", s.status)
                        )
                    )
                )
            );

            doc.Save("Stete.xml");
        }
    }
}