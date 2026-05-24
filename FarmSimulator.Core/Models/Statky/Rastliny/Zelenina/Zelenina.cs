using FarmSimulator.Core.Enums.Zelenina;
using FarmSimulator.Core.Models.Produkty;
using FarmSimulator.Core.Models.SpravaFarmy;
using FarmSimulator.Core.Models.Statky.Interfaces;
using System;

// Pridaj správny using pre tvoje TypZeleninyInfo (napr. Records namiesto Enums, ak si to menil)
// using FarmSimulator.Core.Records.Zelenina; 

namespace FarmSimulator.Core.Models.Statky.Rastliny.Zelenina
{
    public class Zelenina : Rastlina, IProdukcne
    {
        public TypZeleninyInfo Info { get; }

        // Udalosti pre UI
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

            // Zelenina produkuje a rastie len kým žije
            if (Zije)
            {
                this.Produkcia();
            }
        }

        // Pomocná metóda na bezpečnú zmenu štádia pre UI
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
            // 1. Kontrola zberu (zelenina je pripravená na zber)
            if (Vek != 0 && Vek % Info.ProdukcnyInterval == 0)
            {
                ZmenStadium(4); // Obrázok plne dozretej zeleniny

                // Vyprodukujeme zeleninu podľa počtu plodov (ovplyvnené hnojivom z Rastlina.cs)
                for (int i = 0; i < PocetPlodov; i++)
                {
                    // Predpokladám, že Info.Produkt odkazuje na TypProduktuInfo
                    var produkt = new Produkt(Info.Produkt);

                    // Odoslanie priamo do Skladu (žiadne ukladanie do lokálnych listov)
                    Sklad.Instance.PridajProdukt(produkt);

                    OnZeleninaVyprodukovana?.Invoke(produkt);
                }

                Console.WriteLine($"[Zelenina] {Nazov} bola zozbieraná (Plodov: {PocetPlodov}) a odumiera.");
                this.Zije = false;
                
            }
            // 2. Fázy rastu (zelenina rastie fixne podľa veku 3 a 6)
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