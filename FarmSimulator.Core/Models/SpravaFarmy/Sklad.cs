using FarmSimulator.Core.Models.Produkty;
using FarmSimulator.Core.Properties.Produkt;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FarmSimulator.Core.Models.SpravaFarmy
{
    public class Sklad
    {
        private static Sklad? _instance;
        public static Sklad Instance => _instance ??= new Sklad();

        public List<Produkt> UskladneneProdukty { get; private set; }

        // TOTO SME PRIDALI: Event, ktorý upozorní okno skladu, že sa zmenili počty
        public event Action? SkladSaZmenil;

        private Sklad()
        {
            UskladneneProdukty = new List<Produkt>();
        }

        public void PridajProdukt(Produkt produkt)
        {
            UskladneneProdukty.Add(produkt);
            SkladSaZmenil?.Invoke(); // Dáme vedieť UI
        }

        public bool OdoberProdukt(TypProduktuInfo info)
        {
            var produktNaOdobratie = UskladneneProdukty.FirstOrDefault(p => p.Info == info && p.Zije);

            if (produktNaOdobratie != null)
            {
                UskladneneProdukty.Remove(produktNaOdobratie);
                SkladSaZmenil?.Invoke(); // Dáme vedieť UI
                return true;
            }
            return false;
        }

        public int ZistiPocet(TypProduktuInfo info)
        {
            return UskladneneProdukty.Count(p => p.Info == info && p.Zije);
        }

        public void PosunCasVSklade()
        {
            var kopiaProduktov = UskladneneProdukty.ToList();

            foreach (var p in kopiaProduktov)
            {
                if (p.Zije)
                {
                    p.Tik();
                    p.VykonajAkcie();
                }
            }

            // Vymažeme všetky pokazené produkty
            int pocetZmazanych = UskladneneProdukty.RemoveAll(p => !p.Zije);

            // Ak sa niečo pokazilo a zmizlo, aktualizujeme UI
            if (pocetZmazanych > 0)
            {
                SkladSaZmenil?.Invoke();
            }
        }
    }
}