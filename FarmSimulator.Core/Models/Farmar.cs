using FarmSimulator.Core.Models.Statky;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmSimulator.Core.Models
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

                // Pridáme statok do Farmy
                Farma.Instance.PridajStatok(statok);

                // Neskôr sem pridáme aj odoslanie do Skladu, keď ho naprogramuješ
                // Sklad.Instance.ZmenPocetStatokSklad(statok, 1);

                return true;
            }

            return false; // Nedostatok peňazí
        }

        /// <summary>
        /// Predá statok, pripočíta peniaze a označí ho ako mŕtvy/predaný.
        /// </summary>
        public bool PredajStatok(Statok statok)
        {
            if (statok.Zije)
            {
                Peniaze += statok.PredajnaCena;

                Farma.Instance.OdstranStatok(statok);
                return true;
            }
            
            // Môžeme tu zavolať akcie, ktoré sa vykonajú pri predaji, napríklad zníženie počtu statkov na farme
            // Neskôr sem pridáš aj odstránenie zo skladu
            // funkciu na najdenie najstaršieho statku a ten
            // odstrani typ sa bude vediet po kliknuti aky druh predava 
            return false;
        }
    }
}
