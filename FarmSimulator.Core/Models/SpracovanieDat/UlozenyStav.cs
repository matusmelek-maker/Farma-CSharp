using FarmSimulator.Core.Models.Produkty;
using FarmSimulator.Core.Models.Statky;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmSimulator.Core.Models.SpracovanieDat
{
    public class UlozenyStav
    {
        public int PeniazeFarmara { get; set; }

        // Zoznamy všetkého živého na farme a všetkého v sklade
        public List<Statok> FarmaStatky { get; set; } = new();
        public List<Produkt> SkladProdukty { get; set; } = new();
    }
}
