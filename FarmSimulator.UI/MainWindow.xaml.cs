using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FarmSimulator.UI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            TxtPeniaze.Text = FarmSimulator.Core.Models.SpravaFarmy.Farmar.Instance.Peniaze.ToString();

            // Prihlásenie na odber zmien peňazí
            FarmSimulator.Core.Models.SpravaFarmy.Farmar.Instance.PeniazeSaZmenili += (novePeniaze) =>
            {
                Dispatcher.Invoke(() => {
                    TxtPeniaze.Text = novePeniaze.ToString();
                });
            };

            // Vykreslenie komponentov farmy
            VykresliOhradky();
            VykresliPolia();
        }

        private void VykresliOhradky()
        {
            ImageBrush ohradaPozadie = new ImageBrush();
            ohradaPozadie.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/obrazky/ohrada.jpg", UriKind.Absolute));
            ohradaPozadie.Stretch = Stretch.UniformToFill;

            for (int i = 0; i < 6; i++)
            {
                Border ohradaPanel = new Border
                {
                    Background = ohradaPozadie,
                    Width = 160,
                    Height = 75,
                    Margin = new Thickness(5),
                    CornerRadius = new CornerRadius(10)
                };
                OhradyGrid.Children.Add(ohradaPanel);
            }
        }

        private void VykresliPolia()
        {
            // 1. Pripravíme si obrázok pozadia pre políčka
            ImageBrush polePozadie = new ImageBrush();
            polePozadie.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/obrazky/pole.jpg", UriKind.Absolute));
            polePozadie.Stretch = Stretch.UniformToFill;

            // 2. Vygenerujeme mriežku 6x6 (36 políčok)
            for (int i = 0; i < 36; i++)
            {
                Border polePanel = new Border
                {
                    Background = polePozadie,
                    Width = 45,                  // Menšia veľkosť, aby sa 6x6 pekne vošlo do stĺpca
                    Height = 45,
                    Margin = new Thickness(2),   // Jemné medzery medzi políčkami
                    CornerRadius = new CornerRadius(3),
                    BorderBrush = Brushes.DarkGreen,
                    BorderThickness = new Thickness(0.5)
                };

                // Pridáme políčko do stredového UniformGridu
                PoliaGrid.Children.Add(polePanel);
            }
        }

        // Akcia po kliknutí na Obchod
        private void BtnObchod_Click(object sender, RoutedEventArgs e)
        {
            // Vytvoríme inštanciu okna obchodu
            ObchodWindow obchod = new ObchodWindow();

            // Nastavíme, že toto hlavné okno je "vlastníkom" obchodu
            obchod.Owner = this;

            // Otvoríme ho ako dialóg (používateľ nemôže klikať na farmu, kým nezavrie obchod)
            obchod.ShowDialog();
        }

        // Akcia po kliknutí na Sklad
        private void BtnSklad_Click(object sender, RoutedEventArgs e)
        {
            // Tu neskôr otvoríš okno skladu
            MessageBox.Show("Otváram Sklad!");
        }
    }
}