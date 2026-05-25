using System.Windows.Controls;

namespace FarmSimulator.UI
{
    public partial class PanelFarmara : UserControl
    {
        public PanelFarmara()
        {
            InitializeComponent();

            // Načítanie počiatočnej hodnoty
            TxtPeniaze.Text = FarmSimulator.Core.Models.SpravaFarmy.Farmar.Instance.Peniaze.ToString();

            // Prihlásenie na odber zmien peňazí
            FarmSimulator.Core.Models.SpravaFarmy.Farmar.Instance.PeniazeSaZmenili += (novePeniaze) =>
            {
                Dispatcher.Invoke(() => {
                    TxtPeniaze.Text = novePeniaze.ToString();
                });
            };
        }
    }
}