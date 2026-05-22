using FarmSimulator.Core.Enums.Statok;
using FarmSimulator.Core.Models.Produkty;

namespace FarmSimulator.Core.Enums.Stromy
{   
    public record TypStromuInfo(
        string NazovObrazka,
        int KupnaCena,
        int PredajnaCena,
        TypProduktuInfo Produkt,
        TypObchodnehoTovaru TypObchodnehoTovaru,
        int ProdukcnyInterval,
        int Zivotnost,
        int DobaRastu
    );

    public static class TypyStromov
    {
        public static readonly TypStromuInfo Slivka = new("slivkov_strom", 5, 3, TypyProduktov.Slivka, TypObchodnehoTovaru.Strom, 12, 300, 18);
        public static readonly TypStromuInfo Jahoda = new("jahodovnik", 5, 2, TypyProduktov.Jahody, TypObchodnehoTovaru.Strom, 12, 300, 15);
        public static readonly TypStromuInfo Citron = new("citronovnik", 7, 4, TypyProduktov.Citron, TypObchodnehoTovaru.Strom, 12, 200, 20);

        public static IEnumerable<TypStromuInfo> GetAll() => [Slivka, Jahoda, Citron];
    }

}
