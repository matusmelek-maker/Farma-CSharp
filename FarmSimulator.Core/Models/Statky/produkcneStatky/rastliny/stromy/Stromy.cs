using FarmSimulator.Core.Enums.statok;
using FarmSimulator.Core.Enums.stromy;
using FarmSimulator.Core.Models.Produkty;
using FarmSimulator.Core.Models.Statky.produkcneStatky.rastliny.stromy;
using System;
using System.Collections.Generic;
using System.Runtime.Intrinsics.X86;
using System.Text;


namespace FarmSimulator.Core.Models.Statky.produkcneStatky.rastliny.stromy
{
    public class Strom : Rastlina
    {
        public TypStromuInfo Info { get; }
        private List<Produkt> vyprodukovaneOvocie = new();
        private bool vyrasteny = false;
        private bool produkoval = false;

        // Udalosť pre UI, aby vedelo, že treba zmeniť obrázok (stádiá 1-4)
        public event Action<Strom>? OnStadiumZmenene;
        public event Action<Produkt>? OnOvocieVyprodukovane;

        public Strom(TypStromuInfo info)
            : base(info.NazovObrazka, info.KupnaCena, info.PredajnaCena, true, info.TypTovaru, info.Zivotnost)
        {
            this.Info = info;
        }

        public override void VykonajAkcie()
        {
            base.VykonajAkcie(); // Rieši starnutie a hnojenie z Rastlina.cs
            this.Produkcia();
            this.RastStromu();
        }

        public override void Produkcia()
        {
            // 1. Fáza: Samotný zber/produkcia plodov
            if (Vek != 0 && Vek % Info.ProdukcnyInterval == 0 && vyrasteny)
            {
                this.StadiumRastu = 4; // Obrázok stromu s ovocím
                OnStadiumZmenene?.Invoke(this);

                for (int i = 0; i < PocetPlodov; i++)
                {
                    var produkt = new Produkt(Info.Produkt);
                    vyprodukovaneOvocie.Add(produkt);
                    OnOvocieVyprodukovane?.Invoke(produkt);
                }
                this.produkoval = true;
            }
            // 2. Fáza: Reset po produkcii (mladý strom bez plodov)
            else if (vyrasteny && produkoval && (Vek % Info.ProdukcnyInterval < (0.33 * Info.DobaRastu)))
            {
                this.StadiumRastu = 2;
                OnStadiumZmenene?.Invoke(this);
                this.produkoval = false;
            }
            // 3. Fáza: Strom začína znova nahadzovať kvety/plody
            else if (vyrasteny && (Vek % Info.ProdukcnyInterval >= (0.33 * Info.DobaRastu)) && (Vek % Info.ProdukcnyInterval < (0.66 * Info.DobaRastu)))
            {
                this.StadiumRastu = 3;
                OnStadiumZmenene?.Invoke(this);
            }
        }

        private void RastStromu()
        {
            if (vyrasteny) return;

            if (Vek >= (0.33 * Info.DobaRastu) && Vek < (0.66 * Info.DobaRastu))
            {
                this.StadiumRastu = 2;
                OnStadiumZmenene?.Invoke(this);
            }
            else if (Vek >= (0.66 * Info.DobaRastu))
            {
                this.StadiumRastu = 3;
                OnStadiumZmenene?.Invoke(this);
            }

            if (Vek >= Info.DobaRastu)
            {
                this.vyrasteny = true;
            }
        }

        public List<Produkt> GetOvocieNaPridanie()
        {
            var result = new List<Produkt>(vyprodukovaneOvocie);
            vyprodukovaneOvocie.Clear();
            return result;
        }
    }
}


