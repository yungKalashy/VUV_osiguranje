using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace VUV_osiguranje.Osoba
{
    internal class SpremanjeOsiguranika
    {
        public static void Spremi(List<Osiguranik> osiguranici)
        {
            XmlDocument doc = new XmlDocument();

            XmlNode root = doc.CreateElement("Osiguranici");
            doc.AppendChild(root);

            foreach (Osiguranik o in osiguranici)
            {
                XmlNode osiguranikNode = doc.CreateElement("Osiguranik");

                XmlNode oibNode = doc.CreateElement("OIB");
                oibNode.InnerText = o.Oib;
                osiguranikNode.AppendChild(oibNode);

                XmlNode imeNode = doc.CreateElement("Ime");
                imeNode.InnerText = o.Ime;
                osiguranikNode.AppendChild(imeNode);

                XmlNode prezimeNode = doc.CreateElement("Prezime");
                prezimeNode.InnerText = o.Prezime;
                osiguranikNode.AppendChild(prezimeNode);

                XmlNode deletedNode = doc.CreateElement("Deleted");
                deletedNode.InnerText = o.Deleted.ToString();
                osiguranikNode.AppendChild(deletedNode);

                root.AppendChild(osiguranikNode);
            }

            doc.Save("Osiguranici.xml");
        }
    }
}
