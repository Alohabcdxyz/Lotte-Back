using AspNetCoreHero.ToastNotification.Abstractions;
using LotteriaAPI.Models;
using LotteriaView.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Drawing;
using System.Text;

namespace LotteriaView.Controllers
{
    public class RegisterViewController : Controller
    {
        Uri baseAddress = new Uri("https://localhost:7260/api");
        private readonly HttpClient _httpClient;
        private readonly INotyfService _notyf;
        public RegisterViewController(INotyfService notyf)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = baseAddress;
            _notyf = notyf;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel registerViewModel)
        {
            try
            {
                string data = JsonConvert.SerializeObject(registerViewModel);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = _httpClient.PostAsync(_httpClient.BaseAddress + "/Register/AddUser", content).Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    _notyf.Success("Đăng ký thành công 👌 ", 3);
                    return RedirectToAction("Login");

                }
            }
            catch (Exception ex)
            {
                return View();
            }
            return View();
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            RegisterViewModel contact = new RegisterViewModel();
            HttpResponseMessage responseMessage = _httpClient.GetAsync(_httpClient.BaseAddress + "/Register/GetDetailUser/" + id).Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                string data = responseMessage.Content.ReadAsStringAsync().Result;
                contact = JsonConvert.DeserializeObject<RegisterViewModel>(data);
            }
            return View(contact);
        }

        [HttpPost]
        public IActionResult Edit(RegisterViewModel model)
        {
            string data = JsonConvert.SerializeObject(model);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage responseMessage = _httpClient.PutAsync(_httpClient.BaseAddress + $"/Register/UpdateContact/{model.UserId}", content).Result;
            if (responseMessage.IsSuccessStatusCode)
            {

                return RedirectToAction("Index");

            }
            return View();
        }

        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            RegisterViewModel contact = new RegisterViewModel();
            HttpResponseMessage responseMessage = _httpClient.GetAsync(_httpClient.BaseAddress + "/Register/GetDetailUser/" + id).Result;
            if (responseMessage.IsSuccessStatusCode)
            {
                string data = responseMessage.Content.ReadAsStringAsync().Result;
                contact = JsonConvert.DeserializeObject<RegisterViewModel>(data);
            }
            return View(contact);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirm(Guid id)
        {

            HttpResponseMessage responseMessage = _httpClient.DeleteAsync(_httpClient.BaseAddress + $"/Register/DeleteContact/{id}").Result;
            if (responseMessage.IsSuccessStatusCode)
            {

                return RedirectToAction("Index");

            }
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(RegisterViewModel registerViewModel)
        {
            try
            {
                string data = JsonConvert.SerializeObject(registerViewModel);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
                HttpResponseMessage responseMessage = _httpClient.PostAsync(_httpClient.BaseAddress + "/Register/Login", content).Result;
                if (responseMessage.IsSuccessStatusCode)
                {
                    var listEntity = JsonConvert.DeserializeObject<Register>(await responseMessage.Content.ReadAsStringAsync());
                    HttpContext.Session.SetString("key",listEntity.UserId.ToString());
                    _notyf.Success("Chào mừng bạn tới với Lotteria 👋", 3);
                    return RedirectToAction("Index", "Home");

                }
            }
            catch (Exception ex)
            {
                 return Content("Loi");
            }   
            return Content("Loi");
        }
        [HttpGet]
        public async Task<IActionResult> Logout(RegisterViewModel registerViewModel)
        {
            HttpContext.Session.Remove("key");
            _notyf.Success("Hẹn gặp lại bạn", 3);
            return RedirectToAction("Index", "Home");
        }





    }
}
