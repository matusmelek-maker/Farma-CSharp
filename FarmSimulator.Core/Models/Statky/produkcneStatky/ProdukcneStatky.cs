using FarmSimulator.Core.Enums;
using FarmSimulator.Core.Enums.Statok;
using System;

namespace FarmSimulator.Core.Models.Statky.ProdukcneStatky
{
    /// <summary>
    /// Abstraktná trieda pre statky, ktoré niečo produkujú (zvieratá, rastliny).
    /// </summary>
    public abstract class ProdukcneStatky : Statok
    {
        // Definujeme udalosť, na ktorú sa UI pripojí
        public event Action? OnZomrel;

        protected ProdukcneStatky(string nazovObrazka, int kupnaCena, int predajnaCena, bool zije, TypObchodnehoTovaru typ, int zivotnost)
            : base(nazovObrazka, kupnaCena, predajnaCena, zije, typ, zivotnost)
        {
        }

        /// <summary>
        /// Prekrytá metóda Zomri, ktorá vyvolá udalosť pre UI/CLI.
        /// </summary>
        public override bool Zomri()
        {
            if (base.Zomri())
            {
                // Vyvoláme udalosť - Core nemusí vedieť, či existuje nejaké GUI.
                // Ak je niekto prihlásený na odber (UI), vykoná sa to.
                OnZomrel?.Invoke();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Každý produkčný statok musí definovať, ako niečo vyrába.
        /// </summary>
        public abstract void Produkcia();
    }
}