using FarmSimulator.Core.Models.SpravaFarmy;
using FarmSimulator.Core.SpracovanieDat;
using FarmSimulator.Core.Properties.Dobytok;
using FarmSimulator.Core.Enums.Stromy;
using FarmSimulator.Core.Enums.Zelenina;
using FarmSimulator.Core.Models.Statky.Zvierata.Dobytok;
using FarmSimulator.Core.Models.Statky.Rastliny.Stromy;
using FarmSimulator.Core.Models.Statky.Rastliny.Zelenina;
using FarmSimulator.Core.Models.Statky;

namespace FarmSimulator.CLI
{
    class Program
    {
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

                case "kup":
                    if (args.Length > 1)
                    {
                        string coKupit = args[1];

                        if (KatalogObchodu.TryGetValue(coKupit, out Func<Statok>? vytvorStatok))
                        {
                            Statok novyStatok = vytvorStatok();

                            if (Farmar.Instance.KupStatok(novyStatok))
                                Console.WriteLine($"[Obchod] Úspešne si si kúpil: {novyStatok.Nazov}");
                            else
                                Console.WriteLine("[Obchod] Nemáš dostatok peňazí!");
                        }
                        else
                        {
                            string ponuka = string.Join(", ", KatalogObchodu.Keys);
                            Console.WriteLine($"Neznámy tovar: '{coKupit}'. Dostupné sú: {ponuka}");
                        }
                    }
                    else Console.WriteLine("Chyba: Musíš zadať, čo chceš kúpiť. Príklad: dotnet run kup krava");
                    break;

                case "predaj":
                    if (args.Length > 1)
                    {
                        string coPredat = args[1].ToLower();

                        var produkt = Sklad.Instance.UskladneneProdukty
                            .FirstOrDefault(p => p.Nazov.ToLower() == coPredat);

                        if (produkt != null)
                        {
                            Sklad.Instance.UskladneneProdukty.Remove(produkt);

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
            if (Farma.Instance.Statky.Count == 0) Console.WriteLine("Farma je prázdna.");

            // Zhlukovanie pomocou LINQ pre pekný výpis
            var zoskupeneStatky = Farma.Instance.Statky
                .GroupBy(s => s.Nazov)
                .Select(g => $"- {g.Key}: {g.Count()} ks");

            foreach (var s in zoskupeneStatky) Console.WriteLine(s);

            Console.WriteLine($"\n--- Sklad ({Sklad.Instance.UskladneneProdukty.Count}) ---");
            if (Sklad.Instance.UskladneneProdukty.Count == 0) Console.WriteLine("Sklad je prázdny.");

            var zoskupeneProdukty = Sklad.Instance.UskladneneProdukty
                .GroupBy(p => p.Nazov)
                .Select(g => $"- {g.Key}: {g.Count()} ks");

            foreach (var p in zoskupeneProdukty) Console.WriteLine(p);

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