using FarmSimulator.Core.Enums.Dobytok;
using FarmSimulator.Core.Enums.Zelenina;
using FarmSimulator.Core.Models;
using FarmSimulator.Core.Models.Farma;
using FarmSimulator.Core.Models.Ludia;
using FarmSimulator.Core.Models.Produkty;
using FarmSimulator.Core.Models.Statky.ProdukcneStatky.Rastliny.Zelenina;
using FarmSimulator.Core.Models.Statky.ProdukcneStatky.Zvierata.Dobytok;
using System;
using System.Linq; // Pridané pre FirstOrDefault

// Spustenie samotnej slučky (Top-level statements)
VytvorFarmara();

void VytvorFarmara()
{
    // 1. Pristupujeme k Farmárovi cez Singleton
    Console.WriteLine("Vytváram farmára, peňazí má: " + Farmar.Instance.Peniaze + " €");

    // 2. Kúpime jednu kravu na farmu
    Farmar.Instance.KupStatok(new Dobytok(TypyDobytka.Krava));

    // Kúpime do skladu 10x mlieko a 10x obilie, nech má zákazník čo kupovať
    for (int i = 0; i < 10; i++)
    {
        Farmar.Instance.KupStatok(new Produkt(TypyProduktov.KravskeMlieko));
        Farmar.Instance.KupStatok(new Produkt(TypyProduktov.Obilie));
    }

    // Výpis PRED posunutím času
    Console.WriteLine("\n--- PRED POSUNUTÍM ČASU ---");
    Console.WriteLine("Vek kravy na farme: " + Farma.Instance.Statky[0].Vek);
    Console.WriteLine("Vek prvého produktu v sklade: " + Sklad.Instance.UskladneneProdukty[0].Vek);
    Console.WriteLine("Peňaženka farmára po našich nákupoch: " + Farmar.Instance.Peniaze + " €");

    // Zobrazenie prázdneho lístka
    Console.WriteLine("\n" + SpravcaLudi.Instance.Clovek.ZiskajNakupnyListokText());

    // 3. Posun času o 15 tikov! Toto spustí zákazníkov nákup v SpravcaLudi.
    Console.WriteLine("\n=== ČAKÁME 15 SEKÚND (TIKOV) NA ZÁKAZNÍKA ===");
    Farma.Instance.PosunCas(15);

    // Výpis PO posunutí času a nákupe zákazníka
    Console.WriteLine("\n--- PO POSUNUTÍ ČASU ---");
    Console.WriteLine("Vek kravy na farme teraz je: " + Farma.Instance.Statky[0].Vek);

    // Sklad mohol zákazník vykúpiť, preto použijeme bezpečnejší výpis
    var prvyProdukt = Sklad.Instance.UskladneneProdukty.FirstOrDefault();
    if (prvyProdukt != null)
    {
        Console.WriteLine("Vek nejakého zvyšného produktu v sklade je: " + prvyProdukt.Vek);
    }

    // 4. Skontrolujeme, či zákazník nakúpil, koľko nám zaplatil a či je spokojný
    Console.WriteLine("\n" + SpravcaLudi.Instance.Clovek.ZiskajNakupnyListokText());
    Console.WriteLine($"Spokojnosť zákazníka s nákupom: {SpravcaLudi.Instance.Clovek.SpokojnySNakupom}");
    Console.WriteLine("Konečná peňaženka farmára: " + Farmar.Instance.Peniaze + " €");
}