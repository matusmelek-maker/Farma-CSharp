using FarmSimulator.Core.Enums.Statok;

using FarmSimulator.Core.Enums.Zvierata;

using FarmSimulator.Core.Models.Produkty;

namespace FarmSimulator.Core.Models.Statky.ProdukcneStatky.Zvierata
{
    public abstract class Zviera : ProdukcneStatky
    {
        // --- Polia a Vlastnosti (Presne podľa Javy) ---
        public int KonstantaHlad { get; }
        public bool Pohlavie { get; } // true = samec, false = samica
        public int KonstantaReprodukcie { get; }
        public int IndexOhradky { get; }
        public DruhZvierata Druh { get; }

        public bool PripravenyNaReprodukciu { get; set; }
        public bool Najedene { get; set; }
        public int UrovenHladu { get; protected set; }

        // Interné zoznamy (ArrayListy z Javy)
        private List<Zviera> pridane = new();
        private List<Produkt> zeleninaZjedena = new();
        private List<Produkt> pridaneProdukty = new();

        // Logika pohybu (iba dáta pre UI)
        public double PoziciaX { get; set; } = 50;
        public double PoziciaY { get; set; } = 50;
        private double smerX;
        private double smerY;
        private Random random = new Random();

        // --- Konštruktor ---
        protected Zviera(string nazovObrazka, int kupnaCena, int predajnaCena, TypObchodnehoTovaru typ,
                         int konstantaHlad, bool pohlavie, int konstantaReprodukcie, int zivotnost,
                         int indexOhradky, DruhZvierata druh)
            : base(nazovObrazka, kupnaCena, predajnaCena, true, typ, zivotnost)
        {
            this.KonstantaHlad = konstantaHlad;
            this.Pohlavie = pohlavie;
            this.KonstantaReprodukcie = konstantaReprodukcie;
            this.IndexOhradky = indexOhradky;
            this.Druh = druh;
            this.PripravenyNaReprodukciu = false;
            this.Najedene = false;
            this.UrovenHladu = 0;

            InicializujSmerPohybu();
        }

        // --- Logika Pohybu (Bez Timerov, volané z Engine) ---
        private void InicializujSmerPohybu()
        {
            double uhol;
            do
            {
                uhol = random.NextDouble() * 2 * Math.PI;
                smerX = Math.Cos(uhol);
                smerY = Math.Sin(uhol);
            } while (Math.Abs(smerX) < 0.3 || Math.Abs(smerY) < 0.3);
        }

        public void AktualizujPohyb(int sirkaOhrady, int vyskaOhrady)
        {
            int rychlost = 1;
            PoziciaX += smerX * rychlost;
            PoziciaY += smerY * rychlost;

            // Odrazy od okrajov (ako v tvojej Jave)
            if (PoziciaX < 0 || PoziciaX > sirkaOhrady - 50) { smerX *= -1; }
            if (PoziciaY < 0 || PoziciaY > vyskaOhrady - 50) { smerY *= -1; }

            // Náhodná zmena smeru (1% šanca)
            if (random.NextDouble() < 0.01)
            {
                double deltaUhol = (random.NextDouble() - 0.5) * Math.PI / 4;
                double aktualnyUhol = Math.Atan2(smerY, smerX);
                smerX = Math.Cos(aktualnyUhol + deltaUhol);
                smerY = Math.Sin(aktualnyUhol + deltaUhol);
            }
        }

        // --- Hlavné Akcie (Metóda VykonajAkcie) ---
        public override void VykonajAkcie()
        {
            RozmnozSa();
            NajedzSa();
            Zomri();
            SpracujNaMeso();
        }

        // --- Reprodukcia ---
        public void RozmnozSa()
        {
           /* if (Vek != 0 && Vek % KonstantaReprodukcie == 0)
            {
                PripravenyNaReprodukciu = true;
            }

            if (PripravenyNaReprodukciu && Pohlavie) // Samec hľadá
            {
                // V C# používame LINQ pre hľadanie partnera
                var samice = Farma.Instance.Zvierata
                    .Where(s => !s.Pohlavie && s.PripravenyNaReprodukciu && s.Druh == this.Druh);

                foreach (var samica in samice)
                {
                    Zviera noveZviera = (random.Next(100) < 50) ? this.VytvorKlon() : samica.VytvorKlon();

                    if (noveZviera != null)
                    {
                        Sklad.Instance.ZmenPocetStatokSklad(noveZviera, 1);
                        Farma.Instance.PridajStatok(noveZviera);

                        this.PripravenyNaReprodukciu = false;
                        samica.PripravenyNaReprodukciu = false;
                        this.pridane.Add(noveZviera);
                    }
                }
            }*/
        }

        // --- Hladovanie ---
        public void NajedzSa()
        {
            // 1. Pokus o kŕmenie
           /* if (Vek % KonstantaHlad == 0 && Vek != 0 && !Najedene)
            {
                var krmivo = Farma.Instance.Statky
                    .OfType<Produkt>()
                    .FirstOrDefault(p => p.Typ == TypObchodnehoTovaru.Krmivo && p.Zije);

                if (krmivo != null)
                {
                    zeleninaZjedena.Add(krmivo);
                    Sklad.Instance.ZmenPocetStatokSklad(krmivo, -1);
                    Najedene = true;

                    if (UrovenHladu != 0) UrovenHladu--;

                    var hnoj = new Produkt(TypyProduktov.Hnoj);
                    pridaneProdukty.Add(hnoj);
                    Sklad.Instance.ZmenPocetStatokSklad(hnoj, 1);
                }
            }

            // 2. Ak ostalo hladné, zvyšuje sa úroveň hladu
            if (Vek % KonstantaHlad == 0 && !Najedene)
            {
                UrovenHladu++;
                if (UrovenHladu >= 3)
                {
                    Zije = false;
                    Zomri();
                }
            }*/
        }

        // --- Spracovanie na mäso ---
        public void SpracujNaMeso()
        {/*
            // C# Pattern matching: overíme či je to Dobytok a či je mŕtvy vekom
            if (this is Dobytok.Dobytok dobytok && !Zije && Vek >= Zivotnost)
            {
                // Tu predpokladáme, že Dobytok má prístup k zoznamu produktov zo svojho Typu
                foreach (var produktInfo in dobytok.Info.Produkty)
                {
                    if (produktInfo.Kategoria == KategoriaProduktu.Jednorazovy)
                    {
                        var produkt = new Produkt(produktInfo);
                        pridaneProdukty.Add(produkt);
                        Sklad.Instance.ZmenPocetStatokSklad(produkt, 1);
                    }
                }
            }*/
        }

        // --- Abstraktné metódy pre podtriedy ---
        protected abstract Zviera VytvorKlon();

        // --- Gettery (S logikou vymazania zoznamu po prebraní) ---
        public List<Zviera> GetPridaneZvierata() { var res = new List<Zviera>(pridane); pridane.Clear(); return res; }
        public List<Produkt> GetZjedenuZeleninu() { var res = new List<Produkt>(zeleninaZjedena); zeleninaZjedena.Clear(); return res; }
        public List<Produkt> GetPridaneProdukty() { var res = new List<Produkt>(pridaneProdukty); pridaneProdukty.Clear(); return res; }
    }
}