using FarmSimulator.Core.Models.SpravaFarmy;
using FarmSimulator.Core.Models.Statky.Interfaces;
using FarmSimulator.Core.Properties.Produkt;
using FarmSimulator.Core.Properties.Statok;
using System.Text.Json.Serialization;
using System;

namespace FarmSimulator.Core.Models.Statky.Rastliny
{
    public abstract class Rastlina : Statok, IPrijimajuciZiviny
    {
        // Pripravené na ukladanie do JSON
        [JsonInclude] public bool Pohnojene { get; protected set; }
        [JsonInclude] public int StadiumRastu { get; protected set; }
        [JsonInclude] public int PocetPlodov { get; protected set; }

        // JSON konštruktor
        [JsonConstructor]
        protected Rastlina() { }

        protected Rastlina(string nazovObrazka, int kupnaCena, int predajnaCena, bool zije, TypObchodnehoTovaru typ, int zivotnost)
            : base(nazovObrazka, kupnaCena, predajnaCena, zije, typ, zivotnost)
        {
            this.Pohnojene = false;
            this.StadiumRastu = 1;
            this.PocetPlodov = 0;
        }

        public override void VykonajAkcie()
        {
            this.Zomri();
            this.PrijmiZiviny();
        }

        public void PrijmiZiviny()
        {
            if (Pohnojene) return;

            if (Sklad.Instance.OdoberProdukt(TypyProduktov.Hnoj))
            {
                Pohnojene = true;
                PocetPlodov = 2;
                Console.WriteLine($"[Rastlina] {Nazov} sa úspešne pohnojila.");
            }
            else
            {
                PocetPlodov = 1;
            }
        }
    }
}