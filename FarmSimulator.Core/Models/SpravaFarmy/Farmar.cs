using FarmSimulator.Core.Models.Produkty;
using FarmSimulator.Core.Models.Statky;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmSimulator.Core.Models.SpravaFarmy
{
    public class Farmar
    {
        private static Farmar? _instance;
        public static Farmar Instance => _instance ??= new Farmar();
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
                AktualizujPeniaze(-statok.PredajnaCena);

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
                    AktualizujPeniaze(statok.PredajnaCena);
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
                    AktualizujPeniaze(statok.PredajnaCena);
                    Farma.Instance.OdstranStatok(statok);
                    return true;
                }
            }

            return false;
        }

        internal void AktualizujPeniaze(int rozdiel)
        {
            Peniaze += rozdiel;
            if (rozdiel >= 0)
                Console.WriteLine($"Farmár získal {rozdiel}$. Celkové peníze: {Peniaze}");
            else
                Console.WriteLine($"Farmár minul {rozdiel}$. Celkové peníze: {Peniaze}");

        }
    }
}
