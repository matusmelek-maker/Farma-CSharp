using FarmSimulator.Core.Properties.Dobytok;
using FarmSimulator.Core.Properties.Produkt;
using FarmSimulator.Core.Models.Produkty;
using FarmSimulator.Core.Models.SpravaFarmy;
using FarmSimulator.Core.Models.Statky.Interfaces;

namespace FarmSimulator.Core.Models.Statky.Zvierata.Dobytok
{
    /// <summary>
    /// Trieda reprezentuje dobytok, ktorý je špecifickým typom zvieraťa.
    /// Spravuje cyklus produkcie (mlieko, vajcia, vlna) na základe stavu nasýtenia.
    /// </summary>
    public class Dobytok : Zviera, IProdukcne, ISpracovatelnyNaMeso
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
            this.SpracujNaMeso();
        }

        /// <summary>
        /// Ak je zviera najedené, vyprodukuje opakované produkty (napr. mlieko).
        /// </summary>
        public void Produkcia()
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

        public void SpracujNaMeso()
        {
            // 1. Skontrolujeme, či zviera zomrelo prirodzene na starobu.
            // (Ak zomrelo na hlad, Vek bude menší ako Zivotnost a nedá mäso).
            if (Vek >= Zivotnost)
            {
                // 2. Prejdeme všetky produkty priradené tomuto typu dobytka v katalógu
                foreach (var pInfo in Info.Produkty)
                {
                    // 3. Vyberieme LEN tie, ktoré sú jednorazové (Mäso, Koža...)
                    if (pInfo.Kategoria == KategoriaProduktu.Jednorazovy)
                    {
                        // Vytvoríme konkrétny objekt produktu
                        var novyProdukt = new Produkt(pInfo);

                        // Pridáme si ho do internej evidencie (použi názov zoznamu, aký máš v triede)
                        vyprodukovaneProdukty.Add(novyProdukt);

                        // 4. Mágia novej architektúry: Mäso ide PRIAMO do Skladu
                        Sklad.Instance.PridajProdukt(novyProdukt);

                        // Pomocný výpis do konzoly pre lepší prehľad v CLI
                        Console.WriteLine($"[Farma] {Nazov} (Vek: {Vek}) prirodzene uhynul a vyprodukoval: {novyProdukt.Nazov}");
                    }
                }
            }
        }
    }
}
