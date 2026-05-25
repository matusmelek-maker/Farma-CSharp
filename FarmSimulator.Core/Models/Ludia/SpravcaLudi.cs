using FarmSimulator.Core.Models.Produkty;
using FarmSimulator.Core.Models.SpravaFarmy;
using FarmSimulator.Core.Properties.Produkt;

namespace FarmSimulator.Core.Models.Ludia
{
    public class SpravcaLudi
    {
        private static SpravcaLudi? _instance;
        public static SpravcaLudi Instance => _instance ??= new SpravcaLudi();

        public Zakaznik Clovek { get; private set; }

        public List<TypProduktuInfo> RealnePredaneProdukty { get; private set; }

        private int _casovac;
        private const int INTERVAL_NAKUPU = 15;

        public event Action<int, bool>? OnNakupDokonceny;

        private SpravcaLudi()
        {
            Clovek = new Zakaznik();
            RealnePredaneProdukty = new List<TypProduktuInfo>();
            _casovac = 0;
        }

        public void Tik()
        {
            _casovac++;

            if (_casovac >= INTERVAL_NAKUPU)
            {
                VykonajNakup();
                _casovac = 0;
            }
        }

        private void VykonajNakup()
        {
            Clovek.VytvorNakupnyListok();
            int pocetNaListku = Clovek.NakupnyListok.Count;
            int predaneKusy = 0;
            int celkovyZarobok = 0;

            foreach (var pozadovanyProdukt in Clovek.NakupnyListok)
            {
                if (Sklad.Instance.OdoberProdukt(pozadovanyProdukt))
                {
                    predaneKusy++;
                    celkovyZarobok += pozadovanyProdukt.PredajnaCena;
                    RealnePredaneProdukty.Add(pozadovanyProdukt);
                }
            }

            Farmar.Instance.AktualizujPeniaze(celkovyZarobok);

            int rozdiel = pocetNaListku - predaneKusy;
            bool spokojny = rozdiel <= (pocetNaListku / 2);
            Clovek.NastavSpokojnost(spokojny);

            OnNakupDokonceny?.Invoke(celkovyZarobok, spokojny);
        }
    }
}
