using Microsoft.AspNetCore.Mvc;

namespace HighBoard.Web.Mvc.Controllers
{
    public class DashboardController : Controller
    {
        private readonly HttpClient _baseHttpClient;

        public DashboardController(IHttpClientFactory httpClientFactory)
        {
            _baseHttpClient = httpClientFactory.CreateClient("BaseHttpClient");
        }

        // GET: Dashboard
        public async Task<IActionResult> Index()
        {
            using var httpClient = await _baseHttpClient.GetAsync("Categories/GetCurrentCategories?citizensWillSee=true");
            if (httpClient.IsSuccessStatusCode)
            {
                var result = await httpClient.Content.ReadAsStringAsync();
            }

            ViewBag.Konular = new List<string>
            {
                "İslam İnancı",
                "Mezhepler Ve Dini Oluşumlar",
                "Temizlik (Taharet)",
                "Namaz",
                "Oruç",
                "Zekat Ve Sadaka",
                "Hac Ve Umre",
                "Kurban, Adak Ve Yemin",
                "İbadet, Dua, Tövbe",
                "Kur'an-ı Kerîm'in Muhteması, İcaizi Ve Tarihi",
                "Aile Hayatı",
                "Ticari Hayat",
                "Tasavvuf",
                "Vakıf, Miras Ve Vasiyet",
                "Kadınlara Özgü Haller",
                "Haklar Ve Sorumluluklar",
                "Helaller Ve Haramlar",
                "Helal Ürün Ve Hizmet",
                "Tip Ve Sağlık",
                "İnançla İlgili Diğer Meseleler",
                "Kur'an'ın Kıraati"
            };

            return View();
        }
    }
}