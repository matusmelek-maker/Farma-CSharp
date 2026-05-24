using FarmSimulator.Core.Properties.Statok;
using FarmSimulator.Core.Properties.Zvierata;
using FarmSimulator.Core.Properties.Produkt;

namespace FarmSimulator.Core.Properties.Dobytok
{
    // Dátový prepis pre jeden druh dobytka
    public record TypDobytkaInfo(
        string Nazov,
        int KupnaCena,
        int PredajnaCena,
        int KonstantaHlad,
        bool Pohlavie,
        int KonstantaReprodukcie,
        int Zivotnost,
        int IndexOhradky,
        DruhZvierata Druh,
        TypObchodnehoTovaru TypTovaru,
        TypProduktuInfo[] Produkty // Pole produktov, ktoré zviera produkuje
    );

    public static class TypyDobytka
    {
        public static readonly TypDobytkaInfo Kohut = new("kohut", 100, 50, 10, true, 15, 100, 0, DruhZvierata.Sliepka, TypObchodnehoTovaru.Dobytok, [TypyProduktov.Hydina]);
        public static readonly TypDobytkaInfo Sliepka = new("sliepka", 120, 60, 10, false, 30, 100, 0, DruhZvierata.Sliepka, TypObchodnehoTovaru.Dobytok, [TypyProduktov.SlepacieVajce, TypyProduktov.Hydina]);

        public static readonly TypDobytkaInfo Baran = new("baran", 400, 250, 15, true, 25, 300, 1, DruhZvierata.Ovca, TypObchodnehoTovaru.Dobytok, [TypyProduktov.Vlna]);
        public static readonly TypDobytkaInfo Ovca = new("ovca", 400, 250, 15, false, 50, 300, 1, DruhZvierata.Ovca, TypObchodnehoTovaru.Dobytok, [TypyProduktov.Vlna]);

        public static readonly TypDobytkaInfo Cap = new("cap", 350, 200, 17, true, 24, 280, 2, DruhZvierata.Koza, TypObchodnehoTovaru.Dobytok, []);
        public static readonly TypDobytkaInfo Koza = new("koza", 350, 200, 17, false, 45, 280, 2, DruhZvierata.Koza, TypObchodnehoTovaru.Dobytok, [TypyProduktov.KozieMlieko]);

        public static readonly TypDobytkaInfo Kanec = new("kanec", 500, 300, 20, true, 20, 250, 3, DruhZvierata.Prasa, TypObchodnehoTovaru.Dobytok, [TypyProduktov.BravcoveMaso]);
        public static readonly TypDobytkaInfo Prasa = new("prasa", 500, 300, 20, false, 40, 250, 3, DruhZvierata.Prasa, TypObchodnehoTovaru.Dobytok, [TypyProduktov.BravcoveMaso]);

        public static readonly TypDobytkaInfo Byk = new("byk", 800, 500, 16, true, 20, 400, 4, DruhZvierata.Krava, TypObchodnehoTovaru.Dobytok, [TypyProduktov.Hovadzina]);
        public static readonly TypDobytkaInfo Krava = new("krava", 800, 500, 16, false, 40, 10, 4, DruhZvierata.Krava, TypObchodnehoTovaru.Dobytok, [TypyProduktov.KravskeMlieko, TypyProduktov.Hovadzina]);

        public static IEnumerable<TypDobytkaInfo> GetAll() =>
            [Kohut, Sliepka, Baran, Ovca, Cap, Koza, Kanec, Prasa, Byk, Krava];
    }
}
