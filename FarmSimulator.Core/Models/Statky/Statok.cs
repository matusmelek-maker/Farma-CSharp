using FarmSimulator.Core.Properties.Statok;
using System;
using System.Text.Json.Serialization;

// Zahrnuté usingy, aby Statok poznal svojich potomkov pre JSON
using FarmSimulator.Core.Models.Statky.Zvierata.Dobytok;
using FarmSimulator.Core.Models.Statky.Zvierata.AtrakcneZviera;
using FarmSimulator.Core.Models.Statky.Rastliny.Stromy;
using FarmSimulator.Core.Models.Statky.Rastliny.Zelenina;
using FarmSimulator.Core.Models.Produkty;

namespace FarmSimulator.Core.Models.Statky
{
    /// <summary>
    /// Abstraktná trieda Statok predstavuje základnú entitu obchodného tovaru.
    /// </summary>

    // Tieto atribúty povedia JSONu, aké objekty má pri načítavaní očakávať a ako ich označiť
    [JsonDerivedType(typeof(Dobytok), typeDiscriminator: "Dobytok")]
    [JsonDerivedType(typeof(AtrakcneZviera), typeDiscriminator: "AtrakcneZviera")]
    [JsonDerivedType(typeof(Strom), typeDiscriminator: "Strom")]
    [JsonDerivedType(typeof(Zelenina), typeDiscriminator: "Zelenina")]
    [JsonDerivedType(typeof(Produkt), typeDiscriminator: "Produkt")]
    public abstract class Statok
    {
        // Vlastnosti musia mať aspoň 'protected set' a [JsonInclude], 
        // aby ich JsonSerializer vedel pri načítavaní zo súboru vyplniť.
        [JsonInclude] public string Nazov { get; protected set; }
        [JsonInclude] public int KupnaCena { get; protected set; }
        [JsonInclude] public int PredajnaCena { get; protected set; }
        [JsonInclude] public int Vek { get; protected set; }
        [JsonInclude] public bool Zije { get; set; }
        [JsonInclude] public TypObchodnehoTovaru Typ { get; protected set; }
        [JsonInclude] public int Zivotnost { get; protected set; }

        public event Action? OnZomrel;

        // BEZPARAMETRICKÝ KONŠTRUKTOR PRE JSON
        // Systém ho nutne potrebuje na vytvorenie prázdneho objektu pred jeho naplnením dátami zo súboru
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

        /// <summary>
        /// Táto metóda nahrádza Timer. Bude volaná zvonku (z triedy Farma).
        /// </summary>
        public void Tik()
        {
            if (Zije)
            {
                Vek++;
            }
        }

        /// <summary>
        /// Abstraktná metóda pre špecifické akcie statku.
        /// </summary>
        public abstract void VykonajAkcie();

        /// <summary>
        /// Skontroluje stav a ošetrí úhyn statku.
        /// </summary>
        /// <returns>True, ak statok práve zomrel.</returns>
        public virtual bool Zomri()
        {
            // V C# používame vlastnosti priamo (Vek namiesto getVek())
            if (Vek >= Zivotnost || !Zije)
            {
                Zije = false;

                // POZNÁMKA: Volanie Sklad.Instance.ZmenPocetStatokSklad(this, -1) 
                // tu zatiaľ nedávame, kým nenaprogramujeme Sklad v Core.
                OnZomrel?.Invoke();
                return true;
            }
            return false;
        }
    }
}