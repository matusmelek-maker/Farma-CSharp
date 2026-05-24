using FarmSimulator.Core.Models.Produkty;
using FarmSimulator.Core.Models.Statky;
using System.Collections.Generic;
using System.Linq;

namespace FarmSimulator.Core.Models.SpravaFarmy
{
    public class Sklad
    {
        private static Sklad? _instance;
        public static Sklad Instance => _instance ??= new Sklad();

        // Zoznam reálnych fyzických produktov, nie len čísel!
        public List<Produkt> UskladneneProdukty { get; private set; }

        private Sklad()
        {
            UskladneneProdukty = new List<Produkt>();
        }

        // Prijíma CELÝ objekt produktu, nie len jeho TypInfo
        public void PridajProdukt(Produkt produkt)
        {
            UskladneneProdukty.Add(produkt);
        }

        public bool OdoberProdukt(TypProduktuInfo info)
        {
            // Nájdeme prvý produkt tohto typu, ktorý ešte žije (nie je pokazený)
            var produktNaOdobratie = UskladneneProdukty.FirstOrDefault(p => p.Info == info && p.Zije);

            if (produktNaOdobratie != null)
            {
                UskladneneProdukty.Remove(produktNaOdobratie);
                return true;
            }
            return false;
        }

        public int ZistiPocet(TypProduktuInfo info)
        {
            // LINQ spočíta, koľko živých produktov daného typu máme
            return UskladneneProdukty.Count(p => p.Info == info && p.Zije);
        }

        // TÚTO METÓDU zavoláme z Farmy pri každom tiku!
        public void PosunCasVSklade()
        {
            var kopiaProduktov = UskladneneProdukty.ToList();

            foreach (var p in kopiaProduktov)
            {
                if (p.Zije)
                {
                    p.Tik(); // Aktualizuje vek, zdravie, atď.

                    p.VykonajAkcie();  // Skontroluje sa, či nezomrel (expiroval)
                }
            }

            // Automaticky vyhodíme zo skladu všetky zhnité/pokazené (mŕtve) produkty
            int povodnyPocet = UskladneneProdukty.Count;
            UskladneneProdukty.RemoveAll(p => !p.Zije);
            int pokazene = povodnyPocet - UskladneneProdukty.Count;

            if (pokazene > 0)
            {
                Console.WriteLine($"[Sklad] Zhnilo / Expirovalo {pokazene} produktov.");
            }
        }
    }
}