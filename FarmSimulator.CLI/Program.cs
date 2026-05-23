
using FarmSimulator.Core.Enums.Dobytok;
using FarmSimulator.Core.Enums.Zelenina;
using FarmSimulator.Core.Models;
using FarmSimulator.Core.Models.Statky;
using FarmSimulator.Core.Models.Statky.ProdukcneStatky.Rastliny.Zelenina;
using FarmSimulator.Core.Models.Statky.ProdukcneStatky.Zvierata.Dobytok;
using System.Runtime.CompilerServices;





// Spustenie samotnej slučky (ak používaš Top-level statements)
vytvorFarmara();


void vytvorFarmara()
{
    var farmar = new Farmar();
    
    // Pridáme nejaké statky na farmu
    Console.WriteLine("Vytváram farmára, penazi ma " + farmar.Peniaze);
    farmar.KupStatok(new Dobytok(TypyDobytka.Krava));
    
    farmar.KupStatok(new Zelenina(TypyZeleniny.Obilie));

    Farma.Instance.PosunCas(1);
    Farma.Instance.VypisStatky();
    Farma.Instance.PosunCas(1);
    Farma.Instance.VypisStatky();
    Farma.Instance.PosunCas(1);
    Farma.Instance.VypisStatky();
    Farma.Instance.PosunCas(1);
    Farma.Instance.VypisStatky();
    Farma.Instance.PosunCas(1);
    Farma.Instance.VypisStatky();
    Farma.Instance.PosunCas(1);
    Farma.Instance.VypisStatky();
    Farma.Instance.PosunCas(1);
    Farma.Instance.VypisStatky();
    Farma.Instance.PosunCas(1);
    Farma.Instance.VypisStatky();

    Console.WriteLine( "pocet statkov na farme: " + Farma.Instance.Statky.Count);

}


