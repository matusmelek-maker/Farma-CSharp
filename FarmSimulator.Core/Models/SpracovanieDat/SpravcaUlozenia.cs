using FarmSimulator.Core.Models.SpracovanieDat;
using FarmSimulator.Core.Models.SpravaFarmy;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace FarmSimulator.Core.SpracovanieDat
{
    public static class SpravcaUlozenia
    {
        private const string CestaKSuboru = "farma_save.json";

        public static void UlozHru()
        {
            var stav = new UlozenyStav
            {
                PeniazeFarmara = Farmar.Instance.Peniaze,
                FarmaStatky = Farma.Instance.Statky.ToList(),
                SkladProdukty = Sklad.Instance.UskladneneProdukty.ToList()
            };

            var moznosti = new JsonSerializerOptions { WriteIndented = true };
            string jsonText = JsonSerializer.Serialize(stav, moznosti);
            File.WriteAllText(CestaKSuboru, jsonText);

            Console.WriteLine($"\n[Systém] Úspešne uložené do: {CestaKSuboru}");
        }

        public static bool NacitajHru()
        {
            if (!File.Exists(CestaKSuboru))
            {
                return false;
            }

            try
            {
                string jsonText = File.ReadAllText(CestaKSuboru);
                var stav = JsonSerializer.Deserialize<UlozenyStav>(jsonText);

                if (stav != null)
                {
                    Farmar.Instance.NastavPeniaze(stav.PeniazeFarmara);

                    Farma.Instance.Statky.Clear();
                    foreach (var s in stav.FarmaStatky)
                    {
                        Farma.Instance.PridajStatok(s);
                    }

                    Sklad.Instance.UskladneneProdukty.Clear();
                    foreach (var p in stav.SkladProdukty)
                    {
                        Sklad.Instance.PridajProdukt(p);
                    }

                    Console.WriteLine("\n[Systém] Úspešne načítané zo súboru.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[Chyba] Nepodarilo sa načítať hru: {ex.Message}");
            }

            return false;
        }
    }
}