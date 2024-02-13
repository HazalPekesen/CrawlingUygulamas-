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
        string cookie = "vid=228; cdid=jxDienTcVwLfCj5W659812d3; csls=5DI3PDQ5xLyqTy1RQTVAivBQ5A5MzjdKnHnCJiPDgMzYftixJgTkKYSjn7H2hDq_Jg9EU_EFX2wZYe4QCBlcZSHjDWmwPiAINJtk8nryDSn9fnDA3gw9MYPUe1QADubD77pFAUrUu9HGizPMjQbJI_6guhZ_P62EIn7HvX3WKkqPSMkhgfeWU1BMDmTugwmVNFwq26rkrowttOEBrRTLpoIbIHvOab1LCWjHApFDJmA; nwsh=std; showPremiumBanner=false; _gcl_au=1.1.365759077.1704465109; __ssid=d49ecec58ff2ea5004f632b14291d0d; OptanonAlertBoxClosed=2024-01-05T14:32:21.562Z; bannerClosed=false; MS1=https://www.google.com/; _gid=GA1.2.737288824.1707597857; _fbp=fb.1.1707602589053.1154134344; st=b60474d32422f40b69ab2bc04a24e29f00f5c54b0bac50aec65262df44471b3800b0dab99f1d23f346de559b2784ac96e6b79afa27aecb499; myPriceHistorySplashClosed=1; acc_type=bireysel_uye; kno=aRgXxhsvQQQq-h-gGxknz0A; ssp=ms; DHABD=true; sbt=none; disableTooltip=true; xsrf-token=437da4cc9e27d26bb37aa8fad3efe3d31cb25087; __cflb=0H28vudCb12J6LVB9qNuBCU3iDnBjmLgeRrVcxKteS7; geoipCity=istanbul; geoipIsp=turk_telekom; gcd=20240213150945; MDR=20221025; lastVisit=20240213; userType=yeni_bireysel; shuid=cXeHwR4mNoiXEFQnzK3t1Lw; dopingPurchase=false; getPurchase=false; priceHistorySplashClosed=1; dp=1366*768-landscape; ulfuid=null; csss=AGojHYTu4-l9zcjoTC_wm-Ml2sN3JTgvX-JnNR5GlQd4l32fLWFeIlLm5pCDt2WIprTL6FStknqoYbQ6YUbBCEX9e9KH_2ju5-9VyjscxEcqEhVu9PtjYoaLIt_xi8pKd18OAGbmNkPDFwaxDzBCIzConIVyaCPpBbnCKFLAA_wJLpFkPRtA3CvoDCCHRe5e7LpvA7x7fB29I3qBHDyCw27qM8Ze5iDlIubHjKSYaGA; csid=7Ex42VFZGxvfgfXuWNx7iIxJCs_h60wMxZJQ9fyZ8B9FDMVrJxG2g646yEqC7LjQNyHoGkL9gx43UfiZUeP9GgOey2maKVuXaUJloiAlkH1fbQf5k84sJUdaC8pCfIAnuDhbr-U5LukltqEKCFmhNOoOX8uC8AZ1bZ5YS0bbYm4JSVAGeZOE3Sk6dkwaID9lJU8ukdMhxfpjo9yUcnvqgkRAWMJmUfK69S7qL9BEt6EdgJE1tYMkMHhen3StipCkcf96n5R8Ij3cq_UI5qMyq1ngRjFpf-Kl00reD09eUze0xLNU850fN5W3UCEizEZIO52qHhLDn44-n_gm_g8r-j2ItEZp8kw6DscvQqYBNGam-5fN7FfxU8t9QGLHfLcMTUEGC_wIBt0OKa_rTfPlsxRmfFYkokRf9HRmN9PxFoqHskK6wRTbrK8koTyTRgzeJ-fAQGeDHKp6Wye998rWhj7aDRYgWV7ybs86_XkCQAWdF0V36TLoQcY6-7wptEeN1e-IHqdfZgOLLX5EBH6XEq_W4W3VTDc9Iks4NKrJDMrzGcGornCoDwin58lA4ugy48ejy_czo_q0d-XIQZ38One85Pfsr_bhBL36tWQjM6sxCroJbbQkNXFbV1IYczRNCsQQJ9wT1h29XImhagb3kiFFJP1uQNoWORDMmE2GoKI; __gads=ID=a46ba2c0b49b45cd:T=1704465111:RT=1707827043:S=ALNI_Mbxpz3RteTdCA4DL-4NwIwjKHxdIA; __gpi=UID=00000d39ebff6311:T=1704465111:RT=1707827043:S=ALNI_Mbjna4Y_kbTl-SC8LboHwflk5qvvA; __eoi=ID=702b4ed98ecee7b3:T=1707252907:RT=1707827043:S=AA-AfjYvkXySUneaQTGTCx3-df8j; _dc_gtm_UA-235070-1=1; __cf_bm=7NcREi31qDN9OfTmY4X9Eg1cqAfPr3a6wscQoOrqgqk-1707827068-1-ARvmYs7y4AA6pC6IUDbnE8oj3D5Myns492MJjNZmd7xx8WAhMST2G50q2ZUX76jGMNNHzWjDrZpXeg9sFSu5gKU=; cf_clearance=JrLWgPTa58UzJbSmINb7D4uflQgOUac_S9OU81YZV04-1707827070-1-AckdpOclswHHpQyA5/9VwtuqsJ1Siq7fTT7VtbTkqBDJPhenl9rQzcalPCnpfYJTv1rfS9Kt63R1R7hZxez0xks=; OptanonConsent=isGpcEnabled=0&datestamp=Tue+Feb+13+2024+15%3A24%3A30+GMT%2B0300+(GMT%2B03%3A00)&version=202310.2.0&browserGpcFlag=0&isIABGlobal=false&consentId=df3727ce-9674-4661-9af2-f47a12bf92c6&interactionCount=2&landingPath=NotLandingPage&groups=C0004%3A0%2CC0001%3A1%2CC0003%3A0%2CC0002%3A0&hosts=H131%3A0%2CH108%3A0%2CH97%3A0%2CH110%3A0%2CH76%3A0%2CH77%3A0%2CH147%3A0%2CH78%3A0%2CH98%3A0%2CH79%3A0%2CH106%3A0%2CH58%3A0%2CH174%3A0%2CH8%3A0%2CH80%3A0%2CH67%3A0%2CH27%3A0%2CH14%3A0%2CH82%3A0%2CH83%3A0%2CH99%3A0%2CH10%3A0%2CH31%3A0%2CH114%3A0%2CH84%3A0%2CH175%3A0%2CH176%3A0%2CH115%3A0%2CH117%3A0%2CH100%3A0%2CH87%3A0%2CH88%3A0%2CH119%3A0%2CH89%3A0%2CH3%3A1%2CH4%3A0%2CH90%3A0%2CH91%3A0%2CH102%3A0%2CH5%3A0%2CH140%3A0%2CH93%3A0%2CH103%3A0%2CH94%3A0%2CH61%3A0%2CH36%3A0%2CH141%3A0%2CH122%3A0%2CH96%3A0%2CH104%3A0%2CH125%3A0%2CH105%3A1%2CH142%3A0&genVendors=&AwaitingReconsent=false&geolocation=TR%3B34";

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
