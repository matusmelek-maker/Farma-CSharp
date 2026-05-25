using FarmSimulator.Core.Models.SpravaFarmy;
using FarmSimulator.Core.SpracovanieDat;
using FarmSimulator.Core.Properties.Dobytok;
using FarmSimulator.Core.Enums.Stromy;
using FarmSimulator.Core.Enums.Zelenina;
using FarmSimulator.Core.Models.Statky.Zvierata.Dobytok;
using FarmSimulator.Core.Models.Statky.Rastliny.Stromy;
using FarmSimulator.Core.Models.Statky.Rastliny.Zelenina;
using FarmSimulator.Core.Models.Statky;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq; // Potrebné pre LINQ dotazy (napr. FirstOrDefault)

namespace FarmSimulator.CLI
{
    class Program
    {
        // TOTO JE NAŠE ZJEDNODUŠENIE! 
        // Tu zadefinujeme náš "obchod". Kľúč je slovo, hodnota je funkcia, ktorá vytvorí statok.
        // StringComparer.OrdinalIgnoreCase zabezpečí, že nezáleží na tom, či napíšeš "Krava" alebo "krava".
        private static readonly Dictionary<string, Func<Statok>> KatalogObchodu = new(StringComparer.OrdinalIgnoreCase)
        {
            { "krava", () => new Dobytok(TypyDobytka.Krava) },
            { "byk", () => new Dobytok(TypyDobytka.Byk) },
            { "slivka", () => new Strom(TypyStromov.Slivka) },
            { "semiacka_paradajka", () => new Zelenina(TypyZeleniny.Paradajka) }
        };

        static void Main(string[] args)
        {
            if (args.Length == 0 || args[0] == "--help" || args[0] == "-h")
            {
                VypisNapovedu();
                return;
            }

            string prikaz = args[0].ToLower();

            if (prikaz == "reset")
            {
                if (File.Exists("farma_save.json"))
                {
                    File.Delete("farma_save.json");
                    Console.WriteLine("Uložená hra bola vymazaná. Pri ďalšom príkaze začneš odznova.");
                }
                else Console.WriteLine("Žiadna uložená hra sa nenašla.");
                return;
            }

            bool nacitane = SpravcaUlozenia.NacitajHru();
            if (!nacitane)
            {
                Console.WriteLine("[Systém] Vytváram novú farmu so štartovacím kapitálom 1000$.");
                Farmar.Instance.NastavPeniaze(1000);
            }

            switch (prikaz)
            {
                case "stav":
                    VypisStav();
                    break;

                case "posun":
                    if (args.Length > 1 && int.TryParse(args[1], out int pocetTikov))
                    {
                        Farma.Instance.PosunCas(pocetTikov);
                    }
                    else Console.WriteLine("Chyba: Musíš zadať počet dní. Príklad: dotnet run posun 5");
                    break;

                // NOVÝ KUP (Zjednodušený bez IFov!)
                case "kup":
                    if (args.Length > 1)
                    {
                        string coKupit = args[1];

                        // Pozrieme sa, či slovo existuje v našom slovníku
                        if (KatalogObchodu.TryGetValue(coKupit, out Func<Statok>? vytvorStatok))
                        {
                            // Spustíme funkciu, ktorá vyrobí objekt
                            Statok novyStatok = vytvorStatok();

                            if (Farmar.Instance.KupStatok(novyStatok))
                                Console.WriteLine($"[Obchod] Úspešne si si kúpil: {novyStatok.Nazov}");
                            else
                                Console.WriteLine("[Obchod] Nemáš dostatok peňazí!");
                        }
                        else
                        {
                            // Ak napísal niečo zlé, rovno mu vypíšeme všetky kľúče z nášho slovníka
                            string ponuka = string.Join(", ", KatalogObchodu.Keys);
                            Console.WriteLine($"Neznámy tovar: '{coKupit}'. Dostupné sú: {ponuka}");
                        }
                    }
                    else Console.WriteLine("Chyba: Musíš zadať, čo chceš kúpiť. Príklad: dotnet run kup krava");
                    break;

                // NOVÝ PREDAJ
                case "predaj":
                    if (args.Length > 1)
                    {
                        string coPredat = args[1].ToLower();

                        // Nájdeme v sklade prvý produkt, ktorého názov sa presne zhoduje s tým, čo chceš predať
                        var produkt = Sklad.Instance.UskladneneProdukty
                            .FirstOrDefault(p => p.Nazov.ToLower() == coPredat);

                        if (produkt != null)
                        {
                            // 1. Zmažeme produkt zo skladu
                            Sklad.Instance.UskladneneProdukty.Remove(produkt);
                            // 2. Pripíšeme farmárovi peniaze
                            Farmar.Instance.AktualizujPeniaze(produkt.PredajnaCena);

                            Console.WriteLine($"[Trh] Úspešne si predal '{produkt.Nazov}' za {produkt.PredajnaCena} $.");
                        }
                        else
                        {
                            Console.WriteLine($"[Chyba] Tovar '{args[1]}' sa v sklade nenachádza!");
                        }
                    }
                    else Console.WriteLine("Chyba: Musíš zadať, čo chceš predať z produktov v sklade. Príklad: dotnet run predaj hovädzina");
                    break;

                case "uloz":
                    Console.WriteLine("[Príkaz] Hra bude manuálne uložená.");
                    break;

                default:
                    Console.WriteLine($"Neznámy príkaz: {prikaz}");
                    VypisNapovedu();
                    break;
            }

            SpravcaUlozenia.UlozHru();
        }

        static void VypisStav()
        {
            Console.WriteLine("\n=== STAV FARMY ===");
            Console.WriteLine($"Peniaze farmára: {Farmar.Instance.Peniaze} $");

            Console.WriteLine($"\n--- Statky na farme ({Farma.Instance.Statky.Count}) ---");
            Farma.Instance.VypisStatky();

            Console.WriteLine($"\n--- Sklad ({Sklad.Instance.UskladneneProdukty.Count}) ---");
            Sklad.Instance.VypisProdukty();

            Console.WriteLine("==================\n");
        }

        static void VypisNapovedu()
        {
            Console.WriteLine("=== FARM SIMULATOR CLI ===");
            Console.WriteLine("Použitie:");
            Console.WriteLine("  dotnet run <príkaz> [argumenty]");
            Console.WriteLine("\nDostupné príkazy:");
            Console.WriteLine("  --help        Vypíše túto nápovedu.");
            Console.WriteLine("  stav          Zobrazí aktuálny stav peňazí, farmy a skladu.");
            Console.WriteLine("  posun <dni>   Posunie čas na farme o zadaný počet dní (tikov).");
            Console.WriteLine("  kup <tovar>   Kúpi vybraný statok z ponuky.");
            Console.WriteLine("  predaj <vec>  Predá konkrétny produkt zo skladu.");
            Console.WriteLine("  uloz          Manuálne uloží aktuálny stav farmy.");
            Console.WriteLine("  reset         Vymaže uloženú hru a začneš odznova.");
        }
    }
}