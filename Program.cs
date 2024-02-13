using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using HtmlAgilityPack;
using RestSharp;

class Program
{
    static async Task Main(string[] args)
    {
        Console.Write("User-Agent: ");
        string userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/121.0.0.0 Safari/537.36";

        Console.Write("Cookie: ");
        string cookie;

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
                        var ilanDetailResponse = await client.ExecuteAsync(ilanDetailRequest);
                        var ilanDetailHtml = ilanDetailResponse.Content;

                        var ilanDetailDoc = new HtmlDocument();
                        ilanDetailDoc.LoadHtml(ilanDetailHtml);


                        // BURAYA BAKILACAK.
                        var ilanDetailFiyatNode = ilanDetailDoc.DocumentNode.SelectSingleNode("//div[contains(@class, 'classifiedInfo')]//h3");
                        if (ilanDetailFiyatNode != null)
                        {
                            var ilanFiyat = ilanDetailFiyatNode.InnerText.Trim();
                            if (!string.IsNullOrEmpty(ilanFiyat))
                            {
                                ilanBilgileri.Add($"{ilanBaslik}: {ilanFiyat}");
                            }
                            else
                            {
                                ilanBilgileri.Add($"{ilanBaslik}: Fiyat bulunamadı");
                            }
                        }
                        else
                        {
                            ilanBilgileri.Add($"{ilanBaslik}: Fiyat bulunamadı");
                        }


                    }

                }
            }

            Console.WriteLine("İlan Bilgileri:");

            foreach (var ilanBilgisi in ilanBilgileri)
            {
                Console.WriteLine(ilanBilgisi);
            }

            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string dosyaYolu = Path.Combine(desktopPath, "ilanlar.txt");
            File.WriteAllLines(dosyaYolu, ilanBilgileri);

            // FİYATLARIN ORTALAMASI
            var fiyatlar = ilanBilgileri
                .Select(ilan =>
                {
                    var fiyatStr = ilan.Split(':')[1].Trim().Replace("TL", "").Replace(".", "").Replace(",", "").Trim();
                    return decimal.TryParse(fiyatStr, out decimal fiyat) ? fiyat : 0;
                })
                .ToList();

            if (fiyatlar.Any())
            {
                var ortalamaFiyat = fiyatlar.Average();
                Console.WriteLine($"Fiyatların Ortalaması: {ortalamaFiyat} TL");
            }

            else
            {
                Console.WriteLine("İlanlardan herhangi birinde fiyat bulunamadı.");
            }
        }

        else
        {
            Console.WriteLine("İlanlar bulunamadı.");
        }
    }
}
