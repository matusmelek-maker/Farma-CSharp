using FarmSimulator.Core.Models.Produkty;
using FarmSimulator.Core.Properties.Produkt;

namespace FarmSimulator.Core.Models.SpravaFarmy
{
    public class Sklad
    {
        private static Sklad? _instance;
        public static Sklad Instance => _instance ??= new Sklad();

        public List<Produkt> UskladneneProdukty { get; private set; }

        private Sklad()
        {
            UskladneneProdukty = new List<Produkt>();
        }

        public void PridajProdukt(Produkt produkt)
        {
            UskladneneProdukty.Add(produkt);
        }

        public bool OdoberProdukt(TypProduktuInfo info)
        {
            var produktNaOdobratie = UskladneneProdukty.FirstOrDefault(p => p.Info == info && p.Zije);

            if (produktNaOdobratie != null)
            {
                UskladneneProdukty.Remove(produktNaOdobratie);
                return true;
            }
            return false;
        }

        public void VypisProdukty()
        {
            Console.WriteLine("\n--- Aktuálne produkty v sklade ---");
            foreach (var produkt in UskladneneProdukty)
            {
                Console.WriteLine($"Produkt: {produkt.Nazov}, Vek: {produkt.Vek}");
            }
        }

        public int ZistiPocet(TypProduktuInfo info)
        {
            return UskladneneProdukty.Count(p => p.Info == info && p.Zije);
        }

        public void PosunCasVSklade()
        {
            var kopiaProduktov = UskladneneProdukty.ToList();

            foreach (var p in kopiaProduktov)
            {
                if (p.Zije)
                {
                    p.Tik();

                    p.VykonajAkcie();
                }
            }

            int povodnyPocet = UskladneneProdukty.Count;
            UskladneneProdukty.RemoveAll(p => !p.Zije);
        }
    }
}