using FarmSimulator.Core.Models.Statky;
using FarmSimulator.Core.Properties.Statok;
using FarmSimulator.Core.Properties.Produkt;

namespace FarmSimulator.Core.Models.Produkty
{
    public class Produkt : Statok
    {
        // Držíme si referenciu na info, z ktorého bol produkt vytvorený
        public TypProduktuInfo Info { get; }

        public Produkt(TypProduktuInfo info)
            : base(info.Nazov, info.KupnaCena, info.PredajnaCena, true, info.TypTovaru, info.Zivotnost)
        {
            Info = info;
        }

        /// <summary>
        /// Implementácia akcie pre produkt. 
        /// V tvojom prípade ide hlavne o starnutie a kontrolu expirácie (zomretia).
        /// </summary>
        public override void VykonajAkcie()
        {
            // Voláme metódu zo základnej triedy Statok
            Zomri();
        }
    }
}