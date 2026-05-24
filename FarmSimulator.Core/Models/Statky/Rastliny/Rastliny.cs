using FarmSimulator.Core.Properties.Statok;
using FarmSimulator.Core.Models.Produkty;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmSimulator.Core.Models.Statky.Rastliny
{
    public abstract class Rastlina : Statok
    {
        // Vlastnosti rastliny
        public bool Pohnojene { get; private set; }
        public int StadiumRastu { get; set; }
        public int PocetPlodov { get; private set; }

        // Pomocný zoznam pre použitý hnoj (ako v Jave)
        private List<Produkt> pouzityHnoj = new();
        private string nazovObrazka;
        private int kupnaCena;
        private int predajnaCena;
        private bool v;
        private TypObchodnehoTovaru typTovaru;
        private int zivotnost;

        // Eventy pre odstrihnutie Skladu/Farmy
        public event Action<Produkt>? OnHnojSpotrebovany;

        protected Rastlina(string nazovObrazka, int kupnaCena, int predajnaCena, bool zije, TypObchodnehoTovaru typ, int zivotnost)
            : base(nazovObrazka, kupnaCena, predajnaCena, zije, typ, zivotnost)
        {
            this.Pohnojene = false;
            this.StadiumRastu = 1;
            this.PocetPlodov = 0;
            this.nazovObrazka = nazovObrazka;
        }

        /// <summary>
        /// Vykoná akcie rastliny – kontrola úmrtia a príjem hnojiva.
        /// </summary>
        public override void VykonajAkcie()
        {
            // base.Tik() by malo zvyšovať vek, zomri() kontroluje životnosť
            this.Zomri();
            this.PrijmiHnoj();
        }

        /// <summary>
        /// Pokiaľ rastlina nie je pohnojená, pokúsi sa nájsť hnoj v systéme.
        /// Ak je pohnojená, produkuje 2 plody, inak 1.
        /// </summary>
        public void PrijmiHnoj()
        {/*
            if (!this.Pohnojene)
            {
                // Hľadáme hnoj na farme (používame LINQ)
                var hnojVskladu = Farma.Instance.Statky
                    .OfType<Produkt>()
                    .FirstOrDefault(p => p.TypInfo.Typ == TypyProduktov.Hnoj.Typ && p.Zije);

                if (hnojVskladu != null)
                {
                    this.pouzityHnoj.Add(hnojVskladu);
                    hnojVskladu.Zomri(); // Hnoj sa "spotrebuje"
                    this.Pohnojene = true;

                    // NAMIESTO: Sklad.Instance.ZmenPocetStatokSklad(hnojVskladu, -1);
                    // Informujeme engine o spotrebe
                    OnHnojSpotrebovany?.Invoke(hnojVskladu);
                }

                // Nastavenie počtu plodov podľa hnojenia
                this.PocetPlodov = this.Pohnojene ? 2 : 1;
            }*/
        }

        /// <summary>
        /// Vráti zoznam spotrebovaných hnojív a vyčistí interný zoznam.
        /// </summary>
        public List<Produkt> GetPouzityHnoj()
        {
            var result = new List<Produkt>(this.pouzityHnoj);
            this.pouzityHnoj.Clear();
            return result;
        }

    }
}
