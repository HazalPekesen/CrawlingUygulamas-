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
        var options = new RestClientOptions("https://www.sahibinden.com/")
        {
            MaxTimeout = -1,
            UserAgent = userAgent,
        };
        var client = new RestClient(options);
        var request = new RestRequest("/", Method.Get);
        request.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,/;q=0.8,application/signed-exchange;v=b3;q=0.7");
        request.AddHeader("Cookie", cookie);

        var response = await client.ExecuteAsync(request);
        var htmlContent = response.Content;

        var doc = new HtmlDocument();
        doc.LoadHtml(htmlContent);

        // İLAN BİLGİLERİ ÇEKİLİR
        var ilanListesi = doc.DocumentNode.SelectNodes("//ul[contains(@class, 'vitrin-list')]/li");

        if (ilanListesi != null)
        {
            List<string> ilanBilgileri = new List<string>();

            foreach (var ilan in ilanListesi)
            {
                var ilanBaslik = ilan.SelectSingleNode(".//span")?.InnerText.Trim();

                Console.WriteLine("İlan Başlık: " + ilanBaslik);

                if (!string.IsNullOrEmpty(ilanBaslik))
                {

                    string url = "https://www.sahibinden.com/";

                    var ilanDetailUrl = ilan.SelectSingleNode(".//a")?.GetAttributeValue("href", "")?.Trim();

                    if (ilanDetailUrl != null)
                    {
                        string fullUrl = new Uri(new Uri(url), ilanDetailUrl).ToString();
                        Console.WriteLine("İlan Detay Syf. Tam URL: " + fullUrl);

                        var ilanDetailRequest = new RestRequest(fullUrl, Method.Get);
                        ilanDetailRequest.AddHeader("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,/;q=0.8,application/signed-exchange;v=b3;q=0.7");
                        ilanDetailRequest.AddHeader("Cookie", cookie);

                        var ilanDetailResponse = await client.ExecuteAsync(ilanDetailRequest);
                        var ilanDetailHtml = ilanDetailResponse.Content;

                        var ilanDetailDoc = new HtmlDocument();
                        ilanDetailDoc.LoadHtml(ilanDetailHtml);

                        // ilan fiyatı çekilir
                        var ilanFiyatNode = ilanDetailDoc.DocumentNode.SelectSingleNode("//div[contains(@class, 'classifiedInfo')]//h3/text()");
                        if (ilanFiyatNode != null)
                        {
                            var ilanFiyat = ilanFiyatNode.InnerText.Trim();
                            Console.WriteLine("İlan Fiyatı: " + ilanFiyat);
                        }
                        else
                        {
                            Console.WriteLine("Fiyat bilgisi bulunamadı.");
                        }
                    }

                }
            }

            foreach (var ilanBilgisi in ilanBilgileri)
            {
                Console.WriteLine(ilanBilgisi);
            }

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string dosyaYolu = Path.Combine(desktopPath, "ilanlar.txt");
            File.WriteAllLines(dosyaYolu, ilanBilgileri);
        }

        else
        {
            Console.WriteLine("İlanlar bulunamadı.");
        }
    }
}