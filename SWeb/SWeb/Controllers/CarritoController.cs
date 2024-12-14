using Microsoft.AspNetCore.Mvc;
using static System.Net.WebRequestMethods;
using SWeb.Models;
using System.Net.Http.Headers;
using System.Text.Json;
using SWeb.Servicios;
using static System.Net.Mime.MediaTypeNames;

namespace SWeb.Controllers
{
    public class CarritoController : Controller
    {
        private readonly IHttpClientFactory _http;
        private readonly IConfiguration _conf;
        private readonly IMetodosComunes _comunes;
        public CarritoController(IHttpClientFactory http, IConfiguration conf, IMetodosComunes comunes)
        {
            _http = http;
            _conf = conf;
            _comunes = comunes;
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

                var datosCarrito = _comunes.ConsultarCarrito();
                HttpContext.Session.SetString("Total", datosCarrito.Sum(x => x.Total).ToString());

                return Json(result!.Codigo);
            }
        }

        [HttpGet]
        public IActionResult ConsultarCarrito()
        {
            return View(_comunes.ConsultarCarrito());                
        }

        [HttpPost]
        public IActionResult RemoverProductoCarrito(Carrito model)
        {
            using (var client = _http.CreateClient())
            {
                var url = _conf.GetSection("Variables:UrlApi").Value + "Carrito/RemoverProductoCarrito";

                model.ConsecutivoUsuario = long.Parse(HttpContext.Session.GetString("Consecutivo")!.ToString());
                JsonContent datos = JsonContent.Create(model);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("TokenUsuario"));
                var response = client.PostAsync(url, datos).Result;
                var result = response.Content.ReadFromJsonAsync<Respuesta>().Result;

                if (result != null && result.Codigo == 0)
                {
                    var datosCarrito = _comunes.ConsultarCarrito();
                    HttpContext.Session.SetString("Total", datosCarrito.Sum(x => x.Total).ToString());

                    return RedirectToAction("ConsultarCarrito", "Carrito");
                }
                else
                {
                    ViewBag.Mensaje = result!.Mensaje;
                    return View("ConsultarCarrito", _comunes.ConsultarCarrito());
                }
            }
        }

        [HttpPost]
        public IActionResult PagarCarrito(Carrito model)
        {
            using (var client = _http.CreateClient())
            {
                var url = _conf.GetSection("Variables:UrlApi").Value + "Carrito/PagarCarrito";

                model.ConsecutivoUsuario = long.Parse(HttpContext.Session.GetString("Consecutivo")!.ToString());
                JsonContent datos = JsonContent.Create(model);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("TokenUsuario"));
                var response = client.PostAsync(url, datos).Result;
                var result = response.Content.ReadFromJsonAsync<Respuesta>().Result;

                if (result != null && result.Codigo == 0)
                {
                    var datosCarrito = _comunes.ConsultarCarrito();
                    HttpContext.Session.SetString("Total", datosCarrito.Sum(x => x.Total).ToString());

                    return RedirectToAction("Inicio", "Home");
                }
                else
                {
                    ViewBag.Mensaje = result!.Mensaje;
                    return View("ConsultarCarrito", _comunes.ConsultarCarrito());
                }
            }
        }

        [HttpGet]
        public IActionResult ConsultarFacturas()
        {
            using (var client = _http.CreateClient())
            {
                var Consecutivo = long.Parse(HttpContext!.Session.GetString("Consecutivo")!.ToString());
                var url = _conf.GetSection("Variables:UrlApi").Value + "Carrito/ConsultarFacturas?Consecutivo=" + Consecutivo;

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("TokenUsuario"));
                var response = client.GetAsync(url).Result;
                var result = response.Content.ReadFromJsonAsync<Respuesta>().Result;

                if (result != null && result.Codigo == 0)
                {
                    var datosContenido = JsonSerializer.Deserialize<List<Carrito>>((JsonElement)result.Contenido!);
                    return View(datosContenido!.ToList());
                }

                return View(new List<Carrito>());
            }
        }

        [HttpGet]
        public IActionResult ConsultarDetallesFactura(long Consecutivo)
        {
            using (var client = _http.CreateClient())
            {
                var url = _conf.GetSection("Variables:UrlApi").Value + "Carrito/ConsultarDetallesFactura?Consecutivo=" + Consecutivo;

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("TokenUsuario"));
                var response = client.GetAsync(url).Result;
                var result = response.Content.ReadFromJsonAsync<Respuesta>().Result;

                if (result != null && result.Codigo == 0)
                {
                    var datosContenido = JsonSerializer.Deserialize<List<Carrito>>((JsonElement)result.Contenido!);
                    return View(datosContenido!.ToList());
                }

                return View(new List<Carrito>());
            }
        }
    }
}
