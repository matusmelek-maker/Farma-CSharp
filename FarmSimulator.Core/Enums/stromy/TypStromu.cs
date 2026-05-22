using FarmSimulator.Core.Enums.statok;
using FarmSimulator.Core.Models.Produkty;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmSimulator.Core.Enums.stromy
{
    public record TypStromuInfo(
        string NazovObrazka,
        int KupnaCena,
        int PredajnaCena,
        TypProduktuInfo Produkt,
        TypObchodnehoTovaru TypTovaru,
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
