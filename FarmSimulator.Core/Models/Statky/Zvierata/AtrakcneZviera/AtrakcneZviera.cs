using FarmSimulator.Core.Models.Ludia;
using FarmSimulator.Core.Models.SpravaFarmy;
using FarmSimulator.Core.Models.Statky.Interfaces;
using FarmSimulator.Core.Properties.AtrakcneZviera;
using System.Text.Json.Serialization;

namespace FarmSimulator.Core.Models.Statky.Zvierata.AtrakcneZviera
{
    public class AtrakcneZviera : Zviera, IProdukcne, IPohyblive
    {
        [JsonInclude] public TypAtrakcnehoZvierataInfo TypInfo { get; protected set; } = null!;
        public int AktualnePoleIndex { get; set; }
        public int Riadok { get; set; }
        public int Stlpec { get; set; }
        public int MinRiadok { get; set; }
        public int MaxRiadok { get; set; }
        public int MinStlpec { get; set; }
        public int MaxStlpec { get; set; }

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
            this.PohniSa();
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

        public void PohniSa()
        {
            throw new NotImplementedException();
        }
    }
}