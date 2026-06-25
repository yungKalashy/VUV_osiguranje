using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VUV_osiguranje.Osoba
{
    internal class Osiguranik : Osoba
    {

        private bool _deleted;
        public Osiguranik() : base() { }
        public Osiguranik(string oib, string ime, string prezime, bool deleted) : base(oib, ime, prezime)
        {
            _deleted = deleted;
        }

        public bool Deleted
        {
            get { return _deleted; }
            set { _deleted = value; }
        }

        public static void DodajOsiguranike(List<Osiguranik> osiguranici)
        {
            string nastavak;

            do
            {
                string oib;
                string ime;
                string prezime;
                bool deleted;

                // OIB
                while (true)
                {
                    try
                    {
                        Console.WriteLine("OIB:");
                        oib = Console.ReadLine();

                        bool sviBrojevi = true;

                        foreach (char c in oib)
                        {
                            if (!char.IsDigit(c))
                            {
                                sviBrojevi = false;
                                break;
                            }
                        }

                        if (string.IsNullOrWhiteSpace(oib) || oib.Length != 11 || !sviBrojevi)
                            throw new Exception("OIB mora imati točno 11 znamenki.");

                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                // IME
                while (true)
                {
                    try
                    {
                        Console.WriteLine("Ime:");
                        ime = Console.ReadLine();

                        foreach (char c in ime)
                        {
                            if (!char.IsLetter(c) && c != ' ')
                                throw new Exception("Ime smije sadržavati samo slova.");
                        }

                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                // PREZIME
                while (true)
                {
                    try
                    {
                        Console.WriteLine("Prezime:");
                        prezime = Console.ReadLine();

                        foreach (char c in prezime)
                        {
                            if (!char.IsLetter(c) && c != ' ')
                                throw new Exception("Prezime smije sadržavati samo slova.");
                        }

                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                // STATUS
                while (true)
                {
                    try
                    {
                        Console.WriteLine("Status (true/false):");

                        if (!bool.TryParse(Console.ReadLine(), out deleted))
                            throw new Exception("Status mora biti true ili false.");

                        break;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                // KREIRANJE OBJEKTA
                Osiguranik novi = new Osiguranik(oib, ime, prezime, deleted);
                osiguranici.Add(novi);

                try
                {
                    SpremanjeOsiguranika.Spremi(osiguranici);
                    Console.WriteLine("Osiguranik uspješno spremljen!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                Console.WriteLine("Želite li unijeti još osiguranika? (da/ne)");
                nastavak = Console.ReadLine();

            } while (nastavak.ToLower() != "ne");

            
        }

        //Azuriranje
        public static void UrediOsiguranike(List<Osiguranik> osiguranici)
        {
            string nastavak;

            do
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n*** POPIS OSIGURANIKA ***");
                Console.ResetColor();

                List<Osiguranik> aktivni = new List<Osiguranik>();

                foreach (Osiguranik o in osiguranici)
                {
                    if (!o.Deleted)
                        aktivni.Add(o);
                }

                Console.WriteLine("----------------------------------------------------");
                Console.WriteLine($"{"Index",-5}{"OIB",-15}{"Ime",-15}{"Prezime",-15}");
                Console.WriteLine("----------------------------------------------------");

                for (int i = 0; i < aktivni.Count; i++)
                {
                    Console.WriteLine($"{i + 1,-5}{aktivni[i].Oib,-15}{aktivni[i].Ime,-15}{aktivni[i].Prezime,-15}");
                }

                Console.WriteLine("----------------------------------------------------");

                try
                {
                    if (aktivni.Count == 0)
                        throw new Exception("Nema aktivnih osiguranika.");

                    Console.WriteLine("\nUnesite broj osiguranika:");
                    if (!int.TryParse(Console.ReadLine(), out int izbor))
                        throw new Exception("Neispravan unos.");

                    if (izbor < 1 || izbor > aktivni.Count)
                        throw new Exception("Neispravan odabir.");

                    Osiguranik odabrani = aktivni[izbor - 1];

                    Console.WriteLine($"\nOdabrani: {odabrani.Ime} {odabrani.Prezime} ({odabrani.Oib})");

                    // NOVO IME
                    odabrani.Ime = UnosTeksta("Novo ime", true);

                    // NOVO PREZIME
                    odabrani.Prezime = UnosTeksta("Novo prezime", true);

                    // STATUS
                    odabrani.Deleted = UnosBool("Novi status (true/false)");

                    SpremanjeOsiguranika.Spremi(osiguranici);
                    Console.WriteLine("\nPromjene uspješno spremljene!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                Console.WriteLine("\nŽelite li ažurirati još osiguranika? (da/ne)");
                nastavak = Console.ReadLine();

            } while (nastavak.ToLower() != "ne");
        }

        // helper metoda za tekst
        private static string UnosTeksta(string poruka, bool obaveznoSlova)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine(poruka + ":");
                    string unos = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(unos))
                        throw new Exception("Vrijednost ne smije biti prazna.");

                    if (obaveznoSlova)
                    {
                        foreach (char c in unos)
                        {
                            if (!char.IsLetter(c) && c != ' ')
                                throw new Exception("Dozvoljena su samo slova.");
                        }
                    }

                    return unos;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        // helper za bool
        private static bool UnosBool(string poruka)
        {
            while (true)
            {
                Console.WriteLine(poruka + ":");

                if (bool.TryParse(Console.ReadLine(), out bool result))
                    return result;

                Console.WriteLine("Unesite true ili false.");
            }
        }

        //Brisanje

        public static void ObrisiOsiguranike(List<Osiguranik> osiguranici)
        {
            string nastavak="";

            do
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n*** POPIS OSIGURANIKA (AKTIVNI I OBRISANI) ***");
                Console.ResetColor();

                Console.WriteLine("----------------------------------------------------");
                Console.WriteLine($"{"Index",-5}{"OIB",-15}{"Ime",-15}{"Prezime",-15}{"Status",-10}");
                Console.WriteLine("----------------------------------------------------");

                for (int i = 0; i < osiguranici.Count; i++)
                {
                    string status = osiguranici[i].Deleted ? "OBRISAN" : "AKTIVAN";

                    Console.WriteLine($"{i + 1,-5}{osiguranici[i].Oib,-15}{osiguranici[i].Ime,-15}{osiguranici[i].Prezime,-15}{status,-10}");
                }

                Console.WriteLine("----------------------------------------------------");

                try
                {
                    if (osiguranici.Count == 0)
                        throw new Exception("Nema osiguranika u sustavu.");

                    Console.WriteLine("\nUnesite broj osiguranika:");
                    if (!int.TryParse(Console.ReadLine(), out int izbor))
                        throw new Exception("Neispravan unos.");

                    if (izbor < 1 || izbor > osiguranici.Count)
                        throw new Exception("Neispravan odabir.");

                    Osiguranik odabrani = osiguranici[izbor - 1];

                    Console.WriteLine($"\nOdabrani: {odabrani.Ime} {odabrani.Prezime} ({odabrani.Oib})");

                    if (odabrani.Deleted)
                        throw new Exception("Osiguranik je već obrisan.");

                    Console.WriteLine("\nJeste li sigurni da želite obrisati? (da/ne)");
                    string potvrda = Console.ReadLine().Trim().ToLower();

                    if (potvrda != "da")
                    {
                        Console.WriteLine("Brisanje poništeno.");
                        continue;
                    }

                    odabrani.Deleted = true;

                    SpremanjeOsiguranika.Spremi(osiguranici);

                    Console.WriteLine("\nOsiguranik je označen kao obrisan.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                Console.WriteLine("\nŽelite li obrisati još osiguranika? (da/ne)");
                nastavak = Console.ReadLine();

            } while (nastavak.ToLower().Trim() != "ne");
        }
    }
}
