using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VUV_osiguranje.Osoba;
using VUV_osiguranje.PoliceOsiguranja;

namespace VUV_osiguranje.Stete
{
    internal class Steta
    {
        private Polica _polica;
        private int _sifraPolice;
        private DateTime _datum;
        private string _opis;
        private double _iznos;
        private StatusStete _status;
       

        public Steta(Polica polica, int sifraPolice, DateTime datum, string opis, double iznos, StatusStete status)
        {
            _polica = polica;
            _sifraPolice = sifraPolice;
            _datum = datum;
            _opis = opis;
            _iznos = iznos;
            _status = status;
        }

        public Polica polica
        {
            get { return _polica; }
            set { _polica = value; }
        }

        public int sifraPolice
        {
            get { return _sifraPolice; }
            set { _sifraPolice = value; }
        }

        public DateTime datum
        {
            get { return _datum; }
            set { _datum = value; }
        }

        public string opis
        {
            get { return _opis; }
            set { _opis = value; }
        }

        public double iznos
        {
            get { return _iznos; }
            set { _iznos = value; }
        }

        public StatusStete status
        {
            get { return _status; }

            set
            {
                if(_status == StatusStete.Odobrena || _status == StatusStete.Odbijena)
                {
                    throw new InvalidOperationException("Status se ne može mijenjati nakon odobrenja ili odbijanja.");
                }

                _status = value;
            }
        }

        public void Odobri()
        {
            if(_status != StatusStete.Prijavljena)
            {
                throw new InvalidOperationException();
            }

            status = StatusStete.Odobrena;
        }

        public void Odbij()
        {
            if(_status != StatusStete.Prijavljena)
            {
                throw new InvalidOperationException();
            }

            status = StatusStete.Odbijena;
        }

        public static void PrijavaStete(List<Osiguranik> osiguranici,Dictionary<int, Polica> police,List<Steta> stete)
        {
            string nastavakStete = "";

            do
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\n*** ODABIR POLICE ***");
                    Console.ResetColor();

                    foreach (var p in police.Values)
                    {
                        string vrsta = p.GetType().Name;
                        Console.WriteLine($"{p.Sifra} - {p.Osiguranik} - {vrsta}");
                    }

                    int sifra;

                    while (true)
                    {
                        Console.WriteLine("\nUnesite šifru police:");

                        if (!int.TryParse(Console.ReadLine(), out sifra))
                        {
                            Console.WriteLine("Neispravan unos.");
                            continue;
                        }

                        if (!police.ContainsKey(sifra))
                        {
                            Console.WriteLine("Polica ne postoji.");
                            continue;
                        }

                        break;
                    }

                    DateTime datum;
                    while (true)
                    {
                        Console.WriteLine("Unesite datum štete:");

                        if (DateTime.TryParse(Console.ReadLine(), out datum))
                            break;

                        Console.WriteLine("Neispravan datum.");
                    }

                    Console.WriteLine("Unesite opis štete:");
                    string opis = Console.ReadLine();

                    double iznos;
                    while (true)
                    {
                        Console.WriteLine("Unesite iznos štete:");

                        if (double.TryParse(Console.ReadLine(), out iznos) && iznos >= 0)
                            break;

                        Console.WriteLine("Iznos ne smije biti negativan!");
                    }

                    Steta nova = new Steta(
                        police[sifra],
                        sifra,
                        datum,
                        opis,
                        iznos,
                        StatusStete.Prijavljena
                    );

                    stete.Add(nova);

                    SpremanjeSteta.Spremi(stete);

                    Console.WriteLine("\nŠteta uspješno prijavljena!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                Console.WriteLine("\nŽelite li prijaviti još jednu štetu? (da/ne)");
                nastavakStete = Console.ReadLine();

            } while (nastavakStete.ToLower() != "ne");
        }

