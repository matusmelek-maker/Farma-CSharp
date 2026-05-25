using FarmSimulator.Core.Properties.Stromy;
using FarmSimulator.Core.Models.Produkty;
using FarmSimulator.Core.Models.SpravaFarmy;
using FarmSimulator.Core.Models.Statky.Interfaces;
using System.Text.Json.Serialization;

namespace FarmSimulator.Core.Models.Statky.Rastliny.Stromy
{
    public class Strom : Rastlina, IProdukcne
    {
        public event Action<int> ZmenaStadiaRastu;
        [JsonInclude] public TypStromuInfo Info { get; protected set; } = null!;

        [JsonInclude] public bool Vyrasteny { get; protected set; }
        [JsonInclude] public bool Produkoval { get; protected set; }

        public event Action<Strom>? OnStadiumZmenene;
        public event Action<Produkt>? OnOvocieVyprodukovane;

        [JsonConstructor]
        protected Strom() { }

        public Strom(TypStromuInfo info)
            : base(info.Nazov, info.KupnaCena, info.PredajnaCena, true, info.TypObchodnehoTovaru, info.Zivotnost)
        {
            this.Info = info;
            this.Vyrasteny = false;
            this.Produkoval = false;
        }

        public override void VykonajAkcie()
        {
            base.VykonajAkcie();

            if (Zije)
            {
                this.RastStromu();
                this.Produkcia();
            }
        }

        private void ZmenStadium(int noveStadium)
        {
            if (this.StadiumRastu != noveStadium)
            {
                this.StadiumRastu = noveStadium;

                ZmenaStadiaRastu?.Invoke(noveStadium);
            }
        }

        private void RastStromu()
        {
            if (Vyrasteny) return;

            if (Vek >= (0.33 * Info.DobaRastu) && Vek < (0.66 * Info.DobaRastu))
            {
                ZmenStadium(2);
            }
            else if (Vek >= (0.66 * Info.DobaRastu) && Vek < Info.DobaRastu)
            {
                ZmenStadium(3);
            }
            else if (Vek >= Info.DobaRastu)
            {
                this.Vyrasteny = true;
            }
        }

        public void Produkcia()
        {
            if (!Vyrasteny) return;

            if (Vek % Info.ProdukcnyInterval == 0)
            {
                ZmenStadium(4);

                for (int i = 0; i < PocetPlodov; i++)
                {
                    var ovocie = new Produkt(Info.Produkt);
                    Sklad.Instance.PridajProdukt(ovocie);
                    OnOvocieVyprodukovane?.Invoke(ovocie);
                }

                this.Produkoval = true;
            }
            else if (Produkoval && (Vek % Info.ProdukcnyInterval < (0.33 * Info.ProdukcnyInterval)))
            {
                ZmenStadium(2);
                this.Produkoval = false;
            }
            else if ((Vek % Info.ProdukcnyInterval >= (0.33 * Info.ProdukcnyInterval)) &&
                     (Vek % Info.ProdukcnyInterval < (0.66 * Info.ProdukcnyInterval)))
            {
                ZmenStadium(3);
            }
        }
    }
}