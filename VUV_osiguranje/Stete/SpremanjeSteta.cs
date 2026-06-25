using System.Collections.Generic;
using System.Xml;


namespace VUV_osiguranje.Stete
{
    internal class SpremanjeSteta
    {
        public static void Spremi(List<Steta> stete)
        {
            XmlDocument doc = new XmlDocument();

            XmlElement root = doc.CreateElement("Stete");
            doc.AppendChild(root);

            foreach (Steta s in stete)
            {
                XmlElement steta = doc.CreateElement("Steta");

                XmlElement datum = doc.CreateElement("Datum");
                datum.InnerText = s.datum.ToString();

                XmlElement opis = doc.CreateElement("Opis");
                opis.InnerText = s.opis;

                XmlElement iznos = doc.CreateElement("Iznos");
                iznos.InnerText = s.iznos.ToString();

                XmlElement status = doc.CreateElement("Status");
                status.InnerText = s.status.ToString();

                XmlElement sifraPolice = doc.CreateElement("SifraPolice");
                sifraPolice.InnerText = s.polica.Sifra.ToString();

                steta.AppendChild(datum);
                steta.AppendChild(opis);
                steta.AppendChild(iznos);
                steta.AppendChild(status);
                steta.AppendChild(sifraPolice);

                root.AppendChild(steta);
            }

            doc.Save("Stete.xml");
        }
    }
}