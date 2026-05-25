using FarmSimulator.Core.Models.Produkty;
using FarmSimulator.Core.Models.Statky;

namespace FarmSimulator.Core.Models.SpracovanieDat
{
    public class UlozenyStav
    {
        public int PeniazeFarmara { get; set; }

        public List<Statok> FarmaStatky { get; set; } = new();
        public List<Produkt> SkladProdukty { get; set; } = new();
    }
}
