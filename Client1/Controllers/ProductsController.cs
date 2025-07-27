using Client1.Models.Products;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Client1.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IConfiguration _configuration;

        public ProductsController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IActionResult> GetProducts()
        {
            //HttpClient class'ı endpointlere http requestleri atmak için kullanılır.
            HttpClient httpclient = new HttpClient();
            
            //AuthServer'da discovery endpoint'ine istek atıp dönen verileri alıyoruz.
            var discovery = await httpclient.GetDiscoveryDocumentAsync("https://localhost:7193");

            if (discovery.IsError)
            {
                ViewBag.Error = discovery.Error;
                return View();
            }

            /*Client1 Credentials Grant türü ile token almak için bir istek nesnesi oluşturuluyor. Bu işlem ile AuthServer eğer bu client'ı
             tanıyorsa yani ClientId ve ClientSecret değerleri config dosyasında verilmişse Client1 AuthServer'dan ilgili API'lere 
             istek atmak için token alabilir */
            ClientCredentialsTokenRequest clientCredentialsTokenRequest = new ClientCredentialsTokenRequest()
            {
                ClientId = _configuration["Client:ClientId"],
                ClientSecret = _configuration["Client:ClientSecret"],
                Address = discovery.TokenEndpoint //Address olarak discovery’dan elde edilen TokenEndpoint adresi kullanılıyor.
            };

            //Yukarıda oluşturulan istek kullanılarak token alınır.
            var token =  await httpclient.RequestClientCredentialsTokenAsync(clientCredentialsTokenRequest);

           /*HttpClient nesnesine, bir sonraki istekte kullanılacak olan Bearer tipinde access token eklenir.Bu işlem IdentityModel 
            kütüphanesinden gelen method ile yapılıyor bu hali daha kolay yoksa httpclient'a token'ı eklerken daha fazla işlem yapılması lazım */
            httpclient.SetBearerToken(token.AccessToken);

            /*
            Bu satırda, daha önce elde edilen access token ile yetkilendirilmiş bir şekilde API'den ürün listesi isteniyor. Yani client1
            AuthServer'dan aldığı token ile API1'deki endpoint'e request gönderiyor.
             */
            var response = await httpclient.GetAsync("https://localhost:7233/api/Product/GetProducts");

            //response'dan json datası deserialize edilerek GetProductViewModel model nesnesine dönüştürüyor ve view'a dönülüyor.
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<List<GetProductViewModel>>(content);
                return View(products);
            }
            else
                ViewBag.Error = "Ürün listesi bulunamadı";
                return View();
        }
    }
}
