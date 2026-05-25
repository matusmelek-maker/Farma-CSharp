using FarmSimulator.Core.Enums.Stromy;
using FarmSimulator.Core.Enums.Zelenina;
using FarmSimulator.Core.Properties.AtrakcneZviera;
using FarmSimulator.Core.Properties.Dobytok;
using FarmSimulator.Core.Properties.Produkt;
using System;
using System.Collections.Generic;
using System.Windows;
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
                    PredajnaCena = typ.PredajnaCena
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
                    PredajnaCena = typ.PredajnaCena
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
                    PredajnaCena = typ.PredajnaCena
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
                    PredajnaCena = typ.PredajnaCena
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
                    PredajnaCena = typ.PredajnaCena
                });
            }
            TabulkaProdukty.ItemsSource = listProdukty;
        }
    }
}