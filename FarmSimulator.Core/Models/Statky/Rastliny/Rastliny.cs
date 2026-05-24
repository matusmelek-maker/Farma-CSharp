using FarmSimulator.Core.Models.Produkty;
using FarmSimulator.Core.Models.SpravaFarmy;
using FarmSimulator.Core.Properties.Produkt;
using FarmSimulator.Core.Properties.Statok;
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

        private string nazovObrazka;
        private int kupnaCena;
        private int predajnaCena;
        private bool v;
        private TypObchodnehoTovaru typTovaru;
        private int zivotnost;


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
            this.Zomri();
            this.PrijmiHnoj();
        }

        /// <summary>
        /// Pokiaľ rastlina nie je pohnojená, pokúsi sa nájsť hnoj v systéme.
        /// Ak je pohnojená, produkuje 2 plody, inak 1.
        /// </summary>
        public void PrijmiHnoj()
        {
            // Ak už je pohnojená, neriešime ďalej
            if (Pohnojene) return;

            // Sklad sa sám pozrie, či má Hnoj. Ak áno, rovno ho vymaže a vráti true.
            if (Sklad.Instance.OdoberProdukt(TypyProduktov.Hnoj))
            {
                Pohnojene = true;
                PocetPlodov = 2;
                Console.WriteLine($"[Rastlina] {Nazov} sa úspešne pohnojila.");
            }
            else
            {
                // Ak hnoj v sklade nebol, bude mať len 1 plod
                PocetPlodov = 1;
            }
        }

        /// <summary>
        /// Vráti zoznam spotrebovaných hnojív a vyčistí interný zoznam.
        /// </summary>
        

    }
}