        public static void RjesavanjeStete(List<Steta> stete)
        {
            string nastavakStete = "";

            do
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\n*** PRIJAVLJENE ŠTETE ***");
                    Console.ResetColor();

                    List<Steta> prijavljene = new List<Steta>();

                    foreach (var s in stete)
                    {
                        if (s.status == StatusStete.Prijavljena)
                            prijavljene.Add(s);
                    }

                    if (prijavljene.Count == 0)
                    {
                        Console.WriteLine("Nema prijavljenih šteta.");
                        break;
                    }

                    for (int i = 0; i < prijavljene.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. Polica: {prijavljene[i].polica.Sifra} | {prijavljene[i].opis} | {prijavljene[i].iznos} EUR");
                    }

                    int izbor;

                    while (true)
                    {
                        Console.WriteLine("\nOdaberite štetu za obradu:");

                        if (!int.TryParse(Console.ReadLine(), out izbor))
                        {
                            Console.WriteLine("Neispravan unos.");
                            continue;
                        }

                        if (izbor < 1 || izbor > prijavljene.Count)
                        {
                            Console.WriteLine("Neispravan odabir.");
                            continue;
                        }

                        break;
                    }

                    Steta odabrana = prijavljene[izbor - 1];

                    Console.WriteLine("\nOdaberite novi status:");
                    Console.WriteLine("1 - Odobrena");
                    Console.WriteLine("2 - Odbijena");

                    int statusOdabir;

                    while (true)
                    {
                        if (!int.TryParse(Console.ReadLine(), out statusOdabir))
                        {
                            Console.WriteLine("Neispravan unos.");
                            continue;
                        }

                        if (statusOdabir != 1 && statusOdabir != 2)
                        {
                            Console.WriteLine("Odaberi 1 ili 2.");
                            continue;
                        }

                        break;
                    }

                    switch (statusOdabir)
                    {
                        case 1:
                            odabrana.status = StatusStete.Odobrena;
                            break;

                        case 2:
                            odabrana.status = StatusStete.Odbijena;
                            break;
                    }

                    SpremanjeSteta.Spremi(stete);

                    Console.WriteLine("\nStatus štete uspješno ažuriran!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                Console.WriteLine("\nŽelite li obraditi još jednu štetu? (da/ne)");
                nastavakStete = Console.ReadLine();

            } while (nastavakStete.ToLower().Trim() != "ne");
        }

        public static void PregledStetaPoPolici(List<Steta> stete, Dictionary<int, Polica> police)
        {
            string nastavak;

            do
            {
                try
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\n*** ODABIR POLICE ***");
                    Console.ResetColor();

                    foreach (var p in police.Values)
                    {
                        Console.WriteLine($"{p.Sifra} - {p.Osiguranik} - {p.GetType().Name}");
                    }

                    int sifra;

                    while (true)
                    {
                        Console.WriteLine("\nUnesite šifru police:");

                        if (!int.TryParse(Console.ReadLine(), out sifra))
                        {
                            Console.WriteLine("Neispravan unos.");
                            continue;
                        }

                        if (!police.ContainsKey(sifra))
                        {
                            Console.WriteLine("Polica ne postoji.");
                            continue;
                        }

                        break;
                    }

                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"\n*** SVE PRIJAVLJENE ŠTETE ZA POLICU {sifra} ***");
                    Console.ResetColor();

                    bool postoji = false;

                    foreach (var s in stete)
                    {
                        if (s.polica.Sifra == sifra)
                        {
                            postoji = true;

                            Console.WriteLine(
                                $"Datum: {s.datum} | " +
                                $"Iznos: {s.iznos} EUR | " +
                                $"Status: {s.status}"
                            );
                        }
                    }

                    if (!postoji)
                    {
                        Console.WriteLine("Nema prijavljenih šteta za ovu policu.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Greška: " + ex.Message);
                }

                Console.WriteLine("\nŽelite li pregledati štete za još jednu policu? (da/ne)");
                nastavak = Console.ReadLine();

            } while (nastavak.ToLower().Trim() != "ne");
        }
    }
}

