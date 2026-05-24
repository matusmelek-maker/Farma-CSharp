using FarmSimulator.Core.Enums.Dobytok;
using FarmSimulator.Core.Enums.Zelenina;
using FarmSimulator.Core.Models.SpravaFarmy;
using FarmSimulator.Core.Models.Ludia;
using FarmSimulator.Core.Models.Produkty;
using System;
using System.Linq;
using FarmSimulator.Core.Models.Statky.Zvierata;
using FarmSimulator.Core.Models.Statky.Zvierata.Dobytok;

VytvorFarmara();

void VytvorFarmara()
{
    Console.WriteLine("--- TEST HLADU A ROZMNOŽOVANIA ---");

    // 1. Kúpime "rodičov" - Kravu (samica) a Býka (samec)
    Farmar.Instance.KupStatok(new Dobytok(TypyDobytka.Krava));
    Farmar.Instance.KupStatok(new Dobytok(TypyDobytka.Byk));

    // 2. Kúpime dostatok KRMIVA (napr. Obilie), inak nám na 16. tiku obaja zomrú!
    for (int i = 0; i < 10; i++)
    {
        Farmar.Instance.KupStatok(new Produkt(TypyProduktov.Obilie));
    }

    Console.WriteLine("\n[POČIATOČNÝ STAV]");
    Console.WriteLine($"Počet zvierat na farme: {Farma.Instance.Statky.OfType<Zviera>().Count()}");
    Console.WriteLine($"Krmivo (Obilie) v sklade: {Sklad.Instance.ZistiPocet(TypyProduktov.Obilie)}");
    Console.WriteLine($"Hnoj v sklade: {Sklad.Instance.ZistiPocet(TypyProduktov.Hnoj)}");

    Farma.Instance.PosunCas(15);
    Console.WriteLine();

    Farma.Instance.PosunCas(1);

    Console.WriteLine($"Vek Kravy: {Farma.Instance.Statky[0].Vek}");
    Console.WriteLine($"Krmivo (Obilie) v sklade: {Sklad.Instance.ZistiPocet(TypyProduktov.Obilie)} (Malo klesnúť o 2!)");
    Console.WriteLine($"Hnoj v sklade: {Sklad.Instance.ZistiPocet(TypyProduktov.Hnoj)} (Malo stúpnuť o 2!)");

    // 4. FÁZA 2: Test Reprodukcie (Posun o ďalších 24 tikov, spolu budú mať vek 40)
    // Na tiku 20 bol Býk pripravený, ale Krava ešte nie. 
    // Na tiku 40 už budú obaja pripravení!
    Console.WriteLine("\n=== ČAKÁME ĎALŠÍCH 24 SEKÚND (TEST ROZMNOŽOVANIA) ===");
    Farma.Instance.PosunCas(24);

    Console.WriteLine("\n[STAV PO 40 TIKOCH]");

    var vsetkyZvierata = Farma.Instance.Statky.OfType<Zviera>().ToList();
    Console.WriteLine($"Počet zvierat na farme: {vsetkyZvierata.Count} (Mali by byť 3!)");

    // Vypíšeme si všetky zvieratá, aby sme videli novonarodené mláďa
    foreach (var z in vsetkyZvierata)
    {
        Console.WriteLine($"- Názov: {z.Nazov}, Vek: {z.Vek}, Pohlavie: {(z.Pohlavie ? "Samec" : "Samica")}");
    }
}