using FarmSimulator.Core.Enums.Stromy;
using FarmSimulator.Core.Models.Produkty;
using FarmSimulator.Core.Models.SpravaFarmy;
using FarmSimulator.Core.Models.Statky.Interfaces;
using System;

// Pridaj správne usingy pre tvoje TypStromuInfo
// using FarmSimulator.Core.Records.Stromy; 

namespace FarmSimulator.Core.Models.Statky.Rastliny.Stromy
{
    public class Strom : Rastlina, IProdukcne
    {
        public TypStromuInfo Info { get; }

        private bool vyrasteny = false;
        private bool produkoval = false;

        // Udalosti pre UI
        public event Action<Strom>? OnStadiumZmenene;
        public event Action<Produkt>? OnOvocieVyprodukovane;

        public Strom(TypStromuInfo info)
            : base(info.NazovObrazka, info.KupnaCena, info.PredajnaCena, true, info.TypObchodnehoTovaru, info.Zivotnost)
        {
            this.Info = info;
        }

        public override void VykonajAkcie()
        {
            // Zavolá logiku z Rastliny (Zomri, PrijmiZiviny/Hnoj)
            base.VykonajAkcie();

            if (Zije)
            {
                this.RastStromu();
                this.Produkcia();
            }
        }

        // --- POMOCNÁ METÓDA PRE UI ---
        // Zabezpečí, že event sa odpáli iba raz pri prechode z 1->2, 2->3 atď.
        private void ZmenStadium(int noveStadium)
        {
            if (this.StadiumRastu != noveStadium)
            {
                this.StadiumRastu = noveStadium;
                OnStadiumZmenene?.Invoke(this);
            }
        }

        private void RastStromu()
        {
            if (vyrasteny) return; // Ak už vyrástol, túto metódu ignorujeme

            // Od 0 do 33% je štádium 1 (nastavené v Rastlina.cs)
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
                this.vyrasteny = true;
            }
        }

        public void Produkcia()
        {
            // Strom rodí, len ak už vyrástol
            if (!vyrasteny) return;

            // 1. Fáza: Samotný zber/produkcia plodov (cyklus)
            if (Vek % Info.ProdukcnyInterval == 0)
            {
                ZmenStadium(4); // Obrázok stromu s ovocím

                // Vyprodukujeme ovocie podľa počtu plodov (ovplyvnené hnojivom z Rastlina.cs)
                for (int i = 0; i < PocetPlodov; i++)
                {
                    // Predpokladám, že Info.Produkt odkazuje na TypProduktuInfo
                    var ovocie = new Produkt(Info.Produkt);

                    // Pošleme priamo do Skladu!
                    Sklad.Instance.PridajProdukt(ovocie);

                    OnOvocieVyprodukovane?.Invoke(ovocie);
                }

                this.produkoval = true;
                Console.WriteLine($"[Strom] {Nazov} vyprodukoval ovocie! (Plodov: {PocetPlodov})");
            }
            // 2. Fáza: Reset po produkcii (strom bez plodov)
            else if (produkoval && (Vek % Info.ProdukcnyInterval < (0.33 * Info.ProdukcnyInterval)))
            {
                ZmenStadium(2);
                this.produkoval = false;
            }
            // 3. Fáza: Strom začína znova nahadzovať kvety/puky
            else if ((Vek % Info.ProdukcnyInterval >= (0.33 * Info.ProdukcnyInterval)) &&
                     (Vek % Info.ProdukcnyInterval < (0.66 * Info.ProdukcnyInterval)))
            {
                ZmenStadium(3);
            }
        }
    }
}