using FarmSimulator.Core.Properties.Statok;


namespace FarmSimulator.Core.Properties.Produkt
{
    public record TypProduktuInfo(
        string Nazov,
        int KupnaCena,
        int PredajnaCena,
        KategoriaProduktu Kategoria,
        TypObchodnehoTovaru TypTovaru,
        int Zivotnost
    );

    public static class TypyProduktov
    {
        public static readonly TypProduktuInfo KravskeMlieko = new("Kravské mlieko", 5, 10, KategoriaProduktu.Opakovany, TypObchodnehoTovaru.Produkt, 50);
        public static readonly TypProduktuInfo KozieMlieko = new("Kozie mlieko", 7, 15, KategoriaProduktu.Opakovany, TypObchodnehoTovaru.Produkt, 50);
        public static readonly TypProduktuInfo Hovadzina = new("Hovädzina", 12, 25, KategoriaProduktu.Jednorazovy, TypObchodnehoTovaru.Produkt, 70);
        public static readonly TypProduktuInfo Hydina = new("Kuracina", 9, 18, KategoriaProduktu.Jednorazovy, TypObchodnehoTovaru.Produkt, 50);
        public static readonly TypProduktuInfo BravcoveMaso = new("Bravčové mäso", 11, 22, KategoriaProduktu.Jednorazovy, TypObchodnehoTovaru.Produkt, 60);
        public static readonly TypProduktuInfo SlepacieVajce = new("Slepačie vajce", 1, 3, KategoriaProduktu.Opakovany, TypObchodnehoTovaru.Produkt, 50);
        public static readonly TypProduktuInfo KacacieMaso = new("Kačacina", 3, 7, KategoriaProduktu.Jednorazovy, TypObchodnehoTovaru.Produkt, 60);
        public static readonly TypProduktuInfo Vlna = new("Vlna", 7, 15, KategoriaProduktu.Opakovany, TypObchodnehoTovaru.Produkt, 999);
        public static readonly TypProduktuInfo Hnoj = new("Hnoj", 2, 1, KategoriaProduktu.Opakovany, TypObchodnehoTovaru.Produkt, 999);

        public static readonly TypProduktuInfo Slivka = new("Slivka", 2, 5, KategoriaProduktu.Opakovany, TypObchodnehoTovaru.Produkt, 50);
        public static readonly TypProduktuInfo Citron = new("Citrón", 5, 10, KategoriaProduktu.Opakovany, TypObchodnehoTovaru.Produkt, 50);
        public static readonly TypProduktuInfo Jahody = new("Jahody", 3, 7, KategoriaProduktu.Jednorazovy, TypObchodnehoTovaru.Produkt, 20);

        public static readonly TypProduktuInfo Mrkva = new("Mrkva", 1, 2, KategoriaProduktu.Jednorazovy, TypObchodnehoTovaru.Krmivo, 50);
        public static readonly TypProduktuInfo Kukurica = new("Kukurica", 2, 4, KategoriaProduktu.Jednorazovy, TypObchodnehoTovaru.Krmivo, 60);
        public static readonly TypProduktuInfo Paradajky = new("Paradajky", 3, 6, KategoriaProduktu.Jednorazovy, TypObchodnehoTovaru.Krmivo, 50);
        public static readonly TypProduktuInfo Obilie = new("Obilie", 2, 4, KategoriaProduktu.Jednorazovy, TypObchodnehoTovaru.Krmivo, 100);

        public static IEnumerable<TypProduktuInfo> GetAll() =>
        [
            KravskeMlieko, KozieMlieko, Hovadzina, Hydina, BravcoveMaso,
            SlepacieVajce, KacacieMaso, Vlna, Hnoj, Slivka, Citron,
            Jahody, Mrkva, Kukurica, Paradajky, Obilie
        ];
    }
}