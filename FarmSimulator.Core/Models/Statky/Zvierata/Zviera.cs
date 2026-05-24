using FarmSimulator.Core.Enums.Produkt;
using FarmSimulator.Core.Enums.Statok;

using FarmSimulator.Core.Enums.Zvierata;
using FarmSimulator.Core.Models.Produkty;
using FarmSimulator.Core.Models.SpravaFarmy;

namespace FarmSimulator.Core.Models.Statky.Zvierata
{
    public abstract class Zviera : Statok
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
        }

        // --- Reprodukcia ---
        public void RozmnozSa()
        {
            // 1. Zistenie, či je čas na reprodukciu (v C# používame Properties namiesto getVek())
            if (Vek != 0 && Vek % KonstantaReprodukcie == 0)
            {
                PripravenyNaReprodukciu = true;
            }

            // 2. Ak je samec (Pohlavie = true) a je pripravený, hľadá samicu
            if (PripravenyNaReprodukciu && Pohlavie)
            {
                // 3. Použitie LINQ na nájdenie vhodných samíc (Koniec dlhých for-cyklov a if-ov!)
                var vhodneSamice = Farma.Instance.Statky
                    .OfType<Zviera>() // Zoberieme z farmy iba zvieratá
                    .Where(z => !z.Pohlavie && z.PripravenyNaReprodukciu && z.Druh == this.Druh)
                    .ToList(); // Urobíme si kópiu zoznamu, aby sme mohli bezpečne iterovať

                foreach (var samica in vhodneSamice)
                {
                    // Šanca 50:50, čí klon to bude (používame tvoj private Random random)
                    Zviera noveZviera = (random.Next(100) < 50) ? this.VytvorKlon() : samica.VytvorKlon();

                    if (noveZviera != null)
                    {
                        // 4. Pridanie zvieratka PRIAMO na Farmu
                        Farma.Instance.PridajStatok(noveZviera);

                        // 5. Reset stavu u oboch rodičov
                        this.PripravenyNaReprodukciu = false;
                        samica.PripravenyNaReprodukciu = false;

                        // 6. Pridanie do interného zoznamu zvierata
                        this.pridane.Add(noveZviera);

                        // Samec sa práve rozmnožil, nemusí v tomto tiku hľadať ďalšie samice
                        break; 
                    }
                }
            }
        }

        // --- Hladovanie ---
        public void NajedzSa()
        {
            if (Vek != 0 && Vek % KonstantaHlad == 0)
            {
                if (!Najedene)
                {
                    // 2. Hľadáme potravu v SKLADE (Koniec if(statok is Produkt)!)
                    // FirstOrDefault nájde prvý živý produkt, ktorý je krmivo. Ak nenájde, vráti null.
                    var krmivo = Sklad.Instance.UskladneneProdukty
                        .FirstOrDefault(p => p.Info.TypTovaru == TypObchodnehoTovaru.Krmivo && p.Zije);

                    if (krmivo != null)
                    {
                        // Zviera úspešne našlo potravu
                        zeleninaZjedena.Add(krmivo);

                        // Namiesto odstraňovania, krmivo len "zabijeme" (Sklad si ho uprace sám pri ďalšom tiku)
                        krmivo.Zije = false;

                        Najedene = true;

                        if (UrovenHladu > 0)
                        {
                            UrovenHladu--;
                        }

                        // 3. Produkcia hnoja
                        var hnoj = new Produkt(TypyProduktov.Hnoj);
                        pridaneProdukty.Add(hnoj);

                        // V C# sme Skladu pridali metódu PridajProdukt, ktorá prijíma priamo objekt
                        Sklad.Instance.PridajProdukt(hnoj);
                    }
                }

                // 4. Ak po pokuse o jedenie zostalo hladné (nenašlo sa krmivo), stúpa hlad
                if (!Najedene)
                {
                    UrovenHladu++;
                    if (UrovenHladu >= 3)
                    {
                        // Zviera zomrelo od hladu
                        Zije = false;
                        Zomri(); // Toto automaticky "zakričí" do eventu OnZomrel, ktorý sme nastavili minule!
                    }
                }
            }
        }


        // --- Abstraktné metódy pre podtriedy ---
        protected abstract Zviera VytvorKlon();

        // --- Gettery (S logikou vymazania zoznamu po prebraní) ---
        public List<Zviera> GetPridaneZvierata() { var res = new List<Zviera>(pridane); pridane.Clear(); return res; }
        public List<Produkt> GetZjedenuZeleninu() { var res = new List<Produkt>(zeleninaZjedena); zeleninaZjedena.Clear(); return res; }
        public List<Produkt> GetPridaneProdukty() { var res = new List<Produkt>(pridaneProdukty); pridaneProdukty.Clear(); return res; }
    }
}