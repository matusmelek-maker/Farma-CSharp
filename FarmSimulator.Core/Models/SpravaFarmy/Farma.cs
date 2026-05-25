using FarmSimulator.Core.Models.Ludia;
using FarmSimulator.Core.Models.Statky;
using System.Linq;

namespace FarmSimulator.Core.Models.SpravaFarmy
{
    public class Farma
    {
        private static Farma? _instance;
        public static Farma Instance => _instance ??= new Farma();

        public List<Statok> Statky { get; private set; }

        private Farma()
        {
            Statky = new List<Statok>();
        }

        public void PridajStatok(Statok statok)
        {
            Statky.Add(statok);
        }

        public void PosunCas(int pocetTikov)
        {
            Console.WriteLine($"\n--- Posúvam čas o {pocetTikov} tikov (sekúnd) ---");

            for (int tik = 0; tik < pocetTikov; tik++)
            {
                var aktualneStatky = Statky.ToList();

                foreach (var statok in aktualneStatky)
                {
                    if (statok.Zije)
                    {
                        statok.Tik();
                        statok.VykonajAkcie();
                    }
                }

                Sklad.Instance.PosunCasVSklade();
                SpravcaLudi.Instance.Tik();
            }

            int povodnyPocet = Statky.Count;
            Statky.RemoveAll(s => !s.Zije);
            int mrtve = povodnyPocet - Statky.Count;

        }

        public void VypisStatky()
        {
            Console.WriteLine("\n--- Aktuálne statky na farme ---");
            foreach (var statok in Statky)
            {
                Console.WriteLine($"Typ: {statok.Nazov}, Vek: {statok.Vek}");
            }
        }

        public Statok? NajdiNajstarsiStatok(Statok hladanyTyp)
        {
            return Statky
                .Where(s => s.Zije && s.Nazov == hladanyTyp.Nazov)
                .OrderByDescending(s => s.Vek)
                .FirstOrDefault();
        }

        public void OdstranStatok(Statok statok)
        {
            if (Statky.Contains(statok))
            {
                Statky.Remove(statok);
                Console.WriteLine($"Statok {statok.Nazov} bol z farmy odstránený.");
            }
        }
    }
}
