using FarmSimulator.Core.Properties.Produkt;

namespace FarmSimulator.Core.Models.Ludia
{
    public class Zakaznik
    {
        public List<TypProduktuInfo> NakupnyListok { get; private set; }
        public bool SpokojnySNakupom { get; private set; }

        public Zakaznik()
        {
            NakupnyListok = new List<TypProduktuInfo>();
            SpokojnySNakupom = false;
        }

        public void VytvorNakupnyListok()
        {
            NakupnyListok.Clear();

            var random = new Random();
            var vsetkyTypy = TypyProduktov.GetAll().ToList();

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
