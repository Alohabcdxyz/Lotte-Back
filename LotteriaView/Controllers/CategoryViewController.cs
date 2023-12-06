using LotteriaView.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace LotteriaView.Controllers
{
    public class CategoryViewController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7260/api");
        private readonly HttpClient _httpClient;
        
        public CategoryViewController()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<CategoryViewModel> categoryView = new List<CategoryViewModel>();
            HttpResponseMessage responseMessage = _httpClient.GetAsync(_httpClient.BaseAddress + "/Category/GetAllCategory").Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                string data = responseMessage.Content.ReadAsStringAsync().Result;
                categoryView = JsonConvert.DeserializeObject<List<CategoryViewModel>>(data);
            }
            return View(categoryView);
        }
    }
}
