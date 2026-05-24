using FarmSimulator.Core.Properties.Statok;
using FarmSimulator.Core.Models.Produkty;
using FarmSimulator.Core.Properties.Produkt;

namespace FarmSimulator.Core.Enums.Zelenina
{
    public record TypZeleninyInfo(
        string NazovObrazka,
        int KupnaCena,
        int PredajnaCena,
        TypProduktuInfo Produkt,
        TypObchodnehoTovaru TypTovaru,
        int ProdukcnyInterval,
        int Zivotnost
    );

    public static class TypyZeleniny
    {
        public static readonly TypZeleninyInfo Paradajka = new("semiacka_paradajka", 10, 5, TypyProduktov.Paradajky, TypObchodnehoTovaru.Zelenina, 8, 70);
        public static readonly TypZeleninyInfo Mrkva = new("sadienka_mrkva", 6, 3, TypyProduktov.Mrkva, TypObchodnehoTovaru.Zelenina, 8, 70);
        public static readonly TypZeleninyInfo Kukurica = new("semiacka_kukurica", 4, 2, TypyProduktov.Kukurica, TypObchodnehoTovaru.Zelenina, 8, 70);
        public static readonly TypZeleninyInfo Obilie = new("semiacka_obilie", 4, 2, TypyProduktov.Obilie, TypObchodnehoTovaru.Zelenina, 8, 100);

        public static IEnumerable<TypZeleninyInfo> GetAll() => [Paradajka, Mrkva, Kukurica, Obilie];
    }
}
