using FarmSimulator.Core.Properties.Stromy;
using FarmSimulator.Core.Properties.Zelenina;
using FarmSimulator.Core.Properties.AtrakcneZviera;
using FarmSimulator.Core.Properties.Dobytok;
using FarmSimulator.Core.Properties.Produkt;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace FarmSimulator.UI
{
    public partial class ObchodWindow : Window
    {
        public ObchodWindow()
        {
            InitializeComponent();

            TxtPeniaze.Text = FarmSimulator.Core.Models.SpravaFarmy.Farmar.Instance.Peniaze.ToString();
            FarmSimulator.Core.Models.SpravaFarmy.Farmar.Instance.PeniazeSaZmenili += (novePeniaze) =>
            {
                Dispatcher.Invoke(() => { TxtPeniaze.Text = novePeniaze.ToString(); });
            };

            NacitajVsetkyKategorie();
        }
        private void BtnPredat_Click(object sender, RoutedEventArgs e)
        {
            TabItem aktivnaZalozka = (TabItem)ObchodTabs.SelectedItem;
            string nazovZalozky = aktivnaZalozka.Header.ToString();

            var farmar = FarmSimulator.Core.Models.SpravaFarmy.Farmar.Instance;

            // --- ZELENINA ---
            if (nazovZalozky == "Zelenina")
            {
                var riadok = TabulkaZelenina.SelectedItem as dynamic;
                if (riadok != null) { PredajStatokZFarmy(riadok.PovodnyTyp.Nazov, riadok.PredajnaCena); }
            }
            // --- STROMY ---
            else if (nazovZalozky == "Stromy")
            {
                var riadok = TabulkaStromy.SelectedItem as dynamic;
                if (riadok != null) { PredajStatokZFarmy(riadok.PovodnyTyp.Nazov, riadok.PredajnaCena); }
            }
            // --- DOBYTOK ---
            else if (nazovZalozky == "Dobytok")
            {
                var riadok = TabulkaDobytok.SelectedItem as dynamic;
                if (riadok != null) { PredajStatokZFarmy(riadok.PovodnyTyp.Nazov, riadok.PredajnaCena); }
            }
            // --- ATRAKČNÉ ZVIERATÁ ---
            else if (nazovZalozky == "Atrakčné Zvieratá")
            {
                var riadok = TabulkaAtrakcne.SelectedItem as dynamic;
                if (riadok != null) { PredajStatokZFarmy(riadok.PovodnyTyp.Nazov, riadok.PredajnaCena); }
            }
            // --- PRODUKTY (Tieto sa predávajú zo skladu, nie z farmy!) ---
            else if (nazovZalozky == "Produkty")
            {
                var riadok = TabulkaProdukty.SelectedItem as dynamic;
                if (riadok != null)
                {
                    var typProduktu = riadok.PovodnyTyp;

                    // Pokúsime sa odobrať zo skladu
                    if (FarmSimulator.Core.Models.SpravaFarmy.Sklad.Instance.OdoberProdukt(typProduktu))
                    {
                        farmar.AktualizujPeniaze(riadok.PredajnaCena);
                        AktualizujTabulkuProduktov(); // Prekreslí tabuľku, aby hráč videl aktuálny stav
                    }
                    else
                    {
                        MessageBox.Show($"Na sklade nemáš žiadny produkt: {typProduktu.Nazov}!", "Nedostatok", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
        }

        private void AktualizujTabulkuProduktov()
        {
            
        }

        // POMOCNÁ METÓDA PRE PREDÁVANIE STATKOV Z FARMÁRSKEJ PLOCHY
        private void PredajStatokZFarmy(string nazovStatku, int predajnaCena)
        {
            var farma = FarmSimulator.Core.Models.SpravaFarmy.Farma.Instance;

            // Nájdeme prvý statok na farme, ktorý sa volá rovnako a ešte žije
            // (Potrebujeme "using System.Linq;", ak ho hore nemáš, pridaj ho)
            var statokNaPredaj = farma.Statky.FirstOrDefault(s => s.Nazov == nazovStatku && s.Zije);

            if (statokNaPredaj != null)
            {
                // 1. Pripíšeme peniaze farmárovi
                FarmSimulator.Core.Models.SpravaFarmy.Farmar.Instance.AktualizujPeniaze(predajnaCena);

                // 2. KĽÚČOVÁ VEC: Povieme mu, že umrel. Toto odpáli event do MainWindow a vymaže jeho obrázok!
                statokNaPredaj.OdkazZeSomZomrel();

                // 3. Fyzicky ho vyhodíme zo zoznamu statkov
                farma.Statky.Remove(statokNaPredaj);
            }
            else
            {
                MessageBox.Show($"Na farme momentálne nemáš žiadny živý statok typu: {nazovStatku}!", "Nedá sa predať", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private void BtnKupit_Click(object sender, RoutedEventArgs e)
        {
            TabItem aktivnaZalozka = (TabItem)ObchodTabs.SelectedItem;
            string nazovZalozky = aktivnaZalozka.Header.ToString();

            var farmar = FarmSimulator.Core.Models.SpravaFarmy.Farmar.Instance;
            MainWindow hlavneOkno = (MainWindow)this.Owner;

            // --- ZELENINA ---
            if (nazovZalozky == "Zelenina")
            {
                var riadokZelenina = TabulkaZelenina.SelectedItem as dynamic;
                if (riadokZelenina != null)
                {
                    var typZeleniny = riadokZelenina.PovodnyTyp;
                    int indexVolnehoPola = hlavneOkno.NajdiVolnePole();

                    if (indexVolnehoPola != -1)
                    {
                        if (farmar.Peniaze >= typZeleniny.KupnaCena)
                        {
                            farmar.AktualizujPeniaze(-typZeleniny.KupnaCena);
                            hlavneOkno.ZmenObrazokPolicka(indexVolnehoPola, "zelenina", typZeleniny.Nazov, 1);

                            var novaZelenina = new FarmSimulator.Core.Models.Statky.Rastliny.Zelenina.Zelenina(typZeleniny);

                            // Prihlásenie na rast a smrť
                            novaZelenina.ZmenaStadiaRastu += (noveStadium) =>
                            {
                                hlavneOkno.ZmenObrazokPolicka(indexVolnehoPola, "zelenina", typZeleniny.Nazov, noveStadium + 1);
                            };
                            novaZelenina.OnZomrel += () =>
                            {
                                hlavneOkno.VymazObrazokPolicka(indexVolnehoPola);
                            };

                            FarmSimulator.Core.Models.SpravaFarmy.Farma.Instance.PridajStatok(novaZelenina);
                        }
                        else
                        {
                            MessageBox.Show("Nemáš dostatok peňazí na nákup!", "Nákup zlyhal", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Tvoja farma je úplne plná! Nemáš to kam zasadiť.", "Plno", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            // --- STROMY ---
            else if (nazovZalozky == "Stromy")
            {
                var riadokStromy = TabulkaStromy.SelectedItem as dynamic;
                if (riadokStromy != null)
                {
                    var typStromu = riadokStromy.PovodnyTyp;
                    int indexVolnehoPola = hlavneOkno.NajdiVolnePole();

                    if (indexVolnehoPola != -1)
                    {
                        if (farmar.Peniaze >= typStromu.KupnaCena)
                        {
                            farmar.AktualizujPeniaze(-typStromu.KupnaCena);
                            hlavneOkno.ZmenObrazokPolicka(indexVolnehoPola, "stromy", typStromu.Nazov, 1);

                            var novyStrom = new FarmSimulator.Core.Models.Statky.Rastliny.Stromy.Strom(typStromu);

                            // Prihlásenie na rast a smrť
                            novyStrom.ZmenaStadiaRastu += (noveStadium) =>
                            {
                                hlavneOkno.ZmenObrazokPolicka(indexVolnehoPola, "stromy", typStromu.Nazov, noveStadium);
                            };
                            novyStrom.OnZomrel += () =>
                            {
                                hlavneOkno.VymazObrazokPolicka(indexVolnehoPola);
                            };

                            FarmSimulator.Core.Models.SpravaFarmy.Farma.Instance.PridajStatok(novyStrom);
                        }
                        else
                        {
                            MessageBox.Show("Nemáš dostatok peňazí na nákup!", "Nákup zlyhal", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Tvoja farma je úplne plná! Nemáš to kam zasadiť.", "Plno", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            // --- DOBYTOK ---
            else if (nazovZalozky == "Dobytok")
            {
                var riadokDobytok = TabulkaDobytok.SelectedItem as dynamic;
                if (riadokDobytok != null)
                {
                    var typDobytka = riadokDobytok.PovodnyTyp;
                    if (farmar.Peniaze >= typDobytka.KupnaCena)
                    {
                        farmar.AktualizujPeniaze(-typDobytka.KupnaCena);

                        var novyDobytok = new FarmSimulator.Core.Models.Statky.Zvierata.Dobytok.Dobytok(typDobytka);

                        novyDobytok.Riadok = 0;
                        novyDobytok.Stlpec = 0;
                        novyDobytok.MinRiadok = 0;
                        novyDobytok.MaxRiadok = 2;
                        novyDobytok.MinStlpec = 0;
                        novyDobytok.MaxStlpec = 3;

                        // Prihlásenie na pohyb a smrť
                        novyDobytok.OnPohyb += (novyRiadok, novyStlpec) =>
                        {
                            hlavneOkno.Dispatcher.Invoke(() => {
                                var img = hlavneOkno.NajdiObrazokZvierata(novyDobytok);
                                if (img != null)
                                {
                                    Grid.SetRow(img, novyRiadok);
                                    Grid.SetColumn(img, novyStlpec);
                                }
                            });
                        };

                        novyDobytok.OnZomrel += () =>
                        {
                            hlavneOkno.OdstranZvieratko(novyDobytok);
                        };

                        FarmSimulator.Core.Models.SpravaFarmy.Farma.Instance.PridajStatok(novyDobytok);
                        hlavneOkno.PridajZvieraDoOhrady(novyDobytok, typDobytka.IndexOhradky);
                    }
                    else
                    {
                        MessageBox.Show("Nemáš dostatok peňazí na nákup!", "Nákup zlyhal", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            // --- ATRAKČNÉ ZVIERATÁ ---
            else if (nazovZalozky == "Atrakčné Zvieratá")
            {
                var riadokAtrakcne = TabulkaAtrakcne.SelectedItem as dynamic;
                if (riadokAtrakcne != null)
                {
                    var typAtrakcne = riadokAtrakcne.PovodnyTyp;
                    if (farmar.Peniaze >= typAtrakcne.KupnaCena)
                    {
                        farmar.AktualizujPeniaze(-typAtrakcne.KupnaCena);
                    }
                    else
                    {
                        MessageBox.Show("Nemáš dostatok peňazí na nákup!", "Nákup zlyhal", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            // --- PRODUKTY ---
            else if (nazovZalozky == "Produkty")
            {
                MessageBox.Show("Produkty z farmy nie je možné kupovať. Slúžia iba na predaj z tvojho skladu!", "Informácia", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void NacitajVsetkyKategorie()
        {
            var listDobytok = new List<object>();
            foreach (var typ in TypyDobytka.GetAll())
            {
                listDobytok.Add(new { Nazov = typ.Nazov, KupnaCena = typ.KupnaCena, PredajnaCena = typ.PredajnaCena, PovodnyTyp = typ });
            }
            TabulkaDobytok.ItemsSource = listDobytok;

            var listAtrakcne = new List<object>();
            foreach (var typ in TypyAtrakcnychZvierat.GetAll())
            {
                listAtrakcne.Add(new { Nazov = typ.Nazov, KupnaCena = typ.KupnaCena, PredajnaCena = typ.PredajnaCena, PovodnyTyp = typ });
            }
            TabulkaAtrakcne.ItemsSource = listAtrakcne;

            var listZelenina = new List<object>();
            foreach (var typ in TypyZeleniny.GetAll())
            {
                listZelenina.Add(new { Nazov = typ.Nazov, KupnaCena = typ.KupnaCena, PredajnaCena = typ.PredajnaCena, PovodnyTyp = typ });
            }
            TabulkaZelenina.ItemsSource = listZelenina;

            var listStromy = new List<object>();
            foreach (var typ in TypyStromov.GetAll())
            {
                listStromy.Add(new { Nazov = typ.Nazov, KupnaCena = typ.KupnaCena, PredajnaCena = typ.PredajnaCena, PovodnyTyp = typ });
            }
            TabulkaStromy.ItemsSource = listStromy;

            var listProdukty = new List<object>();
            foreach (var typ in TypyProduktov.GetAll())
            {
                listProdukty.Add(new { Nazov = typ.Nazov, KupnaCena = 0, PredajnaCena = typ.PredajnaCena, PovodnyTyp = typ });
            }
            TabulkaProdukty.ItemsSource = listProdukty;
        }
    }
}