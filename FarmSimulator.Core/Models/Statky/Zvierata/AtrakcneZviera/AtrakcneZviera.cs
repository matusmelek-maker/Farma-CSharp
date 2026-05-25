using FarmSimulator.Core.Models.Ludia;
using FarmSimulator.Core.Models.SpravaFarmy;
using FarmSimulator.Core.Models.Statky.Interfaces;
using FarmSimulator.Core.Properties.AtrakcneZviera;
using System.Text.Json.Serialization;

namespace FarmSimulator.Core.Models.Statky.Zvierata.AtrakcneZviera
{
    public class AtrakcneZviera : Zviera, IProdukcne
    {
        [JsonInclude] public TypAtrakcnehoZvierataInfo TypInfo { get; protected set; } = null!;

        [JsonConstructor]
        protected AtrakcneZviera() { }

        public AtrakcneZviera(TypAtrakcnehoZvierataInfo info)
            : base(info.Nazov, info.KupnaCena, info.PredajnaCena, info.TypTovaru,
                   info.KonstantaHlad, info.Pohlavie, info.KonstantaReprodukcie,
                   info.Zivotnost, info.IndexOhradky, info.Druh)
        {
            this.TypInfo = info;
        }

        public override void VykonajAkcie()
        {
            base.VykonajAkcie();
            this.Produkcia();
        }

        public void Produkcia()
        {
            var zakaznik = SpravcaLudi.Instance.Clovek;

            if (zakaznik.SpokojnySNakupom)
            {
                Farmar.Instance.AktualizujPeniaze(this.TypInfo.CenaJazdy);
                zakaznik.NastavSpokojnost(false);
            }
        }

        protected override Zviera VytvorKlon()
        {
            return new AtrakcneZviera(this.TypInfo);
        }
    }
}