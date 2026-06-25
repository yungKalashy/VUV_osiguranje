using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VUV_osiguranje.PoliceOsiguranja;
using VUV_osiguranje.Stete;

namespace VUV_osiguranje
{
    internal class Statistika
    {
        //Premije po vsrti
        public static void PremijePoVrsti(Dictionary<int, Polica> police)
        {
            double auto = 0;
            double zivotna = 0;
            double imovinska = 0;

            foreach (Polica p in police.Values)
            {
                if (p is AutoPolica)
                    auto += p.izracunajGodisnjuPremiju();

                else if (p is ZivotnaPolica)
                    zivotna += p.izracunajGodisnjuPremiju();

                else if (p is ImovinskaPolica)
                    imovinska += p.izracunajGodisnjuPremiju();
            }

            List<(string vrsta, double iznos)> lista = new List<(string, double)>();

            lista.Add(("Auto", auto));
            lista.Add(("Životna", zivotna));
            lista.Add(("Imovinska", imovinska));

            lista.Sort((a, b) => b.iznos.CompareTo(a.iznos));

            Console.WriteLine("\n*** UKUPNA NAPLAĆENA PREMIJA ***");

            foreach (var item in lista)
            {
                Console.WriteLine($"{item.vrsta}: {item.iznos:F2} EUR");
            }
        }

        //Top 5
        public static void Top5Osiguranika(List<Steta> stete)
        {
            Dictionary<string, double> ukupno = new Dictionary<string, double>();

            foreach (Steta s in stete)
            {
                if (s.status == StatusStete.Odobrena)
                {
                    string osiguranik = s.polica.Osiguranik;

                    if (!ukupno.ContainsKey(osiguranik))
                        ukupno.Add(osiguranik, 0);

                    ukupno[osiguranik] += s.iznos;
                }
            }

            // u listu
            List<KeyValuePair<string, double>> lista =
                new List<KeyValuePair<string, double>>();

            foreach (KeyValuePair<string, double> item in ukupno)
            {
                lista.Add(item);
            }

            // sort
            lista.Sort(delegate (KeyValuePair<string, double> a,
                                 KeyValuePair<string, double> b)
            {
                return b.Value.CompareTo(a.Value);
            });

            Console.WriteLine("\n*** TOP 5 OSIGURANIKA ***");

            int brojac = 0;

            foreach (KeyValuePair<string, double> item in lista)
            {
                Console.WriteLine($"{brojac + 1}. {item.Key} - {item.Value:F2} EUR");

                brojac++;

                if (brojac == 5)
                    break;
            }
        }

        //Udio odobrenih

        public static void UdioOdobrenihSteta(List<Steta> stete)
        {
            string[] vrste ={"AutoPolica","ZivotnaPolica","ImovinskaPolica"};

            Console.WriteLine("\n*** UDIO ODOBRENIH ŠTETA ***");

            foreach (string vrsta in vrste)
            {
                int ukupno = 0;
                int odobrene = 0;

                foreach (Steta s in stete)
                {
                    if (s.polica.GetType().Name == vrsta)
                    {
                        ukupno++;

                        if (s.status == StatusStete.Odobrena)
                            odobrene++;
                    }
                }

                double postotak = 0;

                if (ukupno > 0)
                    postotak = (double)odobrene / ukupno * 100;

                Console.WriteLine($"{vrsta}: {postotak:F2}%");
            }
        }
    }
}
