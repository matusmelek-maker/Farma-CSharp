using FarmSimulator.Core.Enums.Dobytok;
using FarmSimulator.Core.Enums.Produkt;
using FarmSimulator.Core.Models.Produkty;
using FarmSimulator.Core.Models.SpravaFarmy;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmSimulator.Core.Models.Statky.ProdukcneStatky.Zvierata.Dobytok
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
        {
            // 1. Pristupujeme priamo k property (bez isNajedene())
            if (this.Najedene)
            {
                // 2. Resetujeme stav nasýtenia
                this.Najedene = false;

                // 3. Namiesto klasického for-cyklu a indexu 'i' použijeme foreach
                // Toto rovno prechádza pole 'Produkty' definované v tvojom TypDobytkaInfo
                foreach (var pInfo in this.Info.Produkty)
                {
                    // 4. Overíme kategóriu (využívame Enum)
                    if (pInfo.Kategoria == KategoriaProduktu.Opakovany)
                    {
                        // Vytvoríme nový objekt produktu
                        var novyProdukt = new Produkt(pInfo);

                        // Pridáme do interného zoznamu dobytka
                        this.vyprodukovaneProdukty.Add(novyProdukt);

                        // 5. Nová C# architektúra: Pošleme celý objekt priamo do Skladu
                        Sklad.Instance.PridajProdukt(novyProdukt);

                        // Pomocný výpis do konzoly, aby sme videli, že zviera niečo vyprodukovalo
                        Console.WriteLine($"[Farma] {this.Nazov} vyprodukoval: {novyProdukt.Nazov}");
                    }
                }
            }
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
