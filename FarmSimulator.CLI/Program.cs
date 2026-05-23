
using FarmSimulator.Core.Enums.Dobytok;
using FarmSimulator.Core.Models;
using FarmSimulator.Core.Models.Statky;
using FarmSimulator.Core.Models.Statky.ProdukcneStatky.Zvierata.Dobytok;
using System.Runtime.CompilerServices;





// Spustenie samotnej slučky (ak používaš Top-level statements)
ChodFarmy();




Console.WriteLine("Program skončil.");


void vytvorFarmara()
{
    var farmar = new Farmar();
    
    // Pridáme nejaké statky na farmu
    Console.WriteLine("Vytváram farmára, penazi ma " + farmar.Peniaze);
    farmar.KupStatok(new Dobytok(TypyDobytka.Krava));
    Console.WriteLine("Farmár kúpil kravu, penazi ma teraz " + farmar.Peniaze);
    farmar.PredajStatok(Farma.Instance.Statky[0]);
    
    Console.WriteLine( "pocet statkov na farme: " + Farma.Instance.Statky.Count);

}

void ChodFarmy()
{
    var farmar = new Farmar(); // Inicializujeme farmára
    Console.WriteLine("=== VITAJ NA FARME ===");
    Console.WriteLine("Dostupné príkazy: kup <krava>, predaj <krava>, posun <pocet>, vypis, exit\n");

    string vstup = "";
    while (vstup != "exit")
    {
        Console.Write("> ");
        // Načítame vstup a premeníme na malé písmená (aby fungovalo KUP aj kup)
        vstup = Console.ReadLine()?.ToLower() ?? "";

        if (string.IsNullOrWhiteSpace(vstup)) continue;

        // Rozdelíme vstup podľa medzery. 
        // casti[0] bude príkaz (napr. "kup")
        // casti[1] bude parameter (napr. "krava" alebo "10")
        string[] casti = vstup.Split(' ');
        string prikaz = casti[0];

        switch (prikaz)
        {
            case "kup":
                if (casti.Length > 1 && casti[1] == "krava")
                {
                    bool uspesne = farmar.KupStatok(new Dobytok(TypyDobytka.Krava));
                    if (uspesne) Console.WriteLine($"Kúpil si kravu. Zostatok: {farmar.Peniaze} €");
                    else Console.WriteLine("Nedostatok peňazí!");
                }
                else
                {
                    Console.WriteLine("Neznámy statok na kúpu. Skús napríklad: kup krava");
                }
                break;

            case "predaj":
                if (casti.Length > 1 && casti[1] == "krava")
                {
                    // 1. Vytvoríme si "vzor", aby metóda vedela, aký druh má hľadať
                    var vzorovaKrava = new Dobytok(TypyDobytka.Krava);

                    // 2. Nájdeme najstaršiu kravu na farme
                    Statok? najstarsiaKrava = Farma.Instance.NajdiNajstarsiStatok(vzorovaKrava);

                    // 3. Ak sme nejakú našli, predáme ju
                    if (najstarsiaKrava != null)
                    {
                        // Farmar si pripočíta peniaze a zavolá Farma.Instance.OdstranStatok(najstarsiaKrava)
                        farmar.PredajStatok(najstarsiaKrava);

                        Console.WriteLine($"Najstaršia krava (Vek: {najstarsiaKrava.Vek}) bola predaná. Zostatok: {farmar.Peniaze} €");
                    }
                    else
                    {
                        Console.WriteLine("Na farme nemáš žiadnu živú kravu na predaj.");
                    }
                }
                else
                {
                    Console.WriteLine("Neznámy statok na predaj. Skús napríklad: predaj krava");
                }
                break;

            case "posun":
                // Prevedieme textové číslo (napr. "10") na skutočný integer
                if (casti.Length > 1 && int.TryParse(casti[1], out int pocetTikov))
                {
                    Farma.Instance.PosunCas(pocetTikov);
                }
                else
                {
                    Console.WriteLine("Nezadali ste platné číslo pre čas. Skús napríklad: posun 10");
                }
                break;

            case "vypis":
                Console.WriteLine($"\n--- STAV FARMY (Peňaženka: {farmar.Peniaze} €) ---");
                if (Farma.Instance.Statky.Count == 0)
                {
                    Console.WriteLine("Farma je momentálne prázdna.");
                }
                else
                {
                    foreach (var s in Farma.Instance.Statky)
                    {
                        Console.WriteLine($"- {s.NazovObrazka} (Vek: {s.Vek}, Žije: {s.Zije})");
                    }
                }
                Console.WriteLine("------------------------------------\n");
                break;

            case "exit":
                Console.WriteLine("Ukončujem simuláciu farmy...");
                break;

            default:
                Console.WriteLine("Neznámy príkaz.");
                break;
        }
    }
}

