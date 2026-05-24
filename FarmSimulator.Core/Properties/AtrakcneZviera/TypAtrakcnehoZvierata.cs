using FarmSimulator.Core.Properties.Statok;
using FarmSimulator.Core.Properties.Zvierata;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmSimulator.Core.Properties.AtrakcneZviera
{
    public record TypAtrakcnehoZvierataInfo(
        string NazovObrazka,
        int KupnaCena,
        int PredajnaCena,
        int KonstantaHlad,
        bool Pohlavie,
        int KonstantaReprodukcie,
        int Zivotnost,
        int IndexOhradky,
        DruhZvierata Druh,
        TypObchodnehoTovaru TypTovaru,
        int CenaJazdy
    );

    public static class TypyAtrakcnychZvierat
    {
        public static readonly TypAtrakcnehoZvierataInfo Kon = new("kon", 150, 90, 21, true, 25, 250, 5, DruhZvierata.Kon, TypObchodnehoTovaru.Atrakcia, 25);
        public static readonly TypAtrakcnehoZvierataInfo Kobyla = new("kobyla", 160, 95, 21, false, 40, 250, 5, DruhZvierata.Kon, TypObchodnehoTovaru.Atrakcia, 20);

        public static IEnumerable<TypAtrakcnehoZvierataInfo> GetAll() => [Kon, Kobyla];
    }
}
