using FarmSimulator.Core.Models.Statky.Zvierata;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private List<Border> ohradkyUI = new List<Border>();
        private Dictionary<FarmSimulator.Core.Models.Statky.Zvierata.Dobytok.Dobytok, Image> zvierataObrazky = new Dictionary<FarmSimulator.Core.Models.Statky.Zvierata.Dobytok.Dobytok, Image>();

        public MainWindow()
        {
            InitializeComponent();

            TxtPeniaze.Text = FarmSimulator.Core.Models.SpravaFarmy.Farmar.Instance.Peniaze.ToString();

            FarmSimulator.Core.Models.SpravaFarmy.Farmar.Instance.PeniazeSaZmenili += (novePeniaze) =>
            {
                Dispatcher.Invoke(() => {
                    TxtPeniaze.Text = novePeniaze.ToString();
                });
            };

            FarmSimulator.Core.Models.Ludia.SpravcaLudi.Instance.OnNakupDokonceny += (zarobok, spokojny) =>
            {
                Dispatcher.Invoke(() => {
                    string listokText = FarmSimulator.Core.Models.Ludia.SpravcaLudi.Instance.Clovek.ZiskajNakupnyListokText();
                    MessageBox.Show($"{listokText}\n\nZarobok: {zarobok}€\nSpokojný: {spokojny}");
                });
            };

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
                Grid kontajnerZvierat = new Grid();

                for (int r = 0; r < 3; r++) kontajnerZvierat.RowDefinitions.Add(new RowDefinition());
                for (int c = 0; c < 4; c++) kontajnerZvierat.ColumnDefinitions.Add(new ColumnDefinition());

                Border ohradaPanel = new Border
                {
                    Background = ohradaPozadie,
                    Width = 160,
                    Height = 75,
                    Margin = new Thickness(5),
                    CornerRadius = new CornerRadius(10),
                    Child = kontajnerZvierat
                };

                ohradkyUI.Add(ohradaPanel);
                OhradyGrid.Children.Add(ohradaPanel);
            }
        }

        public void PridajZvieraDoOhrady(FarmSimulator.Core.Models.Statky.Zvierata.Dobytok.Dobytok zviera, int indexOhradky)
        {
            Dispatcher.Invoke(() =>
            {
                if (indexOhradky >= 0 && indexOhradky < ohradkyUI.Count)
                {
                    var kontajner = (Grid)ohradkyUI[indexOhradky].Child;

                    Image img = new Image();
                    img.Source = new BitmapImage(new Uri($"pack://application:,,,/Images/obrazky/{zviera.Nazov}.gif", UriKind.Absolute));

                    img.Width = 30;
                    img.Height = 30;
                    img.Stretch = Stretch.Uniform;
                    img.Margin = new Thickness(2);

                    Grid.SetRow(img, zviera.Riadok);
                    Grid.SetColumn(img, zviera.Stlpec);

                    zvierataObrazky[zviera] = img;
                    kontajner.Children.Add(img);
                }
            });

            // Registrácia eventu
            zviera.OnNarodiloSaZviera -= Zviera_OnNarodiloSaZviera;
            zviera.OnNarodiloSaZviera += Zviera_OnNarodiloSaZviera;

            // Dôležité: Tu musíš tiež zaregistrovať pohyb pre nové zvieratko
            zviera.OnPohyb += (novyRiadok, novyStlpec) =>
            {
                Dispatcher.Invoke(() => {
                    var img = NajdiObrazokZvierata(zviera);
                    if (img != null)
                    {
                        Grid.SetRow(img, novyRiadok);
                        Grid.SetColumn(img, novyStlpec);
                    }
                });
            };
        } // <--- TOTO je koniec metódy PridajZvieraDoOhrady

        // Samostatná metóda pre obsluhu narodenia
        private void Zviera_OnNarodiloSaZviera(FarmSimulator.Core.Models.Statky.Zvierata.Zviera potomok)
        {
            var potomokDobytok = potomok as FarmSimulator.Core.Models.Statky.Zvierata.Dobytok.Dobytok;
            if (potomokDobytok != null)
            {
                this.PridajZvieraDoOhrady(potomokDobytok, potomokDobytok.IndexOhradky);
            }
        }

        public Image NajdiObrazokZvierata(FarmSimulator.Core.Models.Statky.Zvierata.Dobytok.Dobytok zviera)
        {
            if (zvierataObrazky.ContainsKey(zviera))
            {
                return zvierataObrazky[zviera];
            }
            return null;
        }

        private void VykresliPolia()
        {
            ImageBrush polePozadie = new ImageBrush();
            polePozadie.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/obrazky/pole.jpg", UriKind.Absolute));
            polePozadie.Stretch = Stretch.UniformToFill;

            for (int i = 0; i < 36; i++)
            {
                Grid vrstvyBunky = new Grid();
                vrstvyBunky.Background = polePozadie;

                Image obrazokRastliny = new Image();
                obrazokRastliny.Stretch = Stretch.Uniform;
                obrazokRastliny.Source = null;

                vrstvyBunky.Children.Add(obrazokRastliny);

                Border polePanel = new Border
                {
                    Child = vrstvyBunky,
                    Width = 35,
                    Height = 35,
                    Margin = new Thickness(2),
                    CornerRadius = new CornerRadius(3),
                    BorderBrush = Brushes.DarkGreen,
                    BorderThickness = new Thickness(0.5),
                    Tag = i
                };

                vizualnePolia[i] = obrazokRastliny;
                PoliaGrid.Children.Add(polePanel);
            }
        }

        public int NajdiVolnePole()
        {
            for (int i = 0; i < 36; i++)
            {
                if (!jePoleObsadene[i])
                {
                    return i;
                }
            }
            return -1;
        }

        public void VymazObrazokPolicka(int indexPolicka)
        {
            Dispatcher.Invoke(() =>
            {
                if (indexPolicka >= 0 && indexPolicka < vizualnePolia.Length)
                {
                    vizualnePolia[indexPolicka].Source = null;
                    jePoleObsadene[indexPolicka] = false;
                }
            });
        }

        public void OdstranZvieratko(FarmSimulator.Core.Models.Statky.Zvierata.Dobytok.Dobytok zviera)
        {
            Dispatcher.Invoke(() =>
            {
                var img = NajdiObrazokZvierata(zviera);
                if (img != null)
                {
                    var ohradaGrid = img.Parent as Grid;
                    if (ohradaGrid != null)
                    {
                        ohradaGrid.Children.Remove(img);
                    }
                    zvierataObrazky.Remove(zviera);
                }
            });
        }

        public void ZmenObrazokPolicka(int indexPolicka, string kategoria, string nazovRastliny, int stadiumRastu)
        {
            Dispatcher.Invoke(() =>
            {
                if (indexPolicka >= 0 && indexPolicka < vizualnePolia.Length)
                {
                    string cestaKObrazku = $"pack://application:,,,/Images/obrazky/{kategoria}/{nazovRastliny}/{stadiumRastu}.png";

                    try
                    {
                        vizualnePolia[indexPolicka].Source = new BitmapImage(new Uri(cestaKObrazku, UriKind.Absolute));
                        jePoleObsadene[indexPolicka] = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                            $"WPF nedokáže načítať tento obrázok:\n\n{cestaKObrazku}",
                            "Chýbajúci obrázok!",
                            MessageBoxButton.OK,
                            MessageBoxImage.Error);
                    }
                }
            });
        }

        private void BtnVypisBackendu_Click(object sender, RoutedEventArgs e)
        {
            var statkyNaFarme = FarmSimulator.Core.Models.SpravaFarmy.Farma.Instance.Statky;

            if (statkyNaFarme.Count == 0)
            {
                MessageBox.Show("Farma je úplne prázdna. Zatiaľ si nič nekúpil.", "Test Backend pamäte", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            string vypis = $"Farme má celkovo {statkyNaFarme.Count} vecí:\n\n";

            foreach (var statok in statkyNaFarme)
            {
                vypis += $"➡️ {statok.Nazov} (Vek: {statok.Vek}, Štádium rastu/Žije: {statok.Zije})\n";
            }

            MessageBox.Show(vypis, "Test Backend pamäte", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnPosunCas_Click(object sender, RoutedEventArgs e)
        {
            FarmSimulator.Core.Models.SpravaFarmy.Farma.Instance.PosunCas(1);
        }

        private void BtnObchod_Click(object sender, RoutedEventArgs e)
        {
            ObchodWindow obchod = new ObchodWindow();
            obchod.Owner = this;
            obchod.ShowDialog();
        }

        private void BtnSklad_Click(object sender, RoutedEventArgs e)
        {
            SkladWindow sklad = new SkladWindow();
            sklad.Owner = this;
            sklad.ShowDialog(); 
        }

        public List<int> DajIndexyOhrady(int indexOhradky)
        {
            int start = indexOhradky * 6;
            return Enumerable.Range(start, 6).ToList();
        }

        
    }
}