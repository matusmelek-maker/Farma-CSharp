using FarmSimulator.Core.Enums.Stromy;
using FarmSimulator.Core.Enums.Zelenina;
using FarmSimulator.Core.Models.SpravaFarmy;
using FarmSimulator.Core.Models.Statky.Rastliny.Stromy;
using FarmSimulator.Core.Models.Statky.Rastliny.Zelenina;
using System;


var paradajka = new Zelenina(TypyZeleniny.Paradajka);
// Predpokladajme, že Jabloň má DobaRastu = 10, ProdukcnyInterval = 4
var jablon = new Strom(TypyStromov.Slivka);

Farma.Instance.PridajStatok(paradajka);
Farma.Instance.PridajStatok(jablon);

Console.WriteLine($"Vysadená zelenina: {paradajka.Nazov}");
Console.WriteLine($"Vysadený strom: {jablon.Nazov}\n");

// 3. Spustíme simuláciu času (napr. 15 tikov/dní)
for (int i = 1; i <= 25; i++)
{
    Console.WriteLine($"--- Tik {i} ---");
    Farma.Instance.PosunCas(1); // Posuň čas o 1 deň

    Console.WriteLine($"vek stromu: {jablon.Vek} zobraz stadium stromu: {jablon.StadiumRastu}");
    Console.WriteLine($"vek zeleniny: {paradajka.Vek}");
    Console.WriteLine($"[Sklad] Aktuálny počet produktov: {Sklad.Instance.UskladneneProdukty.Count}");
    Console.WriteLine($"pocet statkov na farme: {Farma.Instance.Statky.Count}");
}

// Pozrieme sa, či niečo nepribudlo do skladu

Console.WriteLine($"[Sklad] Aktuálny počet produktov: {Sklad.Instance.UskladneneProdukty.Count}");
    
