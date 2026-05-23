using FarmSimulator.Core.Models.Statky;
using System;
using System.Collections.Generic;
using System.Text;

namespace FarmSimulator.Core.Models.Farma
{
    public class Farma
    {
        // Singleton vzor (rovnako ako si mal v Jave, ale s C# syntaxou)
        private static Farma? _instance;
        public static Farma Instance => _instance ??= new Farma();

        // Zoznam všetkých statkov na farme
        public List<Statok> Statky { get; private set; }

        private Farma()
        {
            Statky = new List<Statok>();
        }

        public void PridajStatok(Statok statok)
        {
            Statky.Add(statok);
            Console.WriteLine($"[Farma] Pridaný nový statok: {statok.Typ}");
        }

        /// <summary>
        /// Hlavná metóda, ktorú bude volať CLI alebo UI na simuláciu plynutia času.
        /// </summary>
        public void PosunCas(int pocetTikov)
        {
            Console.WriteLine($"\n--- Posúvam čas o {pocetTikov} tikov (sekúnd) ---");

            for (int tik = 0; tik < pocetTikov; tik++)
            {
                // Kópia zoznamu pre bezpečné prechádzanie
                var aktualneStatky = Statky.ToList();

                foreach (var statok in aktualneStatky)
                {
                    if (statok.Zije)
                    {
                        statok.Tik(); // Aktualizuje vek, zdravie, atď.
                        statok.VykonajAkcie(); // Zviera vyhladne, rozmnoží sa, atď.
                    }
                }

                Sklad.Instance.PosunCasVSklade();
            }

            // Čistenie zoznamu od mŕtvych statkov
            int povodnyPocet = Statky.Count;
            Statky.RemoveAll(s => !s.Zije);
            int mrtve = povodnyPocet - Statky.Count;

            if (mrtve > 0)
            {
                Console.WriteLine($"[Upozornenie] Počas tohto obdobia zomrelo {mrtve} statkov.");
            }
        }

        public void VypisStatky()
        {
            Console.WriteLine("\n--- Aktuálne statky na farme ---");
            foreach (var statok in Statky)
            {
                Console.WriteLine($"Typ: {statok.Nazov}, Vek: {statok.Vek}, Žije: {statok.Zije}");
            }
        }

        public Statok? NajdiNajstarsiStatok(Statok hladanyTyp)
        {
            Statok najstarsi = null;
            int maxVek = -1;

            foreach (var s in Statky)
            {
                if (s.Zije && s.Nazov == hladanyTyp.Nazov)
                {
                    if (s.Vek > maxVek)
                    {
                        maxVek = s.Vek;
                        najstarsi = s;
                    }
                }
            }

            return najstarsi;
        }

        public void OdstranStatok(Statok statok)
        {
            // C# List má vstavanú metódu Remove, ktorá nájde konkrétny objekt a vymaže ho
            if (Statky.Contains(statok))
            {
                Statky.Remove(statok);
                Console.WriteLine($"[Farma] Statok bol z farmy odstránený.");
            }
        }
    }
}
