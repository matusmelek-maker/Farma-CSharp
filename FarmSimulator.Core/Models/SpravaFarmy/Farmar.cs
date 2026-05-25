using FarmSimulator.Core.Models.Produkty;
using FarmSimulator.Core.Models.Statky;

namespace FarmSimulator.Core.Models.SpravaFarmy
{
    public class Farmar
    {
        private static Farmar? _instance;
        public static Farmar Instance => _instance ??= new Farmar();
        public int Peniaze { get; private set; }

        public Farmar(int pociatocnePeniaze = 9999)
        {
            Peniaze = pociatocnePeniaze;
        }
        public void NastavPeniaze(int peniazeFarmara)
        {
            Peniaze = peniazeFarmara;
        }
        public bool KupStatok(Statok statok)
        {
            if (Peniaze >= statok.KupnaCena)
            {
                AktualizujPeniaze(-statok.PredajnaCena);

                if (statok is Produkt produkt)
                {
                    Sklad.Instance.PridajProdukt(produkt);
                }
                else
                {
                    Farma.Instance.PridajStatok(statok);
                }

                return true;
            }

            return false;
        }

        public bool PredajStatok(Statok statok)
        {
            if (statok is Produkt produkt)
            {
                if (Sklad.Instance.OdoberProdukt(produkt.Info))
                {
                    AktualizujPeniaze(statok.PredajnaCena);
                    Console.WriteLine($"{produkt.Nazov} bol predaný zo skladu.");
                    return true;
                }
                return false;
            }
            else
            {
                if (statok.Zije)
                {
                    AktualizujPeniaze(statok.PredajnaCena);
                    Farma.Instance.OdstranStatok(statok);
                    return true;
                }
            }

            return false;
        }

        public void AktualizujPeniaze(int rozdiel)
        {
            Peniaze += rozdiel;
            if (rozdiel >= 0)

                Console.WriteLine($"Farmár získal {rozdiel}$. Celkové peníze: {Peniaze}");
            else
                Console.WriteLine($"Farmár minul {rozdiel}$. Celkové peníze: {Peniaze}");

        }

        
    }
}
