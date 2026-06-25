using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Xml;
using VUV_osiguranje.Osoba;
using VUV_osiguranje.Stete;

namespace VUV_osiguranje.PoliceOsiguranja.Osoba.Stete
{ 

    internal class Program
 {
        static void Main(string[] args)
        {

            List<Osiguranik> osiguranici = XMLUcitavanje.UcitajOsiguranike();
            Dictionary<int, Polica> police = XMLUcitavanje.UcitajPolice();
            List<Steta> stete = XMLUcitavanje.UcitajStete(police);

            string unosGlavni;

            do
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("***** IZBORNIK *****");
                Console.ResetColor();
                Console.WriteLine("\n 1 - Dodavanje/Azuriranje/Brisanje osiguranika \n 2 - Sklapanje police \n 3 - Prijava stete \n 4 - Rjesavanje stete \n 5 - Pregled steta po polici \n 6 - Statistika");

                int odabir = 0;
                bool ispravanUnos = false;

                while (!ispravanUnos)
                {
                    try
                    {
                        Console.WriteLine("Unesite odabir (1-6) ili 'stop':");

                        unosGlavni = Console.ReadLine();

                        if (unosGlavni.ToLower() == "stop")
                            return;

                        odabir = Convert.ToInt32(unosGlavni);

                        if (odabir < 1 || odabir > 6)
                            throw new Exception("Odabir mora biti između 1 i 6.");

                        ispravanUnos = true;
                    }
                    catch (FormatException)
                    {
                        Console.WriteLine("Greška: morate unijeti broj.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }

                switch (odabir)
                {
                    case 1:

                        int odabir1 = 0;
                        bool ispravanUnos1 = false;

                        Console.WriteLine("\n 1 - Dodavanje osiguranika \n 2 - Azuriranje osiguranika \n 3 - Brisanje osiguranika");

                        while (!ispravanUnos1)
                        {
                            try
                            {
                                Console.WriteLine("Unesite odabir (1-3):");
                                odabir1 = Convert.ToInt32(Console.ReadLine());

                                if (odabir1 < 1 || odabir1 > 3)
                                    throw new Exception("Odabir mora biti između 1 i 3.");

                                ispravanUnos1 = true;
                            }
                            catch (FormatException)
                            {
                                Console.WriteLine("Greška: morate unijeti broj.");
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }

                        switch (odabir1)
                        {
                            case 1:
                                Osiguranik.DodajOsiguranike(osiguranici);
                                break;

                            case 2:
                                Osiguranik.UrediOsiguranike(osiguranici);
                                break;

                            case 3:
                                Osiguranik.ObrisiOsiguranike(osiguranici);
                                break;
                        }

                        break;

                    case 2:
                        Polica.SklapanjePolice(osiguranici, police);
                        break;

                    case 3:
                        Steta.PrijavaStete(osiguranici, police, stete);
                        break;

                    case 4:
                        Steta.RjesavanjeStete(stete);
                        break;

                    case 5:
                        Steta.PregledStetaPoPolici(stete, police);
                        break;

                    case 6:
                        Statistika.PremijePoVrsti(police);
                        Statistika.Top5Osiguranika(stete);
                        Statistika.UdioOdobrenihSteta(stete);
                        break;
                }

            } while (true);
        }
    }  
}