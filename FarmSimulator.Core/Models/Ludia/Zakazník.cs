using FarmSimulator.Core.Models.Farma;
using FarmSimulator.Core.Models.Ludia;
using FarmSimulator.Core.Models.Produkty;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmSimulator.Core.Models.Ludia
{
    public class Zakaznik
    {
        // V C# používame List miesto ArrayList
        public List<TypProduktuInfo> NakupnyListok { get; private set; }
        public bool SpokojnySNakupom { get; private set; }

        public Zakaznik()
        {
            NakupnyListok = new List<TypProduktuInfo>();
            SpokojnySNakupom = false;
        }

        public void VytvorNakupnyListok()
        {
            NakupnyListok.Clear(); // Najprv vymažeme starý lístok

            var random = new Random();
            var vsetkyTypy = TypyProduktov.GetAll().ToList(); // Zoberieme katalóg

            // Zákazník si vyžiada 1 až 5 náhodných produktov
            int pocetProduktov = random.Next(1, 6);

            for (int i = 0; i < pocetProduktov; i++)
            {
                int index = random.Next(vsetkyTypy.Count);
                NakupnyListok.Add(vsetkyTypy[index]);
            }
        }

        public void NastavSpokojnost(bool spokojny)
        {
            SpokojnySNakupom = spokojny;
        }

        public string ZiskajNakupnyListokText()
        {
            // Pomocou LINQ zoskupíme rovnaké produkty, aby bol výpis krajší (napr. Mrkva x3)
            var zoskupenePolozky = NakupnyListok
                .GroupBy(p => p.Nazov)
                .Select(skupina => $"- {skupina.Key} x{skupina.Count()}");

            string vyslednyText = "--- NÁKUPNÝ ZOZNAM ZÁKAZNÍKA ---\n";
            vyslednyText += string.Join("\n", zoskupenePolozky);
            vyslednyText += "\n--------------------------------";

            return vyslednyText;
        }
    }
}
