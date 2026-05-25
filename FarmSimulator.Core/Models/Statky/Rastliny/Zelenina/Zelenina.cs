using FarmSimulator.Core.Enums.Zelenina;
using FarmSimulator.Core.Models.Produkty;
using FarmSimulator.Core.Models.SpravaFarmy;
using FarmSimulator.Core.Models.Statky.Interfaces;
using System.Text.Json.Serialization;
using System;

namespace FarmSimulator.Core.Models.Statky.Rastliny.Zelenina
{
    public class Zelenina : Rastlina, IProdukcne
    {
        [JsonInclude] public TypZeleninyInfo Info { get; protected set; } = null!;

        public event Action<Zelenina>? OnStadiumZmenene;
        public event Action<Produkt>? OnZeleninaVyprodukovana;

        // JSON konštruktor
        [JsonConstructor]
        protected Zelenina() { }

        public Zelenina(TypZeleninyInfo info)
            : base(info.NazovObrazka, info.KupnaCena, info.PredajnaCena, true, info.TypTovaru, info.Zivotnost)
        {
            this.Info = info;
        }

        public override void VykonajAkcie()
        {
            base.VykonajAkcie();

            if (Zije)
            {
                this.Produkcia();
            }
        }

        private void ZmenStadium(int noveStadium)
        {
            if (this.StadiumRastu != noveStadium)
            {
                this.StadiumRastu = noveStadium;
                OnStadiumZmenene?.Invoke(this);
            }
        }

        public void Produkcia()
        {
            if (Vek != 0 && Vek % Info.ProdukcnyInterval == 0)
            {
                ZmenStadium(4);

                for (int i = 0; i < PocetPlodov; i++)
                {
                    var produkt = new Produkt(Info.Produkt);
                    Sklad.Instance.PridajProdukt(produkt);
                    OnZeleninaVyprodukovana?.Invoke(produkt);
                }

                Console.WriteLine($"[Zelenina] {Nazov} bola zozbieraná (Plodov: {PocetPlodov}) a odumiera.");
                this.Zije = false;
            }
            else if (Vek == 3)
            {
                ZmenStadium(2);
            }
            else if (Vek == 6)
            {
                ZmenStadium(3);
            }
        }
    }
}