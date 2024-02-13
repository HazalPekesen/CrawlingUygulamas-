using HtmlAgilityPack;
using RestSharp;

class Program
{
    static async Task Main()
    {
        // Kullanıcıdan User-Agent ve Cookie bilgilerini al
        Console.Write("User-Agent: ");
        string userAgent = Console.ReadLine();

        Console.Write("Cookie: ");
        string cookie = Console.ReadLine();

        // RestSharp işlemini başlat ve tamamla
        await ProcessRestSharp(userAgent, cookie);

        Console.WriteLine("İşlem tamamlandı. Çıkış yapmak için bir tuşa basın.");
        Console.ReadKey();
    }

    async static Task ProcessRestSharp(string userAgent, string cookie)
    {
        // RestSharp istemcisini yapılandır
        var options = new RestClientOptions("https://www.sahibinden.com/")
        {
            MaxTimeout = -1,
            UserAgent = userAgent,
        };

        var client = new RestClient(options);
        var request = new RestRequest("/", Method.Get)
            .AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,/;q=0.8,application/signed-exchange;v=b3;q=0.7")
            .AddHeader("Cookie", cookie);

        // Ana sayfadan ilan listesini çek
        var response = await client.ExecuteAsync(request);
        var htmlContent = response.Content;

        var doc = new HtmlDocument();
        doc.LoadHtml(htmlContent);

        var ilanListesi = doc.DocumentNode.SelectNodes("//ul[contains(@class, 'vitrin-list')]/li");

        if (ilanListesi != null)
        {
            List<string> ilanBilgileri = new List<string>();
            List<decimal> ilanFiyatlar = new List<decimal>();

            foreach (var ilan in ilanListesi)
            {
                // Her bir ilanı işlemek için ayrı bir fonksiyon çağır
                ProcessIlan(ilan, cookie, client, ilanBilgileri, ilanFiyatlar);
            }

            // İlan fiyatlarının ortalamasını hesapla
            decimal averageFiyat = ilanFiyatlar.Any() ? ilanFiyatlar.Average() : 0;
            Console.WriteLine($"Ortalama Fiyat: {averageFiyat:C}");

            // İlan bilgilerini dosyaya yaz
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string dosyaYolu = Path.Combine(desktopPath, "ilanlar.txt");
            File.WriteAllLines(dosyaYolu, ilanBilgileri);
        }
        else
        {
            Console.WriteLine("İlanlar bulunamadı.");
        }
    }

    async static void ProcessIlan(HtmlNode ilan, string cookie, RestClient client, List<string> ilanBilgileri, List<decimal> ilanFiyatlar)
    {
        var ilanBaslik = ilan.SelectSingleNode(".//span")?.InnerText.Trim();
        Console.WriteLine("İlan Başlık: " + ilanBaslik);

        if (string.IsNullOrEmpty(ilanBaslik))
            return;

        string url = "https://www.sahibinden.com/";
        var ilanDetailUrl = ilan.SelectSingleNode(".//a")?.GetAttributeValue("href", "")?.Trim();

        if (ilanDetailUrl == null)
            return;

        string fullUrl = new Uri(new Uri(url), ilanDetailUrl).ToString();
        Console.WriteLine("İlan Detay Sayfa Tam URL: " + fullUrl);

        var ilanDetailResponse = await client.ExecuteAsync(new RestRequest(fullUrl, Method.Get)
            .AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,/;q=0.8,application/signed-exchange;v=b3;q=0.7")
            .AddHeader("Cookie", cookie));

        var ilanDetailHtml = ilanDetailResponse.Content;
        var ilanDetailDoc = new HtmlDocument();
        ilanDetailDoc.LoadHtml(ilanDetailHtml);

        var ilanFiyatNode = ilanDetailDoc.DocumentNode.SelectSingleNode("//div[contains(@class, 'classifiedInfo')]//h3/text()");

        if (ilanFiyatNode != null && decimal.TryParse(ilanFiyatNode.InnerText.Trim(), out decimal ilanFiyat))
        {
            Console.WriteLine("İlan Fiyatı: " + ilanFiyat);
            ilanFiyatlar.Add(ilanFiyat);

            // İlan başlık ve fiyatını içeren bilgiyi listeye ekle
            string ilanBilgisi = $"İlan Başlık: {ilanBaslik}, İlan Fiyatı: {ilanFiyat:C}";
            ilanBilgileri.Add(ilanBilgisi);
        }
        else
        {
            Console.WriteLine("Fiyat bilgisi geçerli bir sayı değil veya bulunamadı.");
        }
    }
}