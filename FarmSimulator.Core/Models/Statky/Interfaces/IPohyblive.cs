using System;
using System.Collections.Generic;
using System.Text;

namespace FarmSimulator.Core.Models.Statky.Interfaces
{
    public interface IPohyblive
    {
        int Riadok { get; set; }
        int Stlpec { get; set; }

        // Hranice ohrady
        int MinRiadok { get; set; }
        int MaxRiadok { get; set; }
        int MinStlpec { get; set; }
        int MaxStlpec { get; set; }
        void PohniSa();
    }
}
