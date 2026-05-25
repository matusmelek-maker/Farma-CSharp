using FarmSimulator.Core.Models.SpravaFarmy;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FarmSimulator.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            
        }

        private void OpenObchod_Click(object sender, RoutedEventArgs e)
        {
            // Tu otvoríš nové okno Obchodu
        }

        private void VykresliPole()
        {
            // Predpokladajme, že máš 25 políčok (5x5)
            for (int i = 0; i < 25; i++)
            {
                var btn = new Button { Content = "Prázdne" };
                btn.Click += (s, e) => MessageBox.Show("Sadenie...");
                PoleGrid.Children.Add(btn);
            }
        }
    }
}