using FarmSimulator.Core.Properties.Produkt;
using FarmSimulator.Core.Properties.Statok;
using FarmSimulator.Core.Properties.Zvierata;
using FarmSimulator.Core.Models.Produkty;
using FarmSimulator.Core.Models.SpravaFarmy;
using FarmSimulator.Core.Models.Statky.Interfaces;
using System.Text.Json.Serialization;

namespace FarmSimulator.Core.Models.Statky.Zvierata
{
    public abstract class Zviera : Statok, IPrijimajuciZiviny
    {
        public event Action<Zviera> OnNarodiloSaZviera;
        [JsonInclude] public int KonstantaHlad { get; protected set; }
        [JsonInclude] public bool Pohlavie { get; protected set; }
        [JsonInclude] public int KonstantaReprodukcie { get; protected set; }
        [JsonInclude] public int IndexOhradky { get; protected set; }
        [JsonInclude] public DruhZvierata Druh { get; protected set; }

        [JsonInclude] public bool PripravenyNaReprodukciu { get; set; }
        [JsonInclude] public bool Najedene { get; set; }
        [JsonInclude] public int UrovenHladu { get; protected set; }

        private List<Zviera> pridane = new();
        private List<Produkt> zeleninaZjedena = new();
        private List<Produkt> pridaneProdukty = new();

        private Random random = new Random();

        [JsonConstructor]
        protected Zviera() { }

        protected Zviera(string nazovObrazka, int kupnaCena, int predajnaCena, TypObchodnehoTovaru typ,
                         int konstantaHlad, bool pohlavie, int konstantaReprodukcie, int zivotnost,
                         int indexOhradky, DruhZvierata druh)
            : base(nazovObrazka, kupnaCena, predajnaCena, true, typ, zivotnost)
        {
            this.KonstantaHlad = konstantaHlad;
            this.Pohlavie = pohlavie;
            this.KonstantaReprodukcie = konstantaReprodukcie;
            this.IndexOhradky = indexOhradky;
            this.Druh = druh;
            this.PripravenyNaReprodukciu = false;
            this.Najedene = false;
            this.UrovenHladu = 0;

        }

         
        

        public override void VykonajAkcie()
        {
            RozmnozSa();
            PrijmiZiviny();
            Zomri();
        }

        public void RozmnozSa()
        {
            if (Vek != 0 && Vek % KonstantaReprodukcie == 0)
            {
                PripravenyNaReprodukciu = true;
            }

            if (PripravenyNaReprodukciu && Pohlavie)
            {
                var vhodneSamice = Farma.Instance.Statky
                    .OfType<Zviera>()
                    .Where(z => !z.Pohlavie && z.PripravenyNaReprodukciu && z.Druh == this.Druh)
                    .ToList();

                foreach (var samica in vhodneSamice)
                {
                    Zviera noveZviera = (random.Next(100) < 50) ? this.VytvorKlon() : samica.VytvorKlon();

                    if (noveZviera != null)
                    {
                        Farma.Instance.PridajStatok(noveZviera);
                        OnNarodiloSaZviera?.Invoke(noveZviera);
                        this.PripravenyNaReprodukciu = false;
                        samica.PripravenyNaReprodukciu = false;
                        this.pridane.Add(noveZviera);
                        break;
                    }
                }
            }
        }

        public void PrijmiZiviny()
        {
            if (Vek != 0 && Vek % KonstantaHlad == 0)
            {
                if (!Najedene)
                {
                    var krmivo = Sklad.Instance.UskladneneProdukty
                        .FirstOrDefault(p => p.Info.TypTovaru == TypObchodnehoTovaru.Krmivo && p.Zije);

                    if (krmivo != null)
                    {
                        zeleninaZjedena.Add(krmivo);
                        krmivo.Zije = false;
                        Najedene = true;

                        if (UrovenHladu > 0)
                        {
                            UrovenHladu--;
                        }

                        var hnoj = new Produkt(TypyProduktov.Hnoj);
                        pridaneProdukty.Add(hnoj);
                        Sklad.Instance.PridajProdukt(hnoj);
                    }
                }

                if (!Najedene)
                {
                    UrovenHladu++;
                    if (UrovenHladu >= 3)
                    {
                        Zije = false;
                        Zomri();
                    }
                }
            }
        }

        protected abstract Zviera VytvorKlon();

        public List<Zviera> GetPridaneZvierata() { var res = new List<Zviera>(pridane); pridane.Clear(); return res; }
        public List<Produkt> GetZjedenuZeleninu() { var res = new List<Produkt>(zeleninaZjedena); zeleninaZjedena.Clear(); return res; }
        public List<Produkt> GetPridaneProdukty() { var res = new List<Produkt>(pridaneProdukty); pridaneProdukty.Clear(); return res; }
    }
}