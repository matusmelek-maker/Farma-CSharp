using FarmSimulator.Core.Models.Produkty;
using FarmSimulator.Core.Models.Statky;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmSimulator.Core.Models.Farma
{
    public class Farmar
    {
        // Vlastnosť (Property) pre peniaze. 
        // 'private set' znamená, že zvonku sa dajú iba čítať, meniť ich môže len Farmár
        public int Peniaze { get; private set; }

        public Farmar(int pociatocnePeniaze = 9999)
        {
            Peniaze = pociatocnePeniaze;
        }

        /// <summary>
        /// Pokúsi sa kúpiť statok.
        /// </summary>
        /// <returns>True ak sa nákup podaril, False ak nemá dosť peňazí.</returns>
        public bool KupStatok(Statok statok)
        {
            if (Peniaze >= statok.KupnaCena)
            {
                Peniaze -= statok.KupnaCena;

                if (statok is Produkt produkt)
                {
                    // Posielame tam priamo celý objekt produktu, ktorý sme práve vytvorili
                    Sklad.Instance.PridajProdukt(produkt);
                    Console.WriteLine($"[Farmár] Kúpil si produkt, ktorý bol uložený do skladu.");
                }
                else
                {
                    Farma.Instance.PridajStatok(statok);
                }

                return true;
            }

            return false; // Nedostatok peňazí
        }

        public bool PredajStatok(Statok statok)
        {
            if (statok is Produkt produkt)
            {
                // VOLANIE OPRAVENÉ: Už neposielame ", 1", pýtame si vymazanie jedného kusu podľa Info
                if (Sklad.Instance.OdoberProdukt(produkt.Info))
                {
                    Peniaze += statok.PredajnaCena;
                    Console.WriteLine($"[Farmár] Produkt bol predaný zo skladu.");
                    return true;
                }
                return false; // Produkt nebol na sklade
            }
            else
            {
                // Ak predávame živý statok z farmy
                if (statok.Zije)
                {
                    Peniaze += statok.PredajnaCena;
                    Farma.Instance.OdstranStatok(statok);
                    return true;
                }
            }

            return false;
        }
    }
}
