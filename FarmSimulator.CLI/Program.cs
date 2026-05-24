using FarmSimulator.Core.Enums.Zelenina;
using FarmSimulator.Core.Models.Produkty;
using FarmSimulator.Core.Models.SpravaFarmy;
using FarmSimulator.Core.Models.Statky.Rastliny.Zelenina;
using FarmSimulator.Core.Properties.Produkt;
using System;
// Pridaj si tu tvoje usingy pre Produkt, TypyProduktov, Zeleninu atď.

Console.WriteLine("=== TEST SKLADU A HNOJENIA RASTLINY ===");

// 1. Vyčistíme sklad (pre istotu, aby sme mali čistý štít pre test)
Sklad.Instance.UskladneneProdukty.Clear();

// 2. Vytvoríme a pridáme 2 kusy Hnoja do skladu
// (Použi metódu na pridanie, akú máš v Sklade. Ak nemáš, daj len Add)
var hnoj1 = new Produkt(TypyProduktov.Hnoj);
var hnoj2 = new Produkt(TypyProduktov.Hnoj);

Sklad.Instance.UskladneneProdukty.Add(hnoj1);
Sklad.Instance.UskladneneProdukty.Add(hnoj2);

Console.WriteLine($"[Sklad] Počiatočný stav hnoja: {Sklad.Instance.UskladneneProdukty.Count} ks");

// 3. Vytvoríme rastlinu (Zeleninu - napr. Mrkvu alebo čo máš v systéme definované)
// (Uprav si parametre podľa toho, ako máš urobený konštruktor pre Zeleninu)
var mojaRastlina = new Zelenina(TypyZeleniny.Mrkva);

Console.WriteLine($"\n[Rastlina] Vytvorená: {mojaRastlina.Nazov}");
Console.WriteLine($"[Rastlina] Je pohnojená pred akciou? {mojaRastlina.Pohnojene}");
Console.WriteLine($"[Rastlina] Počet plodov pred akciou: {mojaRastlina.PocetPlodov}");

// 4. Vykonáme hnojenie
Console.WriteLine("\n--- Spúšťam PrijmiHnoj() ---");
mojaRastlina.PrijmiZiviny();

// 5. Výsledky testu
Console.WriteLine("\n=== VÝSLEDKY TESTU ===");
Console.WriteLine($"[Rastlina] Je pohnojená po akcii? {mojaRastlina.Pohnojene} (Malo by byť True)");
Console.WriteLine($"[Rastlina] Počet plodov po akcii: {mojaRastlina.PocetPlodov} (Malo by byť 2)");
Console.WriteLine($"[Sklad] Zostatok hnoja: {Sklad.Instance.UskladneneProdukty.Count} ks (Malo by byť 1)");

// 6. Otestujeme, či si nevezme hnoj, keď už je pohnojená
Console.WriteLine("\n--- Spúšťam PrijmiHnoj() druhýkrát ---");
mojaRastlina.PrijmiZiviny();
Console.WriteLine($"[Sklad] Zostatok hnoja: {Sklad.Instance.UskladneneProdukty.Count} ks (Malo by ostať 1, lebo rastlina už hnoj má)");