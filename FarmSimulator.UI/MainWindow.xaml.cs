using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FarmSimulator.UI
{
    public partial class MainWindow : Window
    {
        private Image[] vizualnePolia = new Image[36];
        private bool[] jePoleObsadene = new bool[36];
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
            ImageBrush polePozadie = new ImageBrush();
            polePozadie.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/obrazky/pole.jpg", UriKind.Absolute));
            polePozadie.Stretch = Stretch.UniformToFill;

            for (int i = 0; i < 36; i++)
            {
                // 1. Vytvoríme mriežku, ktorá drží vrstvy na sebe
                Grid vrstvyBunky = new Grid();
                vrstvyBunky.Background = polePozadie; // Toto pozadie zeme sa už NIKDY nezmení

                // 2. Vytvoríme obrázok rastliny, ktorý pôjde NA pozadie
                Image obrazokRastliny = new Image();
                obrazokRastliny.Stretch = Stretch.Uniform;
                obrazokRastliny.Source = null; // Zatiaľ je prázdny (nič tu nerastie)

                vrstvyBunky.Children.Add(obrazokRastliny);

                // 3. Zabalíme to do starého známeho ohraničenia (Border)
                Border polePanel = new Border
                {
                    Child = vrstvyBunky, // Do vnútra vložíme našu dvojvrstvovú mriežku
                    Width = 35,
                    Height = 35,
                    Margin = new Thickness(2),
                    CornerRadius = new CornerRadius(3),
                    BorderBrush = Brushes.DarkGreen,
                    BorderThickness = new Thickness(0.5),
                    Tag = i
                };

                // 4. Uložíme si IBA TÚ VRCHNÚ VRSTVU (obrázok rastliny), aby sme ho neskôr vedeli meniť
                vizualnePolia[i] = obrazokRastliny;

                PoliaGrid.Children.Add(polePanel);
            }
        }

        // NOVÁ METÓDA: Nájde prvé prázdne pole
        public int NajdiVolnePole()
        {
            for (int i = 0; i < 36; i++)
            {
                // Pýtame sa nášho nového poľa. Výkričník znamená "Ak NIE JE obsadené"
                if (!jePoleObsadene[i])
                {
                    return i; // Vráti prvý voľný index
                }
            }
            return -1;
        }

        // OPRAVENÁ METÓDA: Mení už iba vrchnú vrstvu
        public void ZmenObrazokPolicka(int indexPolicka, string kategoria, string nazovRastliny, int stadiumRastu)
        {
            Dispatcher.Invoke(() =>
            {
                if (indexPolicka >= 0 && indexPolicka < vizualnePolia.Length)
                {
                    string cestaKObrazku = $"pack://application:,,,/Images/{kategoria}/{nazovRastliny}/{stadiumRastu}.png";

                    try
                    {
                        vizualnePolia[indexPolicka].Source = new BitmapImage(new Uri(cestaKObrazku, UriKind.Absolute));

                        // TOTO PRIDAJ: Políčko je odteraz oficiálne obsadené!
                        jePoleObsadene[indexPolicka] = true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Nepodarilo sa načítať obrázok: {cestaKObrazku}");
                    }
                }
            });
        }

        private void BtnPosunCas_Click(object sender, RoutedEventArgs e)
        {
            // Tu zavoláme hlavnú metódu z tvojho backendu (Core), 
            // ktorá prejde cyklom cez všetky zasadené rastliny a zvieratá a posunie im vek/štádium.

            // ZATIAĽ ZAKOMENTOVANÉ, kým to neprepojíme:
            // FarmSimulator.Core.Models.SpravaFarmy.Farma.Instance.PosunCas();

            // Dočasný výpis pre teba, aby si videl, že tlačidlo funguje
            MessageBox.Show("Čas na farme sa posunul o 1!", "Tik-Tak", MessageBoxButton.OK, MessageBoxImage.Information);
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