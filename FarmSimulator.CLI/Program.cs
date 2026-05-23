
using FarmSimulator.Core.Enums.Dobytok;
using FarmSimulator.Core.Enums.Zelenina;
using FarmSimulator.Core.Models.Farma;
using FarmSimulator.Core.Models.Produkty;
using FarmSimulator.Core.Models.Statky;
using FarmSimulator.Core.Models.Statky.ProdukcneStatky.Rastliny.Zelenina;
using FarmSimulator.Core.Models.Statky.ProdukcneStatky.Zvierata.Dobytok;
using System.Runtime.CompilerServices;





// Spustenie samotnej slučky (ak používaš Top-level statements)
vytvorFarmara();


void vytvorFarmara()
{
    var farmar = new Farmar();

    Console.WriteLine("Vytváram farmára, penazi ma " + farmar.Peniaze);

    // Nákup
    farmar.KupStatok(new Dobytok(TypyDobytka.Krava));
    farmar.KupStatok(new Produkt(TypyProduktov.Obilie));

    // Výpis PRED posunutím času
    Console.WriteLine("\n--- PRED POSUNUTÍM ČASU ---");
    Console.WriteLine("Vek kravy na farme: " + Farma.Instance.Statky[0].Vek);
    Console.WriteLine("Vek obilia v sklade: " + Sklad.Instance.UskladneneProdukty[0].Vek);

    // Posun času o 1 tik
    Farma.Instance.PosunCas(1);

    // Výpis PO posunutí času
    Console.WriteLine("\n--- PO POSUNUTÍ ČASU ---");
    Console.WriteLine("pocet produktov v sklade: " + Sklad.Instance.ZistiPocet(TypyProduktov.Obilie));
    Console.WriteLine("pocet statkov na farme: " + Farma.Instance.Statky.Count);

    // Tu sa overí, či funguje Tik() všade rovnako
    Console.WriteLine("Vek kravy na farme teraz je: " + Farma.Instance.Statky[0].Vek);
    Console.WriteLine("Vek obilia v sklade teraz je: " + Sklad.Instance.UskladneneProdukty[0].Vek);
}


