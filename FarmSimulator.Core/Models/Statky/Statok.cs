using FarmSimulator.Core.Properties.Statok;
using System.Text.Json.Serialization;
using FarmSimulator.Core.Models.Statky.Zvierata.Dobytok;
using FarmSimulator.Core.Models.Statky.Zvierata.AtrakcneZviera;
using FarmSimulator.Core.Models.Statky.Rastliny.Stromy;
using FarmSimulator.Core.Models.Statky.Rastliny.Zelenina;
using FarmSimulator.Core.Models.Produkty;
using System;

namespace FarmSimulator.Core.Models.Statky
{
    [JsonDerivedType(typeof(Dobytok), typeDiscriminator: "Dobytok")]
    [JsonDerivedType(typeof(AtrakcneZviera), typeDiscriminator: "AtrakcneZviera")]
    [JsonDerivedType(typeof(Strom), typeDiscriminator: "Strom")]
    [JsonDerivedType(typeof(Zelenina), typeDiscriminator: "Zelenina")]
    [JsonDerivedType(typeof(Produkt), typeDiscriminator: "Produkt")]
    public abstract class Statok
    {
        [JsonInclude] public string Nazov { get; protected set; } = null!;
        [JsonInclude] public int KupnaCena { get; protected set; }
        [JsonInclude] public int PredajnaCena { get; protected set; }
        [JsonInclude] public int Vek { get; protected set; }
        [JsonInclude] public bool Zije { get; set; }
        [JsonInclude] public TypObchodnehoTovaru Typ { get; protected set; }
        [JsonInclude] public int Zivotnost { get; protected set; }

        // Hlavný event, ktorý počúva UI (MainWindow)
        public event Action? OnZomrel;

        [JsonConstructor]
        protected Statok() { }

        protected Statok(string nazovObrazka, int kupnaCena, int predajnaCena, bool zije, TypObchodnehoTovaru typ, int zivotnost)
        {
            Nazov = nazovObrazka;
            KupnaCena = kupnaCena;
            PredajnaCena = predajnaCena;
            Zije = zije;
            Typ = typ;
            Zivotnost = zivotnost;
            Vek = 0;
        }

        public void Tik()
        {
            if (Zije)
            {
                Vek++;
            }
        }

        public abstract void VykonajAkcie();

        // TÚTO METÓDU ZAVOLÁ ZELENINA, KEĎ VYPRODUKUJE PLODY
        public void OdkazZeSomZomrel()
        {
            Zije = false;
            OnZomrel?.Invoke();
        }

        public virtual bool Zomri()
        {
            if (Vek >= Zivotnost || !Zije)
            {
                Zije = false;
                OnZomrel?.Invoke();
                return true;
            }
            return false;
        }
    }
}