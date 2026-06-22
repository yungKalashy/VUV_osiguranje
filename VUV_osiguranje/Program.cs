using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Xml.Linq;
using System.Xml.XPath;
using VUV_osiguranje.Osoba;
using VUV_osiguranje.Stete;

namespace VUV_osiguranje.PoliceOsiguranja.Osoba.Stete
{ 

    internal class Program
 {
        static void Main(string[] args)
        {
            List<Osiguranik> osiguranici = new List<Osiguranik>();

            try
            {
                osiguranici = XDocument
                .Load("Osiguranici.xml")
                .Root
                .Elements("Osiguranik")
                .Select(x => new Osiguranik(
                x.Element("OIB")?.Value,
                x.Element("Ime")?.Value,
                x.Element("Prezime")?.Value,
                bool.Parse(x.Element("Deleted")?.Value ?? "false")
                ))
                .ToList();
            } 
            catch(Exception ex)
            {
                Console.WriteLine("Greška pri učitavanju Osiguranici.xml: " + ex.Message);
            }


            Dictionary<int, Polica> police = new Dictionary<int, Polica>();

            try
            {
                var lista = XDocument
                    .Load("Police.xml")
                    .Root
                    .Elements("Polica")
                    .Select(x => new AutoPolica(  
                        int.Parse(x.Element("Sifra")?.Value ?? "0"),
                        x.Element("Osiguranik")?.Value,
                        DateTime.Parse(x.Element("DatumSklapanja")?.Value),
                        DateTime.Parse(x.Element("DatumTrajanja")?.Value),
                        double.Parse(x.Element("OsiguranaSvota")?.Value)
                    ))
                    .ToList();

                foreach (var p in lista)
                {
                    police.Add(p.Sifra, p);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Greška pri učitavanju Police.xml: " + ex.Message);
            }

            List<VUV_osiguranje.Stete.Steta> stete = new();

            try
            {
                var xml = XDocument.Load("Steta.xml");

                stete = xml.Root.Elements("Steta")
                .Select(x =>
                {
                    int sifraPolice = int.Parse(x.Element("SifraPolice")?.Value ?? "0");

                    police.TryGetValue(sifraPolice, out var polica);

                    return new Steta(
                    polica,
                    DateTime.Parse(x.Element("Datum")?.Value),
                    x.Element("Opis")?.Value,
                    double.Parse(x.Element("Iznos")?.Value),
                    Enum.Parse<StatusStete>(x.Element("Status")?.Value)
                    );
                })
                .ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Greška u Stete.xml: " + ex.Message);
            }


            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("***** IZBORNIK *****");
            Console.ResetColor();
            Console.WriteLine("\n 1 - Dodavanje/Azuriranje/Brisanje osiguranika \n 2 - Sklapanje police \n 3 - Prijava stete \n 4 - Rjesavanje stete \n 5 - Pregled steta po polici \n 6 - Statistika");

            int odabir = 0;
            odabir = Convert.ToInt32(Console.ReadLine());

            switch(odabir)
            {
                case 1:

                    int odabir1 = 0;

                    Console.WriteLine("\n 1 - Dodavanje osiguranika \n 2 - Azuriranje osiguranika \n 3 - Brisanje osiguranika");
                    odabir1 = Convert.ToInt32(Console.ReadLine());

                    switch(odabir1)
                    {
                        case 1:

                            string nastavak = "";

                            do
                            {
                                Console.WriteLine("Unesite OIB, Ime, Prezime te status osiguranika:");

                                string oib;
                                string ime;
                                string prezime;
                                bool deleted;

                                // Oib
                                while (true)
                                {
                                    try
                                    {
                                        Console.WriteLine("OIB:");
                                        oib = Console.ReadLine();

                                        if (string.IsNullOrWhiteSpace(oib) ||
                                            oib.Length != 11 ||
                                            !oib.All(char.IsDigit))
                                        {
                                            throw new Iznimke(
                                                "Dodavanje osiguranika",
                                                "OIB mora imati točno 11 znamenki."
                                            );
                                        }

                                        break;
                                    }
                                    catch (Iznimke ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }
                                }

                                // Ime
                                while (true)
                                {
                                    try
                                    {
                                        Console.WriteLine("Ime:");
                                        ime = Console.ReadLine();

                                        if (string.IsNullOrEmpty(ime))
                                        {
                                            throw new Iznimke(
                                                "Dodavanje osiguranika",
                                                "Ime ne smije biti prazno."
                                            );
                                        }

                                        if (!ime.All(c => char.IsLetter(c) || c == ' '))
                                        {
                                            throw new Iznimke(
                                                "Dodavanje osiguranika",
                                                "Ime smije sadržavati samo slova."
                                            );
                                        }

                                        break;
                                    }
                                    catch (Iznimke ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }
                                }

                                // Prezime
                                while (true)
                                {
                                    try
                                    {
                                        Console.WriteLine("Prezime:");
                                        prezime = Console.ReadLine();

                                        if (string.IsNullOrEmpty(prezime))
                                        {
                                            throw new Iznimke(
                                                "Dodavanje osiguranika",
                                                "Prezime ne smije biti prazno."
                                            );
                                        }

                                        if (!prezime.All(c => char.IsLetter(c) || c == ' '))
                                        {
                                            throw new Iznimke(
                                                "Dodavanje osiguranika",
                                                "Prezime smije sadržavati samo slova."
                                            );
                                        }

                                        break;
                                    }
                                    catch (Iznimke ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }
                                }

                                // Status
                                while (true)
                                {
                                    try
                                    {
                                        Console.WriteLine("Status osiguranika (true/false):");

                                        if (!bool.TryParse(Console.ReadLine(), out deleted))
                                        {
                                            throw new Iznimke(
                                                "Dodavanje osiguranika",
                                                "Status mora biti true ili false."
                                            );
                                        }

                                        break;
                                    }
                                    catch (Iznimke ex)
                                    {
                                        Console.WriteLine(ex.Message);
                                    }
                                }

                                Osiguranik noviOsiguranik = new Osiguranik(
                                    oib,
                                    ime,
                                    prezime,
                                    deleted);

                                osiguranici.Add(noviOsiguranik);

                                try
                                {
                                    SpremanjeOsiguranika.Spremi(osiguranici);
                                    Console.WriteLine("\nOsiguranik dodan i spremljen!");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Greška prilikom spremanja XML-a: {ex.Message}");
                                }

                                Console.WriteLine("\nŽelite li unijeti još osiguranika? (da/ne)");
                                nastavak = Console.ReadLine();

                            } while (nastavak.ToLower() != "ne");

                            break;


                        case 2:

                            string nastavak1 = "";

                            do
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine("\n*** POPIS OSIGURANIKA ***");
                                Console.ResetColor();
                                Console.WriteLine("----------------------------------------------------");
                                Console.WriteLine($"{"Index",-5}{"OIB",-15}{"Ime",-15}{"Prezime",-15}");
                                Console.WriteLine("----------------------------------------------------");

                                int index = 1;

                                foreach (var o in osiguranici.Where(x => !x.Deleted))
                                {
                                    Console.WriteLine($"{index,-5}{o.Oib,-15}{o.Ime,-15}{o.Prezime,-15}");
                                    index++;
                                }

                                Console.WriteLine("----------------------------------------------------\n");

                                try
                                {
                                    var aktivni = osiguranici.Where(x => !x.Deleted).ToList();

                                    if (aktivni.Count == 0)
                                        throw new Iznimke("Ažuriranje osiguranika", "Nema aktivnih osiguranika.");

                                    

                                    Console.WriteLine("\nUnesite broj osiguranika:");
                                    int izbor = Convert.ToInt32(Console.ReadLine());

                                    if (izbor < 1 || izbor > aktivni.Count)
                                        throw new Iznimke("Ažuriranje osiguranika", "Neispravan odabir.");

                                    Osiguranik odabrani = aktivni[izbor - 1];

                                    Console.WriteLine($"\n✔ Odabrani osiguranik:");
                                    Console.WriteLine($"{odabrani.Ime} {odabrani.Prezime} ({odabrani.Oib})");

                                    // NOVO IME
                                   
                                    while (true)
                                    {
                                        try
                                        {
                                            Console.WriteLine("\nNovo ime:");
                                            string novoIme = Console.ReadLine();

                                            if (string.IsNullOrWhiteSpace(novoIme))
                                                throw new Iznimke("Ažuriranje osiguranika", "Ime ne smije biti prazno.");

                                            if (!novoIme.All(c => char.IsLetter(c) || c == ' '))
                                                throw new Iznimke("Ažuriranje osiguranika", "Ime smije sadržavati samo slova i razmake.");

                                            odabrani.Ime = novoIme;
                                            break;
                                        }
                                        catch (Iznimke ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }

                                    // NOVO PREZIME

                                    while (true)
                                    {
                                        try
                                        {
                                            Console.WriteLine("\nNovo prezime:");
                                            string novoPrezime = Console.ReadLine();

                                            if (string.IsNullOrWhiteSpace(novoPrezime))
                                                throw new Iznimke("Ažuriranje osiguranika", "Prezime ne smije biti prazno.");

                                            if (!novoPrezime.All(c => char.IsLetter(c) || c == ' '))
                                                throw new Iznimke("Ažuriranje osiguranika", "Prezime smije sadržavati samo slova i razmake.");

                                            odabrani.Prezime = novoPrezime;
                                            break;
                                        }
                                        catch (Iznimke ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }

                                    // STATUS
                                    while (true)
                                    {
                                        try
                                        {
                                            Console.WriteLine("\nNovi status (true/false):");

                                            if (!bool.TryParse(Console.ReadLine(), out bool deleted))
                                                throw new Iznimke("Ažuriranje osiguranika", "Status mora biti true ili false.");

                                            odabrani.Deleted = deleted;
                                            break;
                                        }
                                        catch (Iznimke ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                        }
                                    }

                                    // SPREMANJE U XML
                                    try
                                    {
                                        SpremanjeOsiguranika.Spremi(osiguranici);
                                        Console.WriteLine("\n Promjene uspješno spremljene u XML.");
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("Greška pri spremanju: " + ex.Message);
                                    }
                                }
                                catch (Iznimke ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }

                                Console.WriteLine("\nŽelite li ažurirati još osiguranika? (da/ne)");
                                nastavak1 = Console.ReadLine();

                            } while (nastavak1.ToLower() != "ne");

                            break;

                        case 3:

                            string nastavak3;

                            do
                            {
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                Console.WriteLine("\n*** POPIS OSIGURANIKA (AKTIVNI I OBRISANI) ***");
                                Console.ResetColor();
                                Console.WriteLine("----------------------------------------------------");
                                Console.WriteLine($"{"Index",-5}{"OIB",-15}{"Ime",-15}{"Prezime",-15}{"Status",-10}");
                                Console.WriteLine("----------------------------------------------------");

                                int index = 1;

                                foreach (var o in osiguranici)
                                {
                                    string status = o.Deleted ? "OBRISAN" : "AKTIVAN";

                                    Console.WriteLine($"{index,-5}{o.Oib,-15}{o.Ime,-15}{o.Prezime,-15}{status,-10}");
                                    index++;
                                }

                                Console.WriteLine("----------------------------------------------------\n");

                                try
                                {
                                    if (osiguranici.Count == 0)
                                        throw new Iznimke("Brisanje osiguranika", "Nema osiguranika u sustavu.");

                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("\n*** ODABERITE OSIGURANIKA ZA BRISANJE ***");
                                    Console.ResetColor();
                                

                                    Console.WriteLine("\nUnesite broj osiguranika:");
                                    int izbor = Convert.ToInt32(Console.ReadLine());

                                    if (izbor < 1 || izbor > osiguranici.Count)
                                        throw new Iznimke("Brisanje osiguranika", "Neispravan odabir.");

                                    Osiguranik odabrani = osiguranici[izbor - 1];

                                    Console.WriteLine($"\n Odabrani osiguranik:");
                                    Console.WriteLine($"{odabrani.Ime} {odabrani.Prezime} ({odabrani.Oib})");

                                    if (odabrani.Deleted)
                                    {
                                        throw new Iznimke("Brisanje osiguranika", "Ovaj osiguranik je već obrisan.");
                                    }

                                    
                                    Console.WriteLine("\nJeste li sigurni da želite obrisati ovog osiguranika? (da/ne)");
                                    string potvrda = Console.ReadLine().Trim().ToLower();

                                    if (potvrda != "da")
                                    {
                                        Console.WriteLine("Brisanje poništeno.");
                                        break;
                                    }

                                    odabrani.Deleted = true;

                                    try
                                    {
                                        SpremanjeOsiguranika.Spremi(osiguranici);
                                        Console.WriteLine("\n Osiguranik je označen kao obrisan (soft delete).");
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("Greška pri spremanju: " + ex.Message);
                                    }
                                }
                                catch (Iznimke ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }

                                Console.WriteLine("\nŽelite li obrisati još osiguranika? (da/ne)");
                                nastavak3 = Console.ReadLine();

                            } while (nastavak3.ToLower().Trim() != "ne");

                            break;
                    }

                    break;

                // Sklapanje polica

                case 2:

                    string nastavakPolica = "";

                    do
                    {
                        try
                        {
                            var aktivni = osiguranici.Where(x => !x.Deleted).ToList();

                            if (aktivni.Count == 0)
                                throw new Iznimke("Sklapanje police", "Nema aktivnih osiguranika.");

                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("\n*** AKTIVNI OSIGURANICI ***");
                            Console.ResetColor();
                            Console.WriteLine("----------------------------------------------------");

                            for (int i = 0; i < aktivni.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}. {aktivni[i].Ime} {aktivni[i].Prezime} ({aktivni[i].Oib})");
                            }

                            Console.WriteLine("----------------------------------------------------");

                            int izborOsiguranika;

                            while (true)
                            {
                                try
                                {
                                    Console.WriteLine("\nOdaberite osiguranika:");

                                    if (!int.TryParse(Console.ReadLine(), out izborOsiguranika))
                                        throw new Iznimke("Sklapanje police", "Morate unijeti broj.");

                                    if (izborOsiguranika < 1 || izborOsiguranika > aktivni.Count)
                                        throw new Iznimke("Sklapanje police", "Neispravan odabir osiguranika.");

                                    break;
                                }
                                catch (Iznimke ex)
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
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    Console.WriteLine("\n*** VRSTA POLICE ***");
                                    Console.ResetColor();
                                    Console.WriteLine("1 - Auto polica");
                                    Console.WriteLine("2 - Životna polica");
                                    Console.WriteLine("3 - Imovinska polica");

                                    if (!int.TryParse(Console.ReadLine(), out vrstaPolice))
                                        throw new Iznimke("Sklapanje police", "Morate unijeti broj.");

                                    if (vrstaPolice < 1 || vrstaPolice > 3)
                                        throw new Iznimke("Sklapanje police", "Odaberite 1, 2 ili 3.");

                                    break;
                                }
                                catch (Iznimke ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }

                            double osiguranaSvota;

                            while (true)
                            {
                                try
                                {
                                    Console.WriteLine("\nUnesite osiguranu svotu:");

                                    if (!double.TryParse(Console.ReadLine(), out osiguranaSvota))
                                        throw new Iznimke("Sklapanje police", "Osigurana svota mora biti broj.");

                                    if (osiguranaSvota <= 0)
                                        throw new Iznimke("Sklapanje police", "Osigurana svota mora biti veća od 0.");

                                    break;
                                }
                                catch (Iznimke ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }

                            DateTime datumSklapanja;

                            while (true)
                            {
                                try
                                {
                                    Console.WriteLine("\nUnesite datum sklapanja (DD.MM.YYYY):");

                                    if (!DateTime.TryParse(Console.ReadLine(), out datumSklapanja))
                                        throw new Iznimke("Sklapanje police", "Neispravan datum sklapanja.");

                                    break;
                                }
                                catch (Iznimke ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }

                            DateTime trajanje;

                            while (true)
                            {
                                try
                                {
                                    Console.WriteLine("\nUnesite datum isteka police (DD.MM.YYYY):");

                                    if (!DateTime.TryParse(Console.ReadLine(), out trajanje))
                                        throw new Iznimke("Sklapanje police", "Neispravan datum isteka.");

                                    if (trajanje <= datumSklapanja)
                                        throw new Iznimke("Sklapanje police", "Datum isteka mora biti nakon datuma sklapanja.");

                                    break;
                                }
                                catch (Iznimke ex)
                                {
                                    Console.WriteLine(ex.Message);
                                }
                            }

                            int sifra = police.Count + 1;

                            Polica novaPolica;

                            switch (vrstaPolice)
                            {
                                case 1:
                                    novaPolica = new AutoPolica(sifra, odabrani.Ime + " " + odabrani.Prezime, datumSklapanja, trajanje, osiguranaSvota);
                                    break;

                                case 2:
                                    novaPolica = new ZivotnaPolica(sifra, odabrani.Ime + " " + odabrani.Prezime, datumSklapanja, trajanje, osiguranaSvota);
                                    break;

                                case 3:
                                    novaPolica = new ImovinskaPolica(sifra, odabrani.Ime + " " + odabrani.Prezime, datumSklapanja, trajanje, osiguranaSvota);
                                    break;

                                default:
                                    throw new Iznimke("Sklapanje police", "Nepostojeća vrsta police.");
                            }

                            police.Add(novaPolica.Sifra, novaPolica);

                            SpremanjePolica.Spremi(police);

                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("\n*** POLICA USPJEŠNO SKLOPLJENA ***");
                            Console.ResetColor();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        Console.WriteLine("\nŽelite li sklopiti još jednu policu? (da/ne)");
                        nastavakPolica = Console.ReadLine();

                    } while (nastavakPolica.ToLower() != "ne");

                    break;

                case 3:
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
                                    Console.WriteLine($"{p.Sifra} - {p.Osiguranik}");
                                }

                                Console.WriteLine("\nUnesite šifru police:");
                                int sifra = int.Parse(Console.ReadLine());

                                if (!police.ContainsKey(sifra))
                                    throw new Exception("Polica ne postoji.");

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

                        break;
                    }

                case 4:
                    {
                        try
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Console.WriteLine("\n*** PRIJAVLJENE ŠTETE ***");
                            Console.ResetColor();

                            var prijavljene = stete
                                .Where(s => s.status == StatusStete.Prijavljena)
                                .ToList();

                            if (prijavljene.Count == 0)
                            {
                                Console.WriteLine("Nema prijavljenih šteta.");
                                break;
                            }

                            for (int i = 0; i < prijavljene.Count; i++)
                            {
                                Console.WriteLine($"{i + 1}. Polica: {prijavljene[i].polica.Sifra} | {prijavljene[i].opis} | {prijavljene[i].iznos} €");
                            }

                            Console.WriteLine("\nOdaberite štetu za obradu:");
                            int izbor = int.Parse(Console.ReadLine());

                            if (izbor < 1 || izbor > prijavljene.Count)
                                throw new Exception("Neispravan odabir štete.");

                            Steta odabrana = prijavljene[izbor - 1];

                            
                            if (odabrana.status != StatusStete.Prijavljena)
                                throw new Exception("Status ove štete se više ne može mijenjati.");

                            Console.WriteLine("\nOdaberite novi status:");
                            Console.WriteLine("1 - Odobrena");
                            Console.WriteLine("2 - Odbijena");

                            int statusOdabir = int.Parse(Console.ReadLine());

                            switch (statusOdabir)
                            {
                                case 1:
                                    odabrana.status = StatusStete.Odobrena;
                                    break;

                                case 2:
                                    odabrana.status = StatusStete.Odbijena;
                                    break;

                                default:
                                    throw new Exception("Neispravan odabir statusa.");
                            }

                            SpremanjeSteta.Spremi(stete);

                            Console.WriteLine("\nStatus štete uspješno ažuriran!");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        break;
                    }
            }
    }
 }
}