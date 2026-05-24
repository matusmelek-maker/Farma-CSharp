using FarmSimulator.Core.Models.Produkty;
using FarmSimulator.Core.Models.SpravaFarmy;
using FarmSimulator.Core.Properties.Produkt;

namespace FarmSimulator.Core.Models.Ludia
{
    public class SpravcaLudi
    {
        // Singleton vzor
        private static SpravcaLudi? _instance;
        public static SpravcaLudi Instance => _instance ??= new SpravcaLudi();

        public Zakaznik Clovek { get; private set; }

        // Ak chceš sledovať štatistiku, čo všetko sa už reálne predalo
        public List<TypProduktuInfo> RealnePredaneProdukty { get; private set; }

        private int _casovac;
        private const int INTERVAL_NAKUPU = 15; // Po koľkých tikoch príde zákazník

        // Definujeme udalosť, na ktorú sa môže pripojiť CLI/UI pre výpis nákupu
        public event Action<int, bool>? OnNakupDokonceny;

        private SpravcaLudi()
        {
            Clovek = new Zakaznik();
            RealnePredaneProdukty = new List<TypProduktuInfo>();
            _casovac = 0;
        }

        /// <summary>
        /// Túto metódu bude volať Farma pri posúvaní času.
        /// </summary>
        public void Tik()
        {
            _casovac++;

            if (_casovac >= INTERVAL_NAKUPU)
            {
                VykonajNakup();
                _casovac = 0; // Resetujeme časovač
            }
        }

        private void VykonajNakup()
        {
            // 1. Zákazník si spraví lístok
            Clovek.VytvorNakupnyListok();
            int pocetNaListku = Clovek.NakupnyListok.Count;
            int predaneKusy = 0;
            int celkovyZarobok = 0;

            // 2. Prechádzame jeho lístok a hľadáme to v Sklade
            foreach (var pozadovanyProdukt in Clovek.NakupnyListok)
            {
                // Sklad.OdoberProdukt vráti True, ak bol na sklade živý produkt tohto typu
                if (Sklad.Instance.OdoberProdukt(pozadovanyProdukt))
                {
                    predaneKusy++;
                    celkovyZarobok += pozadovanyProdukt.PredajnaCena;
                    RealnePredaneProdukty.Add(pozadovanyProdukt);
                }
            }

            // 3. Zákazník platí farmárovi (nemusíme volať PredajStatok, rovno pripíšeme peniaze)
            Farmar.Instance.AktualizujPeniaze(celkovyZarobok);

            // 4. Vyhodnotenie spokojnosti (rovnaká logika ako v Jave - aspoň polovica nakúpená)
            int rozdiel = pocetNaListku - predaneKusy;
            bool spokojny = rozdiel <= (pocetNaListku / 2);
            Clovek.NastavSpokojnost(spokojny);

            // 5. Oznámime okolitému svetu (napr. konzole), že prebehol nákup
            OnNakupDokonceny?.Invoke(celkovyZarobok, spokojny);
        }
    }
}
