using System;
using System.Collections.Generic;
using System.Globalization; 
using System.Linq; 
using System.Net.Http; 
using System.Text.Json; 
using System.Threading.Tasks;

namespace PublicHolidayTracker
{
    public class Holiday
    {
        public string? date { get; set; }       
        public string? localName { get; set; }  
        public string? name { get; set; }        
        public string? countryCode { get; set; } 

       ' işareti koyduk.
        public bool @fixed { get; set; }

        public bool global { get; set; }         
    }

    class Program
    {
        private static readonly HttpClient client = new HttpClient();

        private static List<Holiday> allHolidays = new List<Holiday>();

        static async Task Main(string[] args)
        {
            Console.Title = "Public Holiday Tracker - Türkiye (2023-2025)";

            Console.WriteLine("Veriler API üzerinden çekiliyor, lütfen bekleyiniz...");

            await LoadHolidaysAsync();

            if (allHolidays.Count == 0)
            {
                Console.WriteLine("HATA: Veriler sunucudan alınamadı. İnternet bağlantınızı kontrol edin.");
                Console.ReadLine(); 
                return;
            }

           
            bool exit = false;
            while (!exit)
            {
                Console.Clear(); 

                Console.WriteLine("========================================");
                Console.WriteLine("      TÜRKİYE RESMİ TATİL TAKİBİ");
                Console.WriteLine($"      (Hafızadaki Kayıt Sayısı: {allHolidays.Count})");
                Console.WriteLine("========================================");
                Console.WriteLine("1. Yıla Göre Listele (2023, 2024, 2025)");
                Console.WriteLine("2. Tarihe Göre Ara (Örn: 15-07 veya 1.1)");
                Console.WriteLine("3. İsme Göre Ara (Örn: Cumhuriyet)");
                Console.WriteLine("4. Tüm Listeyi Göster");
                Console.WriteLine("5. Çıkış");
                Console.Write("Seçiminiz: ");

                string secim = Console.ReadLine() ?? ""; 

                switch (secim)
                {
                    case "1":
                        YearSelectionMenu();
                        break;
                    case "2":
                        SearchByDate();
                        break;
                    case "3":
                        SearchByName();
                        break;
                    case "4":
                        ListAllHolidays();
                        break;
                    case "5":
                        exit = true;
                        Console.WriteLine("Program kapatılıyor...");
                        break;
                    default:
                        Console.WriteLine("Geçersiz seçim, lütfen tekrar deneyin.");
                        break;
                }

                if (!exit)
                {
                    Console.WriteLine("\nAna menüye dönmek için Enter'a basınız...");
                    Console.ReadLine();
                }
            }
        }

        private static async Task LoadHolidaysAsync()
        {
            int[] years = { 2023, 2024, 2025 }; 
            string baseUrl = "https://date.nager.at/api/v3/PublicHolidays/";

            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            foreach (var year in years)
            {
                try
                {
                    string url = $"{baseUrl}{year}/TR";

                    string jsonResponse = await client.GetStringAsync(url);

                    var yearHolidays = JsonSerializer.Deserialize<List<Holiday>>(jsonResponse, options);

                    if (yearHolidays != null)
                    {
                        allHolidays.AddRange(yearHolidays);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{year} yılı verisi çekilemedi: {ex.Message}");
                }
            }
        }

        private static void YearSelectionMenu()
        {
            Console.Write("\nListelemek istediğiniz yılı girin: ");
            string inputYear = Console.ReadLine() ?? "";

            var filteredHolidays = allHolidays
                .Where(h => h.date != null && h.date.StartsWith(inputYear))
                .ToList();

            if (filteredHolidays.Count > 0)
            {
                Console.WriteLine($"\n--- {inputYear} Yılı Resmi Tatilleri ---");
                PrintTable(filteredHolidays);
            }
            else
            {
                Console.WriteLine("Bu yıla ait veri bulunamadı.");
            }
        }

        private static void SearchByDate()
        {
            Console.WriteLine("\n--- Tarihe Göre Arama ---");
            Console.Write("Tarih girin (Gün ve Ay): ");
            string input = Console.ReadLine() ?? "";

            string cleanInput = input.Replace(".", "-").Replace("/", "-").Replace(" ", "-");

            string[] parts = cleanInput.Split('-');

            if (parts.Length >= 2)
            {
                if (int.TryParse(parts[0], out int searchDay) && int.TryParse(parts[1], out int searchMonth))
                {
                    var foundHolidays = allHolidays.Where(h =>
                    {
                        if (string.IsNullOrEmpty(h.date)) return false;

                        if (DateTime.TryParseExact(h.date, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
                        {
                            return dt.Day == searchDay && dt.Month == searchMonth;
                        }
                        return false;
                    }).ToList();

                    if (foundHolidays.Count > 0)
                    {
                        Console.WriteLine($"\n--- {searchDay}.{searchMonth} Tarihindeki Tatiller ---");
                        PrintTable(foundHolidays);
                    }
                    else
                    {
                        Console.WriteLine("Bu tarihte bir resmi tatil bulunamadı.");
                    }
                }
                else
                {
                    Console.WriteLine("Geçersiz format! Lütfen sayısal tarih giriniz.");
                }
            }
            else
            {
                Console.WriteLine("Hatalı giriş! Lütfen Gün-Ay şeklinde giriniz (Örn: 15-07).");
            }
        }

        private static void SearchByName()
        {
            Console.Write("\nTatil adı girin (Örn: Cumhuriyet): ");
            string keyword = (Console.ReadLine() ?? "").ToLower(); 

            var foundHolidays = allHolidays.Where(h =>
                (h.localName != null && h.localName.ToLower().Contains(keyword)) ||
                (h.name != null && h.name.ToLower().Contains(keyword))
            ).ToList();

            if (foundHolidays.Count > 0)
            {
                Console.WriteLine($"\n--- '{keyword}' İçeren Tatiller ---");
                PrintTable(foundHolidays);
            }
            else
            {
                Console.WriteLine("Bu isimle eşleşen tatil bulunamadı.");
            }
        }

        private static void ListAllHolidays()
        {
            Console.WriteLine("\n--- 2023-2025 Tüm Resmi Tatiller ---");
            PrintTable(allHolidays);
        }

        private static void PrintTable(List<Holiday> holidays)
        {
            Console.WriteLine("{0,-12} {1,-40} {2,-30}", "TARİH", "YEREL İSİM", "ULUSLARARASI İSİM");
            Console.WriteLine(new string('-', 90));

            foreach (var h in holidays)
            {
                string d = h.date ?? "-";
                string ln = h.localName ?? "-";
                string n = h.name ?? "-";

                Console.WriteLine("{0,-12} {1,-40} {2,-30}", d, ln, n);
            }
            Console.WriteLine($"\nToplam {holidays.Count} kayıt listelendi.");
        }
    }
}
