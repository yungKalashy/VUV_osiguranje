using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VUV_osiguranje.Osoba;

namespace VUV_osiguranje.PoliceOsiguranja
{
    abstract class Polica : IObracunljivo
    {
        private int _sifra;
        private string _osiguranik;
        private DateTime _datumSklapanja;
        private DateTime _trajanje;
        private double _osiguranaSvota;

        public Polica(int sifra, string osiguranik, DateTime datumSklapanja, DateTime trajanje, double osiguranaSvota)
        {
            _sifra = sifra;
            _osiguranik = osiguranik;
            _datumSklapanja = datumSklapanja;
            _trajanje = trajanje;
            _osiguranaSvota = osiguranaSvota;
        }


        public int Sifra
        {
            get { return _sifra; }
        }

        public string Osiguranik
        {
            get { return _osiguranik; }
            set { _osiguranik = value; }
        }

        public DateTime datumSklapanja
        {
            get { return _datumSklapanja; }
            set { _datumSklapanja = value; }
        }

        public DateTime Trajanje
        {
            get { return _trajanje; }
            set { _trajanje = value; }
        }

        public double OsiguranaSvota
        {
            get { return _osiguranaSvota; }
            set { _osiguranaSvota = value; }
        }


        public abstract double izracunajGodisnjuPremiju();

        public static void SklapanjePolice(List<Osiguranik> osiguranici, Dictionary<int, Polica> police)
        {
            string nastavakPolica = "";

            do
            {
                try
                {
                    List<Osiguranik> aktivni = new List<Osiguranik>();

                    foreach (Osiguranik o in osiguranici)
                    {
                        if (!o.Deleted)
                            aktivni.Add(o);
                    }

                    if (aktivni.Count == 0)
                        throw new Iznimke("Sklapanje police", "Nema aktivnih osiguranika.");

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\n*** AKTIVNI OSIGURANICI ***");
                    Console.ResetColor();

                    for (int i = 0; i < aktivni.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {aktivni[i].Ime} {aktivni[i].Prezime} ({aktivni[i].Oib})");
                    }

                    int izborOsiguranika;

                    while (true)
                    {
                        try
                        {
                            Console.WriteLine("\nOdaberite osiguranika:");

                            if (!int.TryParse(Console.ReadLine(), out izborOsiguranika))
                                throw new Iznimke("Sklapanje police", "Morate unijeti broj.");

                            if (izborOsiguranika < 1 || izborOsiguranika > aktivni.Count)
                                throw new Iznimke("Sklapanje police", "Neispravan odabir.");

                            break;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    Osiguranik odabrani = aktivni[izborOsiguranika - 1];

                    int vrstaPolice;

                    while (true)
                    {
                        try
                        {
                            Console.WriteLine("\n1-Auto | 2-Životna | 3-Imovinska");

                            if (!int.TryParse(Console.ReadLine(), out vrstaPolice))
                                throw new Iznimke("Sklapanje police", "Morate unijeti broj.");

                            if (vrstaPolice < 1 || vrstaPolice > 3)
                                throw new Iznimke("Sklapanje police", "Neispravan odabir.");

                            break;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    double osiguranaSvota;
                    while (true)
                    {
                        try
                        {
                            Console.WriteLine("Unesite svotu:");

                            if (!double.TryParse(Console.ReadLine(), out osiguranaSvota))
                                throw new Exception("Neispravan unos.");

                            if (osiguranaSvota <= 0)
                                throw new Exception("Mora biti veće od 0.");

                            break;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }

                    DateTime datumSklapanja;
                    DateTime trajanje;

                    while (true)
                    {
                        Console.WriteLine("Datum sklapanja:");
                        DateTime.TryParse(Console.ReadLine(), out datumSklapanja);

                        Console.WriteLine("Datum isteka:");
                        DateTime.TryParse(Console.ReadLine(), out trajanje);

                        if (trajanje > datumSklapanja)
                            break;

                        Console.WriteLine("Greška u datumima.");
                    }

                    int sifra = 1;
                    while (police.ContainsKey(sifra))
                        sifra++;

                    Polica novaPolica;

                    switch (vrstaPolice)
                    {
                        case 1:
                            novaPolica = new AutoPolica(sifra, odabrani.Ime + " " + odabrani.Prezime, datumSklapanja, trajanje, osiguranaSvota);
                            break;
                        case 2:
                            novaPolica = new ZivotnaPolica(sifra, odabrani.Ime + " " + odabrani.Prezime, datumSklapanja, trajanje, osiguranaSvota);
                            break;
                        default:
                            novaPolica = new ImovinskaPolica(sifra, odabrani.Ime + " " + odabrani.Prezime, datumSklapanja, trajanje, osiguranaSvota);
                            break;
                    }

                    police.Add(sifra, novaPolica);
                    SpremanjePolica.Spremi(police);

                    Console.WriteLine("\nPolica uspješno kreirana!");

                    Console.WriteLine($"Godišnja premija iznosi: {novaPolica.izracunajGodisnjuPremiju():F2} EUR");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                Console.WriteLine("\nŽelite li još policu? (da/ne)");
                nastavakPolica = Console.ReadLine();

            } while (nastavakPolica.ToLower() != "ne");
        }
    }

}