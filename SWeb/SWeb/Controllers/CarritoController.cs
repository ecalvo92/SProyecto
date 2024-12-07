using Microsoft.AspNetCore.Mvc;
using static System.Net.WebRequestMethods;
using SWeb.Models;
using System.Net.Http.Headers;

namespace SWeb.Controllers
{
    public class CarritoController : Controller
    {
        private readonly IHttpClientFactory _http;
        private readonly IConfiguration _conf;
        public CarritoController(IHttpClientFactory http, IConfiguration conf)
        {
            _http = http;
            _conf = conf;
        }

        [HttpPost]
        public IActionResult RegistrarCarrito(long ConsecutivoProducto, int Cantidad)
        {
            using (var client = _http.CreateClient())
            {
                var url = _conf.GetSection("Variables:UrlApi").Value + "Carrito/RegistrarCarrito";

                var model = new Carrito();
                model.ConsecutivoUsuario = long.Parse(HttpContext.Session.GetString("Consecutivo")!.ToString());
                model.ConsecutivoProducto = ConsecutivoProducto;
                model.Unidades = Cantidad;

                JsonContent datos = JsonContent.Create(model);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("TokenUsuario"));
                var response = client.PostAsync(url, datos).Result;
                var result = response.Content.ReadFromJsonAsync<Respuesta>().Result;

                return Json(result!.Codigo);
            }
        }

    }
}
