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

    Farmar.Instance.KupStatok(new Dobytok(TypyDobytka.Krava));

    Farma.Instance.PosunCas(10);

    Farma.Instance.VypisStatky();
    Console.WriteLine($"Počet hovädziny v sklade: {Sklad.Instance.ZistiPocet(TypyProduktov.Hovadzina)}");

}