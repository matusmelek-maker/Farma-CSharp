using FarmSimulator.Core.Models.Statky;
using FarmSimulator.Core.Properties.Statok;
using FarmSimulator.Core.Properties.Produkt;
using System.Text.Json.Serialization;

namespace FarmSimulator.Core.Models.Produkty
{
    public class Produkt : Statok
    {
        [JsonInclude] public TypProduktuInfo Info { get; protected set; }

        [JsonConstructor]
        protected Produkt() { }

        public Produkt(TypProduktuInfo info)
            : base(info.Nazov, info.KupnaCena, info.PredajnaCena, true, info.TypTovaru, info.Zivotnost)
        {
            Info = info;
        }

        public override void VykonajAkcie()
        {
            Zomri();
        }
    }
}