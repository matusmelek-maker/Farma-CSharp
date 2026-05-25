using FarmSimulator.Core.Models.SpravaFarmy;
using System.Linq;
using System.Windows;

namespace FarmSimulator.UI
{
    public partial class SkladWindow : Window
    {
        public SkladWindow()
        {
            InitializeComponent();

            // Prihlásenie na odber zmien, aby sa tabuľka sama obnovila pri pridaní/zhnití produktu
            Sklad.Instance.SkladSaZmenil += AktualizujTabulku;

            // Prvé načítanie pri otvorení okna
            AktualizujTabulku();
        }

        private void AktualizujTabulku()
        {
            Dispatcher.Invoke(() =>
            {
                // Zoberieme produkty, zoskupíme ich podľa typu a vyberieme LEN názov a počet
                var zoskupeneProdukty = Sklad.Instance.UskladneneProdukty
                    .Where(p => p.Zije) // Iba tie, čo ešte nie sú pokazené
                    .GroupBy(p => p.Info)
                    .Select(skupina => new
                    {
                        Nazov = skupina.Key.Nazov,
                        PocetKusov = skupina.Count()
                    })
                    .ToList();

                TabulkaSklad.ItemsSource = zoskupeneProdukty;
            });
        }

        // Dôležité: Keď sa okno zatvorí, musíme ho odhlásiť z eventu
        protected override void OnClosed(System.EventArgs e)
        {
            Sklad.Instance.SkladSaZmenil -= AktualizujTabulku;
            base.OnClosed(e);
        }
    }
}