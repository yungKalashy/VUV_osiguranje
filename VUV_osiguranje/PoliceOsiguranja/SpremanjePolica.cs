using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace VUV_osiguranje.PoliceOsiguranja
{
    internal class SpremanjePolica
    {
        public static void Spremi(Dictionary<int, Polica> police)
        {
            XmlDocument doc = new XmlDocument();

            XmlElement root = doc.CreateElement("Police");
            doc.AppendChild(root);

            foreach (Polica p in police.Values)
            {
                XmlElement polica = doc.CreateElement("Polica");

                XmlElement sifra = doc.CreateElement("Sifra");
                sifra.InnerText = p.Sifra.ToString();

                XmlElement osiguranik = doc.CreateElement("Osiguranik");
                osiguranik.InnerText = p.Osiguranik;

                XmlElement vrsta = doc.CreateElement("VrstaPolice");
                vrsta.InnerText = p.GetType().Name;

                XmlElement datumSklapanja = doc.CreateElement("DatumSklapanja");
                datumSklapanja.InnerText = p.datumSklapanja.ToString("yyyy-MM-dd");

                XmlElement datumTrajanja = doc.CreateElement("DatumTrajanja");
                datumTrajanja.InnerText = p.Trajanje.ToString("yyyy-MM-dd");

                XmlElement svota = doc.CreateElement("OsiguranaSvota");
                svota.InnerText = p.OsiguranaSvota.ToString();

                polica.AppendChild(sifra);
                polica.AppendChild(osiguranik);
                polica.AppendChild(vrsta);
                polica.AppendChild(datumSklapanja);
                polica.AppendChild(datumTrajanja);
                polica.AppendChild(svota);

                root.AppendChild(polica);
            }

            doc.Save("Police.xml");
        }
    }
}
