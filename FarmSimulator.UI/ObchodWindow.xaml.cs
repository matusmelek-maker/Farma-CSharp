using FarmSimulator.Core.Enums.Stromy;
using FarmSimulator.Core.Enums.Zelenina;
using FarmSimulator.Core.Properties.AtrakcneZviera;
using FarmSimulator.Core.Properties.Dobytok;
using FarmSimulator.Core.Properties.Produkt;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
// Dôležité: Nezabudni sem pridať "using" pre tvoje enumy z Core projektu!
// napr.: using FarmSimulator.Core.Modely; 

namespace FarmSimulator.UI
{
    public partial class ObchodWindow : Window
    {
        public ObchodWindow()
        {
            InitializeComponent();

            // Načítanie peňazí (nechávame rovnaké)
            TxtPeniaze.Text = FarmSimulator.Core.Models.SpravaFarmy.Farmar.Instance.Peniaze.ToString();
            FarmSimulator.Core.Models.SpravaFarmy.Farmar.Instance.PeniazeSaZmenili += (novePeniaze) =>
            {
                Dispatcher.Invoke(() => { TxtPeniaze.Text = novePeniaze.ToString(); });
            };

            // Zavoláme naplnenie pre všetky sekcie
            NacitajVsetkyKategorie();
        }
        private void BtnKupit_Click(object sender, RoutedEventArgs e)
        {
            // 1. Zistíme, ktorá záložka je momentálne otvorená
            TabItem aktivnaZalozka = (TabItem)ObchodTabs.SelectedItem;
            string nazovZalozky = aktivnaZalozka.Header.ToString();

            var farmar = FarmSimulator.Core.Models.SpravaFarmy.Farmar.Instance;

            // 2. Podľa záložky zistíme označený riadok a vykonáme nákup
            if (nazovZalozky == "Zelenina")
            {
                var vybranyRiadok = TabulkaZelenina.SelectedItem as dynamic;
                if (vybranyRiadok != null)
                {
                    var typZeleniny = vybranyRiadok.PovodnyTyp;
                    if (farmar.Peniaze >= typZeleniny.KupnaCena)
                    {
                        farmar.AktualizujPeniaze(-typZeleniny.KupnaCena);
                        MainWindow hlavneOkno = (MainWindow)this.Owner;
                        hlavneOkno.ZmenObrazokPolicka(hlavneOkno.NajdiVolnePole(), "zelenina", typZeleniny.Nazov, 1);
                        var novaZelenina = new FarmSimulator.Core.Properties.Zelenina(typZeleniny);

                        // 4. TOTO PRIDÁME: Povieme farme, nech si ho uloží na daný index
                        FarmSimulator.Core.Models.SpravaFarmy.Farma.Instance.PridajStat okNaPole(indexVolnehoPola, novaZelenina);
                    }
                    else
                    {
                        MessageBox.Show("Nemáš dostatok peňazí na nákup!", "Nákup zlyhal", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            else if (nazovZalozky == "Dobytok")
            {
                var vybranyRiadok = TabulkaDobytok.SelectedItem as dynamic;
                if (vybranyRiadok != null)
                {
                    var typDobytka = vybranyRiadok.PovodnyTyp;
                    if (farmar.Peniaze >= typDobytka.KupnaCena)
                    {
                        farmar.AktualizujPeniaze(-typDobytka.KupnaCena);
                        // Sem neskôr doplníš: farmar.KupStatok(new Dobytok(typDobytka));
                    }
                    else
                    {
                        MessageBox.Show("Nemáš dostatok peňazí na nákup!", "Nákup zlyhal", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            else if (nazovZalozky == "Atrakčné Zvieratá")
            {
                // DOPLNENÉ: Načítanie a nákup pre Atrakčné zvieratá
                var vybranyRiadok = TabulkaAtrakcne.SelectedItem as dynamic;
                if (vybranyRiadok != null)
                {
                    var typAtrakcne = vybranyRiadok.PovodnyTyp;
                    if (farmar.Peniaze >= typAtrakcne.KupnaCena)
                    {
                        farmar.AktualizujPeniaze(-typAtrakcne.KupnaCena);
                        // Sem neskôr doplníš: farmar.KupStatok(new AtrakcneZviera(typAtrakcne));
                    }
                    else
                    {
                        MessageBox.Show("Nemáš dostatok peňazí na nákup!", "Nákup zlyhal", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            else if (nazovZalozky == "Stromy")
            {
                // DOPLNENÉ: Načítanie a nákup pre Stromy
                var vybranyRiadok = TabulkaStromy.SelectedItem as dynamic;
                if (vybranyRiadok != null)
                {
                    var typStromu = vybranyRiadok.PovodnyTyp;
                    if (farmar.Peniaze >= typStromu.KupnaCena)
                    {
                        farmar.AktualizujPeniaze(-typStromu.KupnaCena);
                        // Sem neskôr doplníš: farmar.KupStatok(new Strom(typStromu));
                    }
                    else
                    {
                        MessageBox.Show("Nemáš dostatok peňazí na nákup!", "Nákup zlyhal", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            else if (nazovZalozky == "Produkty")
            {
                // DOPLNENÉ: Produkty sa nedajú kúpiť, tak hráča len informujeme
                MessageBox.Show("Produkty z farmy nie je možné kupovať. Slúžia iba na predaj z tvojho skladu!", "Informácia", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void NacitajVsetkyKategorie()
        {
            // 1. DOBYTOK
            var listDobytok = new List<object>();
            foreach (var typ in TypyDobytka.GetAll())
            {
                listDobytok.Add(new
                {
                    Nazov = typ.Nazov,
                    KupnaCena = typ.KupnaCena,
                    PredajnaCena = typ.PredajnaCena,
                    PovodnyTyp = typ
                });
            }
            TabulkaDobytok.ItemsSource = listDobytok;

            // 2. ATRAKČNÉ ZVIERATÁ
            var listAtrakcne = new List<object>();
            foreach (var typ in TypyAtrakcnychZvierat.GetAll())
            {
                listAtrakcne.Add(new
                {
                    Nazov = typ.Nazov,
                    KupnaCena = typ.KupnaCena,
                    PredajnaCena = typ.PredajnaCena,
                    PovodnyTyp = typ
                });
            }
            TabulkaAtrakcne.ItemsSource = listAtrakcne;

            // 3. ZELENINA
            var listZelenina = new List<object>();
            foreach (var typ in TypyZeleniny.GetAll())
            {
                listZelenina.Add(new
                {
                    Nazov = typ.Nazov,
                    KupnaCena = typ.KupnaCena,
                    PredajnaCena = typ.PredajnaCena,
                    PovodnyTyp = typ
                });
            } // <-- TOTO TI TAM CHÝBALO
            TabulkaZelenina.ItemsSource = listZelenina;

            // 4. STROMY
            var listStromy = new List<object>();
            foreach (var typ in TypyStromov.GetAll())
            {
                listStromy.Add(new
                {
                    Nazov = typ.Nazov,
                    KupnaCena = typ.KupnaCena,
                    PredajnaCena = typ.PredajnaCena,
                    PovodnyTyp = typ
                });
            }
            TabulkaStromy.ItemsSource = listStromy;

            // 5. PRODUKTY (Na predaj zo skladu)
            var listProdukty = new List<object>();
            foreach (var typ in TypyProduktov.GetAll())
            {
                listProdukty.Add(new
                {
                    Nazov = typ.Nazov,
                    // Tu som dal 0, keďže produkty sa zväčša len predávajú. 
                    // V C# musíš zachovať rovnaký typ (číslo) pre stĺpec.
                    KupnaCena = 0,
                    PredajnaCena = typ.PredajnaCena,
                    PovodnyTyp = typ
                });
            }
            TabulkaProdukty.ItemsSource = listProdukty;
        }
    }
}