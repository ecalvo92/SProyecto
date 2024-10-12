using Microsoft.AspNetCore.Mvc;
using SWeb.Models;

namespace SWeb.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHttpClientFactory _http;
        private readonly IConfiguration _conf;
        public LoginController(IHttpClientFactory http, IConfiguration conf)
        {
            _http = http;
            _conf = conf;
        }



        [HttpGet]
        public IActionResult CrearCuenta()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CrearCuenta(Usuario model)
        {
            using (var client = _http.CreateClient())
            {
                var url = _conf.GetSection("Variables:UrlApi").Value + "Login/CrearCuenta";
                JsonContent datos = JsonContent.Create(model);

                var response = client.PostAsync(url, datos).Result;

                if (response.IsSuccessStatusCode)
                { 
                    var result = response.Content.ReadFromJsonAsync<Usuario>().Result;
                }
            }

            return View();
        }



        [HttpGet]
        public IActionResult InicioSesion()
        {
            return View();
        }

        [HttpPost]
        public IActionResult InicioSesion(Usuario model)
        {
            return View();
        }



        [HttpGet]
        public IActionResult RecuperarAcceso()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RecuperarAcceso(Usuario model)
        {
            return View();
        }

    }
}
