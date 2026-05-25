using FarmSimulator.Core.Models.SpracovanieDat;
using FarmSimulator.Core.Models.SpravaFarmy;
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

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"\n[Chyba] Nepodarilo sa načítať hru: {ex.Message}");
            }

            return false;
        }
    }
}