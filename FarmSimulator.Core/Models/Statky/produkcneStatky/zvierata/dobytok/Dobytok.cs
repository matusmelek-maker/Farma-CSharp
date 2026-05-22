using FarmSimulator.Core.Enums.dobytok;
using FarmSimulator.Core.Enums.produkt;
using FarmSimulator.Core.Models.Produkty;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmSimulator.Core.Models.Statky.produkcneStatky.zvierata.dobytok
{
    /// <summary>
    /// Trieda reprezentuje dobytok, ktorý je špecifickým typom zvieraťa.
    /// Spravuje cyklus produkcie (mlieko, vajcia, vlna) na základe stavu nasýtenia.
    /// </summary>
    public class Dobytok : Zviera
    {
        // Property pre prístup k statickým dátam typu dobytka
        public TypDobytkaInfo Info { get; }

        // Zoznam vyprodukovaných produktov (nahrádza ArrayList)
        private List<Produkt> vyprodukovaneProdukty = new();

        public Dobytok(TypDobytkaInfo info)
            : base(info.NazovObrazka,
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

        /// <summary>
        /// Vykoná všetky akcie dobytka vrátane základných (hlad, vek, reprodukcia) a produkcie.
        /// </summary>
        public override void VykonajAkcie()
        {
            // Zavolá logiku zo Zviera.cs (Tik, NajedzSa, RozmnozSa, Zomri, SpracujNaMeso)
            base.VykonajAkcie();

            // Pridá špecifickú logiku produkcie pre dobytok
            this.Produkcia();
        }

        /// <summary>
        /// Ak je zviera najedené, vyprodukuje opakované produkty (napr. mlieko).
        /// </summary>
        public override void Produkcia()
        {/*
            // Používame property Najedene definovanú v triede Zviera
            if (this.Najedene)
            {
                // Resetujeme stav nasýtenia po produkcii (ako v tvojej Jave)
                this.Najedene = false;

                // Prechádzame všetky možné produkty, ktoré tento typ dobytka môže mať
                foreach (var pInfo in Info.Produkty)
                {
                    // Produkujeme len opakované produkty (vajcia, mlieko, vlna)
                    // Mäso sa rieši v base.SpracujNaMeso() pri úhyne
                    if (pInfo.Kategoria == KategoriaProduktu.Opakovany)
                    {
                        Produkt novyProdukt = new Produkt(pInfo);
                        this.vyprodukovaneProdukty.Add(novyProdukt);

                        // Pridanie do globálneho skladu
                        Sklad.Instance.ZmenPocetStatokSklad(novyProdukt, 1);
                    }
                }
            }*/
        }

        /// <summary>
        /// Vráti zoznam produktov pripravených na spracovanie v UI a vyprázdni interný zoznam.
        /// </summary>
        public List<Produkt> GetProduktyNaPridanie()
        {
            var result = new List<Produkt>(this.vyprodukovaneProdukty);
            this.vyprodukovaneProdukty.Clear();
            return result;
        }

        /// <summary>
        /// Implementácia abstraktnej metódy zo Zviera.cs pre potreby rozmnožovania.
        /// </summary>
        protected override Zviera VytvorKlon()
        {
            return new Dobytok(this.Info);
        }
    }
}
