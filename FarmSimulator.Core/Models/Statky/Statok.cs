using FarmSimulator.Core.Enums.Statok;
using System;

namespace FarmSimulator.Core.Models.Statky
{
    /// <summary>
    /// Abstraktná trieda Statok predstavuje základnú entitu obchodného tovaru.
    /// </summary>
    public abstract class Statok
    {
        // Properties (Vlastnosti) nahrádzajú private polia a gettery
        public string NazovObrazka { get; }
        public int KupnaCena { get; }
        public int PredajnaCena { get; }
        public int Vek { get; protected set; } // protected set umožní potomkom meniť vek
        public bool Zije { get; set; }
        public TypObchodnehoTovaru Typ { get; }
        public int Zivotnost { get; }

        protected Statok(string nazovObrazka, int kupnaCena, int predajnaCena, bool zije, TypObchodnehoTovaru typ, int zivotnost)
        {
            NazovObrazka = nazovObrazka;
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
            if (Vek >= Zivotnost)
            {
                Zije = false;

                // POZNÁMKA: Volanie Sklad.Instance.ZmenPocetStatokSklad(this, -1) 
                // tu zatiaľ nedávame, kým nenaprogramujeme Sklad v Core.

                return true;
            }
            return false;
        }
    }
}