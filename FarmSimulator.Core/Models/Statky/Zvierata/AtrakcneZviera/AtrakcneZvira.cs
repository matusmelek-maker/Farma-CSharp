using FarmSimulator.Core.Enums.AtrakcneZviera;
using FarmSimulator.Core.Models.Statky.Interfaces;
using FarmSimulator.Core.Models.Statky.Zvierata;

namespace FarmSimulator.Core.Models.Statky.Zvierata.AtrakcneZviera
{
    public class AtrakcneZviera : Zviera, IProdukcne
    {
        public TypAtrakcnehoZvierataInfo TypInfo { get; }

        // Definujeme udalosť pre vygenerovanie zisku (namiesto priameho volania Farmára)
        public event Action<int>? OnZiskVygenerovany;

        public AtrakcneZviera(TypAtrakcnehoZvierataInfo info)
            : base(info.NazovObrazka, info.KupnaCena, info.PredajnaCena, info.TypTovaru,
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
        {/*
            // V C# používame Singleton pre SpravcaLudi (ak ho máš tak implementovaný)
            var clovek = SpravcaLudi.Instance.GetClovek();

            if (clovek != null && clovek.IsSpokojnySNakupom)
            {
                // NAMIESTO: Farmar.Instance.SetPeniaze(...)
                // Vyvoláme udalosť, že sme zarobili
                OnZiskVygenerovany?.Invoke(this.TypInfo.CenaJazdy);

                // Reset spokojnosti (podľa tvojej logiky)
                clovek.IsSpokojnySNakupom = false;
            }*/
        }

        protected override Zviera VytvorKlon()
        {
            return new AtrakcneZviera(this.TypInfo);
        }
    }
}
