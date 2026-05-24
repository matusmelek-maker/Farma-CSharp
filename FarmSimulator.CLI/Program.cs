using FarmSimulator.Core.Models.SpravaFarmy;
using FarmSimulator.Core.Models.Ludia;
using FarmSimulator.Core.Properties.AtrakcneZviera;
using FarmSimulator.Core.Models.Statky.Zvierata.AtrakcneZviera;
using System;

Console.WriteLine("--- TEST ATRAKCIE A ZISKU ---");

// 1. Pripravíme si farmára a zákazníka
var farmar = Farmar.Instance;
var zakaznik = SpravcaLudi.Instance.Clovek;

Console.WriteLine($"Počiatočný stav peňazí: {farmar.Peniaze} €");

// 2. Kúpime koňa (Atrakčné zviera)
var konInfo = TypyAtrakcnychZvierat.Kon;
var mojKon = new AtrakcneZviera(konInfo);
Farma.Instance.PridajStatok(mojKon);

Console.WriteLine($"Kúpil som: {mojKon.Nazov} za {mojKon.KupnaCena} €");

// 3. Simulujeme, že zákazník práve niečo nakúpil a je spokojný
// Týmto simulujeme stav, kedy má atrakcia čo "zinkasovať"
zakaznik.NastavSpokojnost(true);
Console.WriteLine("Zákazník je teraz spokojný a pripravený na atrakciu.");

// 4. Simulujeme Tik (Farma posunie čas, zavolá VykonajAkcie -> Produkcia)
Console.WriteLine("\n--- Simulujem posun času (Tik) ---");
mojKon.VykonajAkcie(); // Toto vnútri zavolá Produkcia(), ktorá zoberie peniaze

// 5. Kontrola výsledku
Console.WriteLine($"\n[VÝSLEDOK TESTU]");
Console.WriteLine($"Konečný stav peňazí farmára: {farmar.Peniaze} €");
Console.WriteLine($"Je zákazník stále spokojný? {zakaznik.SpokojnySNakupom} (Malo by byť False)");