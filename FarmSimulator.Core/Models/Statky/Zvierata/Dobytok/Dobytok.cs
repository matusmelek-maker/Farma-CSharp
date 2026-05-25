using FarmSimulator.Core.Models.Produkty;
using FarmSimulator.Core.Models.SpravaFarmy;
using FarmSimulator.Core.Models.Statky.Interfaces;
using FarmSimulator.Core.Properties.Dobytok;
using FarmSimulator.Core.Properties.Produkt;
using System.Text.Json.Serialization;
using static System.Net.Mime.MediaTypeNames;

namespace FarmSimulator.Core.Models.Statky.Zvierata.Dobytok
{
    public class Dobytok : Zviera, IProdukcne, ISpracovatelnyNaMeso, IPohyblive
    {
        public event Action<int, int> OnPohyb;
        [JsonInclude] public TypDobytkaInfo Info { get; protected set; } = null!;
        public int Riadok { get; set; }
        public int Stlpec { get; set; }
        public int MinRiadok { get; set; }
        public int MaxRiadok { get; set; }
        public int MinStlpec { get; set; }
        public int MaxStlpec { get; set; }
        

        private List<Produkt> vyprodukovaneProdukty = new();

        [JsonConstructor]
        protected Dobytok() { }

        public Dobytok(TypDobytkaInfo info)
            : base(info.Nazov,
                   info.KupnaCena,
                   info.PredajnaCena,
                   info.TypTovaru,
                   info.KonstantaHlad,
                   info.Pohlavie,
                   info.KonstantaReprodukcie,
                   info.Zivotnost,
                   info.IndexOhradky,
                   info.Druh)
        {
            this.Info = info;
        }

        public override void VykonajAkcie()
        {
            base.VykonajAkcie();
            this.Produkcia();
            this.SpracujNaMeso();
            this.PohniSa(); 
        }

        public void Produkcia()
        {
            if (this.Najedene)
            {
                this.Najedene = false;

                foreach (var pInfo in this.Info.Produkty)
                {
                    if (pInfo.Kategoria == KategoriaProduktu.Opakovany)
                    {
                        var novyProdukt = new Produkt(pInfo);
                        this.vyprodukovaneProdukty.Add(novyProdukt);
                        Sklad.Instance.PridajProdukt(novyProdukt);
                    }
                }
            }
        }

        public List<Produkt> GetProduktyNaPridanie()
        {
            var result = new List<Produkt>(this.vyprodukovaneProdukty);
            this.vyprodukovaneProdukty.Clear();
            return result;
        }

        protected override Zviera VytvorKlon()
        {
            return new Dobytok(this.Info);
        }

        public void SpracujNaMeso()
        {
            if (Vek >= Zivotnost)
            {
                foreach (var pInfo in Info.Produkty)
                {
                    if (pInfo.Kategoria == KategoriaProduktu.Jednorazovy)
                    {
                        var novyProdukt = new Produkt(pInfo);
                        vyprodukovaneProdukty.Add(novyProdukt);
                        Sklad.Instance.PridajProdukt(novyProdukt);
                    }
                }
            }
        }

        public void PohniSa()
        {
            Random rng = new Random();

            // Skúsime urobiť krok o -1, 0 alebo +1
            int novyRiadok = Riadok + rng.Next(-1, 2);
            int novyStlpec = Stlpec + rng.Next(-1, 2);

            // Kontrola hraníc (Clamp)
            if (novyRiadok >= MinRiadok && novyRiadok <= MaxRiadok &&
                novyStlpec >= MinStlpec && novyStlpec <= MaxStlpec)
            {
                Riadok = novyRiadok;
                Stlpec = novyStlpec;

                // Event na posun v UI
                OnPohyb?.Invoke(Riadok, Stlpec);
            }

        }
    }
}