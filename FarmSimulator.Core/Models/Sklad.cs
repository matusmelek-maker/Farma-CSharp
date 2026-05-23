using FarmSimulator.Core.Models.Produkty;
using System.Collections.Generic;

namespace FarmSimulator.Core.Models
{
    public class Sklad
    {
        // Singleton vzor, rovnako ako Farma
        private static Sklad? _instance;
        public static Sklad Instance => _instance ??= new Sklad();

        // Slovník, kde kľúč je typ produktu (napr. KravskeMlieko) a hodnota je počet kusov
        public Dictionary<TypProduktuInfo, int> Polozky { get; private set; }

        private Sklad()
        {
            Polozky = new Dictionary<TypProduktuInfo, int>();
        }

        /// <summary>
        /// Pridá produkt do skladu. Ak tam už taký typ je, len zvýši počet.
        /// </summary>
        public void PridajProdukt(TypProduktuInfo produktInfo, int mnozstvo)
        {
            if (Polozky.ContainsKey(produktInfo))
            {
                Polozky[produktInfo] += mnozstvo;
            }
            else
            {
                Polozky.Add(produktInfo, mnozstvo);
            }
        }

        /// <summary>
        /// Pokúsi sa odobrať produkt zo skladu (napr. pri predaji alebo kŕmení).
        /// </summary>
        /// <returns>True, ak bol na sklade dostatok a odobratie sa podarilo.</returns>
        public bool OdoberProdukt(TypProduktuInfo produktInfo, int mnozstvo)
        {
            if (Polozky.ContainsKey(produktInfo) && Polozky[produktInfo] >= mnozstvo)
            {
                Polozky[produktInfo] -= mnozstvo;
                return true;
            }
            return false; // Nedostatok na sklade
        }

        /// <summary>
        /// Vráti aktuálny počet kusov daného produktu na sklade.
        /// </summary>
        public int ZistiPocet(TypProduktuInfo produktInfo)
        {
            if (Polozky.ContainsKey(produktInfo))
            {
                return Polozky[produktInfo];
            }
            return 0;
        }
    }
}