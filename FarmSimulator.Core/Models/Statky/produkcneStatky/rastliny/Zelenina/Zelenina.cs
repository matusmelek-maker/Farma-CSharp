using FarmSimulator.Core.Enums.Zelenina;
using FarmSimulator.Core.Models.Produkty;

namespace FarmSimulator.Core.Models.Statky.ProdukcneStatky.Rastliny.Zelenina
{
    public class Zelenina : Rastlina
    {
        public TypZeleninyInfo Info { get; }
        private List<Produkt> vyprodukovanaZelenina = new();

        // Udalosti pre decoupling (oddelenie) od UI a Skladu
        public event Action<Zelenina>? OnStadiumZmenene;
        public event Action<Produkt>? OnZeleninaVyprodukovana;

        public Zelenina(TypZeleninyInfo info)
            : base(info.NazovObrazka, info.KupnaCena, info.PredajnaCena, true, info.TypTovaru, info.Zivotnost)
        {
            this.Info = info;
        }

        public override void VykonajAkcie()
        {
            base.VykonajAkcie(); // Rieši starnutie a hnojenie z Rastlina.cs
            this.Produkcia();
        }

        public override void Produkcia()
        {
            // 1. Kontrola zberu (zelenina je pripravená na zber)
            if (Vek != 0 && Vek % Info.ProdukcnyInterval == 0)
            {
                this.StadiumRastu = 4;
                OnStadiumZmenene?.Invoke(this);

                for (int i = 0; i < PocetPlodov; i++)
                {
                    var produkt = new Produkt(Info.Produkt);
                    vyprodukovanaZelenina.Add(produkt);
                    OnZeleninaVyprodukovana?.Invoke(produkt);
                }

                // Zelenina po zbere hynie (rozdiel oproti stromom)
                this.Zije = false;
                this.Zomri();
            }
            // 2. Fázy rastu (zelenina rastie fixne podľa veku 3 a 6)
            else if (Vek == 3)
            {
                this.StadiumRastu = 2;
                OnStadiumZmenene?.Invoke(this);
            }
            else if (Vek == 6)
            {
                this.StadiumRastu = 3;
                OnStadiumZmenene?.Invoke(this);
            }
        }

        public List<Produkt> GetZeleninaNaPridanie()
        {
            var result = new List<Produkt>(vyprodukovanaZelenina);
            vyprodukovanaZelenina.Clear();
            return result;
        }
    }
}
