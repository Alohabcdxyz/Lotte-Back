
using AspNetCoreHero.ToastNotification.Abstractions;
using LotteriaAPI.Migrations;
using LotteriaView.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LotteriaView.Controllers
{
    
    public class HomeController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7260/api");
        private readonly HttpClient _httpClient;

        private readonly ILogger<HomeController> _logger;
        private readonly INotyfService _notyf;

        public HomeController(ILogger<HomeController> logger, INotyfService notyf)
        {
            _logger = logger;
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
            _notyf = notyf;
        }

    [HttpGet]
    public IActionResult Store()
    {
      return View();
    }

    public IActionResult Switch()
    {
      return View();
    }

    public IActionResult Search()
    {
      return View();
    }

    public IActionResult History()
    {
      return View();
    }

    [HttpGet]
    public IActionResult ChatBot()
    {
      int id = Convert.ToInt32(HttpContext.Session.GetString("key"));
      RegisterViewModel regis = new RegisterViewModel();
      HttpResponseMessage responseMessage = _httpClient.GetAsync(_httpClient.BaseAddress + $"/Register/GetDetailUser/{id}").Result;
      if (responseMessage.IsSuccessStatusCode)
      {
        string data = responseMessage.Content.ReadAsStringAsync().Result;
        regis = JsonConvert.DeserializeObject<RegisterViewModel>(data);
      }
      return View(regis);
    }


    public IActionResult Index()
        {
            List<ProductViewModel> prd = new List<ProductViewModel>();
            HttpResponseMessage responseMessage = _httpClient.GetAsync(_httpClient.BaseAddress + "/Product/GetProducts").Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                string data = responseMessage.Content.ReadAsStringAsync().Result;
                prd = JsonConvert.DeserializeObject<List<ProductViewModel>>(data).Take(3).ToList();
            }

   
        List<NewsViewModel> news = new List<NewsViewModel>();
        HttpResponseMessage responseMessage1 = _httpClient.GetAsync(_httpClient.BaseAddress + "/New/GetAllNews").Result;
        if (responseMessage1.IsSuccessStatusCode)
        {
          string data = responseMessage1.Content.ReadAsStringAsync().Result;
          news = JsonConvert.DeserializeObject<List<NewsViewModel>>(data).Take(4).ToList();
        }
          
        return View(new NewsProductViewModel() { News = news, Products = prd });
        }

        [HttpGet]
        public IActionResult ProductDetail(int id)
        {
          ProductViewModel prd = new ProductViewModel();
          HttpResponseMessage responseMessage = _httpClient.GetAsync(_httpClient.BaseAddress + $"/Product/GetProduct/{id}").Result;
          if (responseMessage.IsSuccessStatusCode)
          {
            string data = responseMessage.Content.ReadAsStringAsync().Result;
            prd = JsonConvert.DeserializeObject<ProductViewModel>(data);
          }
          return View(prd);
        }

    [HttpGet]
    public IActionResult NewsDetail(int id)
    {
      NewsViewModel prd = new NewsViewModel();
      HttpResponseMessage responseMessage = _httpClient.GetAsync(_httpClient.BaseAddress + $"/New/GetNews/{id}").Result;
      if (responseMessage.IsSuccessStatusCode)
      {
        string data = responseMessage.Content.ReadAsStringAsync().Result;
        prd = JsonConvert.DeserializeObject<NewsViewModel>(data);
      }
      return View(prd);
    }

    [HttpGet]
        public IActionResult Order(int? id)
        {
            List<ProductViewModel> prd = new List<ProductViewModel>();
            HttpResponseMessage responseMessage = _httpClient.GetAsync(_httpClient.BaseAddress + "/Product/GetProducts").Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                string data = responseMessage.Content.ReadAsStringAsync().Result;
                prd = JsonConvert.DeserializeObject<List<ProductViewModel>>(data);
            }

            List<CategoryViewModel> categoryView = new List<CategoryViewModel>();
            HttpResponseMessage responseMessage1 = _httpClient.GetAsync(_httpClient.BaseAddress + "/Category/GetAllCategory").Result;
            if (responseMessage1.IsSuccessStatusCode)
            {
                string data = responseMessage1.Content.ReadAsStringAsync().Result;
                categoryView = JsonConvert.DeserializeObject<List<CategoryViewModel>>(data);
            }

            if (id != null)
            {
                prd = prd.Where(x => x.CategoryId == id).ToList();
            }

            return View(new CategoryProductViewModel() { Categories = categoryView, Products = prd });
        }


        [HttpGet]
        public async Task<IActionResult> Cart()
        {
            int id = Convert.ToInt32(HttpContext.Session.GetString("key"));
            CartViewModel cart = new CartViewModel();
            HttpResponseMessage responseMessage = await _httpClient.GetAsync(_httpClient.BaseAddress + $"/Cart/GetCart/{id}");
            if (responseMessage.IsSuccessStatusCode)
            {
                var data = await responseMessage.Content.ReadAsStringAsync();
                cart = JsonConvert.DeserializeObject<CartViewModel>(data);
            }
      return View(new SubmitViewModel()
      {
        CartViewModel = cart
      }); ;
        }

    [HttpGet]
    public async Task<IActionResult> Profile()
    {
      int id = Convert.ToInt32(HttpContext.Session.GetString("key"));
      RegisterViewModel regis = new RegisterViewModel();
      HttpResponseMessage responseMessage = _httpClient.GetAsync(_httpClient.BaseAddress + $"/Register/GetDetailUser/{id}").Result;
      if (responseMessage.IsSuccessStatusCode)
      {
        string data = responseMessage.Content.ReadAsStringAsync().Result;
        regis = JsonConvert.DeserializeObject<RegisterViewModel>(data);
      }
      return View(regis);
    }
    public IActionResult EditProfile(int id)
    {
      RegisterViewModel profile = new RegisterViewModel();
      HttpResponseMessage responseMessage = _httpClient.GetAsync(_httpClient.BaseAddress + "/Register/GetDetailUser/" + id).Result;
      if (responseMessage.IsSuccessStatusCode)
      {
        string data = responseMessage.Content.ReadAsStringAsync().Result;
        profile = JsonConvert.DeserializeObject<RegisterViewModel>(data);
      }
      return View(profile);
    }

    [HttpPost]
    public IActionResult EditProfile(RegisterViewModel model)
    {
      string data = JsonConvert.SerializeObject(model);
      StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
      HttpResponseMessage responseMessage = _httpClient.PutAsync(_httpClient.BaseAddress + $"/Register/UpdateContact/{model.UserId}", content).Result;
      if (responseMessage.IsSuccessStatusCode)
      {

        return RedirectToAction("Profile");

      }
      return View();
    }

    [HttpGet]
        public async Task<IActionResult> AddToCart(int prdId)
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("key"));

                string data = JsonConvert.SerializeObject(new CartDetailViewModel()
                {
                    ProductId = prdId,
                    Quantity = 1
                });

                var content = new StringContent(data, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(_httpClient.BaseAddress + $"/Cart/AddToCart/add-to-cart?userId={userId}",content);

                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    ViewData["Message"] = responseContent;
                    _notyf.Success("Thêm sản phẩm vào giỏ hàng thành công 👌", 3);
                    return RedirectToAction("Cart");
                }
                else
                {
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    ViewData["ErrorMessage"] = errorResponse;
                    _notyf.Error("Thêm sản phẩm vào giỏ hàng thất bại 👎", 3);
        }
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = $"Error: {ex.Message}";
                _notyf.Error("Thêm sản phẩm vào giỏ hàng thất bại 👎", 3);
      }
            return View();

        }

        [HttpGet]
        public async Task<IActionResult> RemoveToCart(int prdId)
        {
            try
            {
                // Retrieve the user ID from the session
                int userId = Convert.ToInt32(HttpContext.Session.GetString("key"));

                string data = JsonConvert.SerializeObject(new CartDetailViewModel()
                {
                    ProductId = prdId,
                });

                var response = await _httpClient.DeleteAsync(_httpClient.BaseAddress + $"/Cart/DeleteCartItem?userId={userId}&productId={prdId}");


                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    ViewData["Message"] = responseContent;
                    _notyf.Success("Xóa sản phẩm khỏi giỏ hàng thành công 👌", 3);
                    return RedirectToAction("Cart");
                }
                else
                {
                    string errorResponse = await response.Content.ReadAsStringAsync();
          _notyf.Error("Xóa sản phẩm khỏi giỏ hàng thất bại 👎", 3);
          ViewData["ErrorMessage"] = errorResponse;
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions, e.g., log the error
                ViewData["ErrorMessage"] = $"Error: {ex.Message}";
        _notyf.Error("Xóa sản phẩm khỏi giỏ hàng thất bại 👎", 3);
      }
            return View();

        }

        [HttpGet]
        public async Task<IActionResult> IncreaseQuantity(int prdId)
        {
            try
            {
                // Retrieve the user ID from the session
                int userId = Convert.ToInt32(HttpContext.Session.GetString("key"));

                string data = JsonConvert.SerializeObject(new CartDetailViewModel()
                {
                    ProductId = prdId,
                });

                var response = await _httpClient.PostAsync(_httpClient.BaseAddress + $"/Cart/IncreaseQuantity?userId={userId}&productId={prdId}",null);


                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    ViewData["Message"] = responseContent;
                    return RedirectToAction("Cart");
                }
                else
                {
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    ViewData["ErrorMessage"] = errorResponse;
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions, e.g., log the error
                ViewData["ErrorMessage"] = $"Error: {ex.Message}";
            }
            return View();

        }

        [HttpGet]
        public async Task<IActionResult> DecreaseQuantity(int prdId)
        {
      try
      {
        // Retrieve the user ID from the session
        int userId = Convert.ToInt32(HttpContext.Session.GetString("key"));

        string data = JsonConvert.SerializeObject(new CartDetailViewModel()
        {
          ProductId = prdId,
        });

        var response = await _httpClient.PostAsync(_httpClient.BaseAddress + $"/Cart/DecreaseQuantity?userId={userId}&productId={prdId}", null);


        if (response.IsSuccessStatusCode)
        {
          string responseContent = await response.Content.ReadAsStringAsync();
          ViewData["Message"] = responseContent;
          return RedirectToAction("Cart");
        }
        else
        {
          string errorResponse = await response.Content.ReadAsStringAsync();
          ViewData["ErrorMessage"] = errorResponse;
        }

      }
      catch (Exception ex)
      {
        // Handle any exceptions, e.g., log the error
        ViewData["ErrorMessage"] = $"Error: {ex.Message}";
      }
      return View();

    }

    [HttpPost]
    public async Task<IActionResult> AddToBill(SubmitViewModel submitViewModel)
    {
      int userId = Convert.ToInt32(HttpContext.Session.GetString("key"));
      var bill = new BillViewModel()
      {
        UserId = userId,
        Total = submitViewModel.Total,
        Status = 0, 
        Phone = submitViewModel.Phone, 
        Address = submitViewModel.Address,
        Note = submitViewModel.Note

  };
      string data = JsonConvert.SerializeObject(bill);
      var content = new StringContent(data, Encoding.UTF8, "application/json");
      var response = await _httpClient.PostAsync(_httpClient.BaseAddress + $"/Bill/AddToBill", content);
      BillViewModel billViewModel = new BillViewModel();
       HttpResponseMessage responseMessage0 = await _httpClient.GetAsync(_httpClient.BaseAddress + $"/Bill/GetBill/{userId}");
      if (responseMessage0.IsSuccessStatusCode)
      {
        var data0 = await responseMessage0.Content.ReadAsStringAsync();
        billViewModel = JsonConvert.DeserializeObject<BillViewModel>(data0);
      }

      CartViewModel cart = new CartViewModel();
      HttpResponseMessage responseMessage = await _httpClient.GetAsync(_httpClient.BaseAddress + $"/Cart/GetCart/{userId}");
      if (responseMessage.IsSuccessStatusCode)
      {
        var data1 = await responseMessage.Content.ReadAsStringAsync();
        cart = JsonConvert.DeserializeObject<CartViewModel>(data1);
      }
      foreach (var item in cart.CartDetails)
      {
        var billDetail = new BillDetailViewModels()
        {
          Quantity = item.Quantity,
          Price = item.Price,
          ProductId = item.ProductId,
          BillId = billViewModel.Id
        };
        string data2 = JsonConvert.SerializeObject(billDetail);
        var content2 = new StringContent(data2, Encoding.UTF8, "application/json");
        var response2 = await _httpClient.PostAsync(_httpClient.BaseAddress + $"/Bill/AddToBillDetail", content2);
      }
      HttpResponseMessage responseMessage4 = await _httpClient.DeleteAsync(_httpClient.BaseAddress + $"/Cart/RemoveCart?userId={userId}&cartId={cart.Id}");
      _notyf.Success("Mua hàng thành công 👌", 3);
      return RedirectToAction("HistoryOrder");
    }

    [HttpGet]
    public async Task<IActionResult> HistoryOrder(int? status)
    {
      int id = Convert.ToInt32(HttpContext.Session.GetString("key"));
      List<BillViewModel> bill = new List<BillViewModel>();
      HttpResponseMessage responseMessage = await _httpClient.GetAsync(_httpClient.BaseAddress + $"/Bill/GetAllBillByUser/{id}");
      if (responseMessage.IsSuccessStatusCode)
      {
        var data = await responseMessage.Content.ReadAsStringAsync();
        bill = JsonConvert.DeserializeObject<List<BillViewModel>>(data);
      }
      if(status != null)
      {
        bill = bill.Where(x => x.Status == status).ToList();
      }
      return View(bill); 
    }

    [HttpPost]
    public async Task<IActionResult> CancelOrder(BillViewModel bill)
    {
      try
      {
        string data = JsonConvert.SerializeObject(new CancelBillViewModel()
        {
          BillId = bill.Id,
          Note = bill.Note
        }
        );
        var content = new StringContent(data, Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync(_httpClient.BaseAddress + $"/Bill/CancelOrder/{bill.Id}", content); // Pass the bill.Id in the URL

        if (response.IsSuccessStatusCode)
        {
          string responseContent = await response.Content.ReadAsStringAsync();
          ViewData["Message"] = responseContent;
          return Redirect("HistoryOrder?status=3");
        }
        else
        {
          string errorResponse = await response.Content.ReadAsStringAsync();
          ViewData["ErrorMessage"] = errorResponse;
        }
      }
      catch (Exception ex)
      {
        // Handle any exceptions, e.g., log the error
        ViewData["ErrorMessage"] = $"Error: {ex.Message}";
      }
      return Redirect("HistoryOrder?status=3");
    }

    [HttpGet]
    public async Task<IActionResult> ConfirmOrder(int billId)
    {
      try
      {
        
        var response = await _httpClient.PostAsync(_httpClient.BaseAddress + $"/Bill/DoneOrder?id={billId}", null);


        if (response.IsSuccessStatusCode)
        {
          string responseContent = await response.Content.ReadAsStringAsync();
          ViewData["Message"] = responseContent;
          return Redirect("HistoryOrder?status=2");
        }
        else
        {
          string errorResponse = await response.Content.ReadAsStringAsync();
          ViewData["ErrorMessage"] = errorResponse;
        }
      }
      catch (Exception ex)
      {
        // Handle any exceptions, e.g., log the error
        ViewData["ErrorMessage"] = $"Error: {ex.Message}";
      }
      return View();

    }




    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
