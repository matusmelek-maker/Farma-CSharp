using FarmSimulator.Core.Enums.Stromy;
using FarmSimulator.Core.Properties.Dobytok;
using FarmSimulator.Core.Models.SpravaFarmy;
using FarmSimulator.Core.Models.Statky.Rastliny.Stromy;
using FarmSimulator.Core.Models.Statky.Zvierata.Dobytok;
using FarmSimulator.Core.SpracovanieDat;
using FarmSimulator.Core.Models.Produkty;
using System;
using System.Linq;

Console.WriteLine("=== KOMPLEXNÝ TEST UKLADANIA (ZVIERATÁ, RASTLINY A PRODUKTY) ===");

// 1. ČISTÝ ŠTÍT (Vyprázdnime staré dáta)
Sklad.Instance.UskladneneProdukty.Clear();
Farma.Instance.Statky.Clear();


// 2. NÁKUP STATKOV
var strom = new Strom(TypyStromov.Slivka);
var krava = new Dobytok(TypyDobytka.Krava);
var byk = new Dobytok(TypyDobytka.Byk);

Farma.Instance.PridajStatok(strom);
Farma.Instance.PridajStatok(krava);
Farma.Instance.PridajStatok(byk);

Console.WriteLine($"\n[Farma] Pridané: {strom.Nazov}, {krava.Nazov} (životnosť 4), {byk.Nazov}");

// 3. SIMULÁCIA ČASU
// Keďže má krava životnosť 4, za 5 tikov naisto zomrie a zanechá mäso v sklade
Console.WriteLine("\n--- Posúvam čas o 5 tikov ---");
Farma.Instance.PosunCas(25);

Console.WriteLine("\n=== STAV PRED ULOŽENÍM ===");
Console.WriteLine($"Počet zvierat a rastlín na farme: {Farma.Instance.Statky.Count} (Krava by už mala byť preč)");
Console.WriteLine($"Počet produktov v sklade: {Sklad.Instance.UskladneneProdukty.Count}");
foreach (var p in Sklad.Instance.UskladneneProdukty)
{
    Console.WriteLine($"- V sklade je: {p.Nazov} (Vek: {p.Vek})");
}

// 4. ULOŽENIE
SpravcaUlozenia.UlozHru();

// 5. HARD-RESET (Simulácia vypnutia a zapnutia aplikácie)
Console.WriteLine("\n--- RESETUJEM PAMÄŤ ---");
Farma.Instance.Statky.Clear();
Sklad.Instance.UskladneneProdukty.Clear();
Farmar.Instance.NastavPeniaze(0);

// 6. NAČÍTANIE ZO SÚBORU
SpravcaUlozenia.NacitajHru();

// 7. KONTROLA STAVU PO NAČÍTANÍ
Console.WriteLine("\n=== STAV PO NAČÍTANÍ ===");
Console.WriteLine($"Počet zvierat a rastlín na farme: {Farma.Instance.Statky.Count}");

// Skontrolujeme, či si načítané statky pamätajú svoje údaje
var nacitanyByk = Farma.Instance.Statky.OfType<Dobytok>().FirstOrDefault(d => d.Nazov == "byk");
if (nacitanyByk != null)
{
    Console.WriteLine($"[Farma] Býk sa načítal! Vek: {nacitanyByk.Vek}, Úroveň hladu: {nacitanyByk.UrovenHladu}");
}

var nacitanyStrom = Farma.Instance.Statky.OfType<Strom>().FirstOrDefault();
if (nacitanyStrom != null)
{
    Console.WriteLine($"[Farma] Strom sa načítal! Vek: {nacitanyStrom.Vek}, Štádium: {nacitanyStrom.StadiumRastu}");
}

Console.WriteLine($"Počet produktov v sklade: {Sklad.Instance.UskladneneProdukty.Count}");
foreach (var p in Sklad.Instance.UskladneneProdukty)
{
    Console.WriteLine($"- Načítaný produkt: {p.Nazov} (Vek: {p.Vek}, Info: {p.Info?.Nazov})");
}