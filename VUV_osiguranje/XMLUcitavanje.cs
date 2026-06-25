using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using VUV_osiguranje.Osoba;
using VUV_osiguranje.PoliceOsiguranja;
using VUV_osiguranje.Stete;

namespace VUV_osiguranje
{
    internal class XMLUcitavanje
    {
        //Osiguranici
        public static List<Osiguranik> UcitajOsiguranike()
        {
            List<Osiguranik> osiguranici = new List<Osiguranik>();

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("Osiguranici.xml");

                XmlNodeList lista = doc.SelectNodes("/Osiguranici/Osiguranik");

                foreach (XmlNode node in lista)
                {
                    string oib = node["OIB"].InnerText;
                    string ime = node["Ime"].InnerText;
                    string prezime = node["Prezime"].InnerText;

                    bool deleted = node["Deleted"] != null
                        ? bool.Parse(node["Deleted"].InnerText)
                        : false;

                    osiguranici.Add(new Osiguranik(oib, ime, prezime, deleted));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Greška pri učitavanju Osiguranici.xml: " + ex.Message);
            }

            return osiguranici;
        }


        //Police
        public static Dictionary<int, Polica> UcitajPolice()
        {
            Dictionary<int, Polica> police = new Dictionary<int, Polica>();

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("Police.xml");

                XmlNodeList lista = doc.SelectNodes("/Police/Polica");

                foreach (XmlNode node in lista)
                {
                    int sifra = int.Parse(node["Sifra"].InnerText);

                    string osiguranik = node["Osiguranik"].InnerText;

                    DateTime datumSklapanja = DateTime.Parse(node["DatumSklapanja"].InnerText);
                    DateTime datumTrajanja = DateTime.Parse(node["DatumTrajanja"].InnerText);
                    double svota = double.Parse(node["OsiguranaSvota"].InnerText);

                    string vrsta = node["VrstaPolice"].InnerText;

                    Polica polica;

                    switch (vrsta)
                    {
                        case "AutoPolica":
                            polica = new AutoPolica(sifra, osiguranik, datumSklapanja, datumTrajanja, svota);
                            break;

                        case "ZivotnaPolica":
                            polica = new ZivotnaPolica(sifra, osiguranik, datumSklapanja, datumTrajanja, svota);
                            break;

                        case "ImovinskaPolica":
                            polica = new ImovinskaPolica(sifra, osiguranik, datumSklapanja, datumTrajanja, svota);
                            break;

                        default:
                            throw new Exception("Nepoznata vrsta police: " + vrsta);
                    }

                    police.Add(sifra, polica);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Greška pri učitavanju Police.xml: " + ex.Message);
            }

            return police;
        }


        //Stete
        public static List<Steta> UcitajStete(Dictionary<int, Polica> police)
        {
            List<Steta> stete = new List<Steta>();

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load("Stete.xml");

                XmlNodeList lista = doc.SelectNodes("/Stete/Steta");

                foreach (XmlNode node in lista)
                {
                    if (node["SifraPolice"] == null)
                        continue;

                    int sifraPolice = int.Parse(node["SifraPolice"].InnerText);

                    if (!police.TryGetValue(sifraPolice, out Polica polica))
                        continue;

                    Steta steta = new Steta(
                        polica,
                        sifraPolice,
                        DateTime.Parse(node["Datum"].InnerText),
                        node["Opis"].InnerText,
                        double.Parse(node["Iznos"].InnerText),
                        (StatusStete)Enum.Parse(typeof(StatusStete), node["Status"].InnerText)
                    );

                    stete.Add(steta);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Greška u Stete.xml: " + ex.Message);
            }

            return stete;
        }
    }
}
