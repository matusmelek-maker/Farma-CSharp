using FarmSimulator.Core.Properties.Dobytok;
using FarmSimulator.Core.Properties.Produkt;
using FarmSimulator.Core.Models.Produkty;
using FarmSimulator.Core.Models.SpravaFarmy;
using FarmSimulator.Core.Models.Statky.Interfaces;
using System.Text.Json.Serialization; // Pridaný using pre JSON
using System.Collections.Generic;
using System;

namespace FarmSimulator.Core.Models.Statky.Zvierata.Dobytok
{
    public class Dobytok : Zviera, IProdukcne, ISpracovatelnyNaMeso
    {
        // Pripravené pre JSON (pridané JsonInclude a protected set)
        [JsonInclude] public TypDobytkaInfo Info { get; protected set; }

        private List<Produkt> vyprodukovaneProdukty = new();

        // BEZPARAMETRICKÝ KONŠTRUKTOR PRE JSON
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
                        Console.WriteLine($"[Farma] {this.Nazov} vyprodukoval: {novyProdukt.Nazov}");
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
                        Console.WriteLine($"[Farma] {Nazov} (Vek: {Vek}) prirodzene uhynul a vyprodukoval: {novyProdukt.Nazov}");
                    }
                }
            }
        }
    }
}