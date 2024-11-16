using Microsoft.AspNetCore.Mvc;
using SWeb.Models;
using SWeb.Servicios;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Cryptography.Xml;
using System.Text.Json;

namespace SWeb.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IHttpClientFactory _http;
        private readonly IConfiguration _conf;
        private readonly IMetodosComunes _comunes;
        public UsuarioController(IHttpClientFactory http, IConfiguration conf, IMetodosComunes comunes)
        {
            _http = http;
            _conf = conf;
            _comunes = comunes;
        }


        [HttpGet]
        public IActionResult CambiarContrasenna()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CambiarContrasenna(Usuario model)
        {
            model.Contrasenna = _comunes.Encrypt(model.Contrasenna);
            model.ConfirmarContrasenna =  _comunes.Encrypt(model.ConfirmarContrasenna);

            if (model.Contrasenna != model.ConfirmarContrasenna)
            {
                ViewBag.Mensaje = "La confirmación de su contraseña no coincide";
                return View();
            }

            model.Consecutivo = long.Parse(HttpContext.Session.GetString("Consecutivo")!.ToString());

            using (var client = _http.CreateClient())
            {
                var url = _conf.GetSection("Variables:UrlApi").Value + "Usuario/ActualizarContrasenna";
                                
                JsonContent datos = JsonContent.Create(model);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("TokenUsuario"));
                var response = client.PutAsync(url, datos).Result;
                var result = response.Content.ReadFromJsonAsync<Respuesta>().Result;

                if (result != null && result.Codigo == 0)
                {
                    return RedirectToAction("CerrarSesion", "Login");
                }
                else
                {
                    ViewBag.Mensaje = result!.Mensaje;
                    return View();
                }
            }
        }


        [HttpGet]
        public IActionResult ActualizarPerfil()
        {
            using (var client = _http.CreateClient())
            {
                var Consecutivo = long.Parse(HttpContext.Session.GetString("Consecutivo")!.ToString());
                string url = _conf.GetSection("Variables:UrlApi").Value + "Usuario/ConsultarUsuario?Consecutivo=" + Consecutivo;

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("TokenUsuario"));
                var response = client.GetAsync(url).Result;
                var result = response.Content.ReadFromJsonAsync<Respuesta>().Result;

                if (result != null && result.Codigo == 0)
                {
                    var datosContenido = JsonSerializer.Deserialize<Usuario>((JsonElement)result.Contenido!);
                    return View(datosContenido);
                }

                return View(new Usuario());
            }
        }

        [HttpPost]
        public IActionResult ActualizarPerfil(Usuario model)
        {
            model.Consecutivo = long.Parse(HttpContext.Session.GetString("Consecutivo")!.ToString());

            using (var client = _http.CreateClient())
            {
                var url = _conf.GetSection("Variables:UrlApi").Value + "Usuario/ActualizarPerfil";

                JsonContent datos = JsonContent.Create(model);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("TokenUsuario"));
                var response = client.PutAsync(url, datos).Result;
                var result = response.Content.ReadFromJsonAsync<Respuesta>().Result;
                ViewBag.Mensaje = result!.Mensaje;

                if (result != null && result.Codigo == 0)
                {
                    HttpContext.Session.SetString("NombreUsuario", model.Nombre);
                    return View();
                }
                else
                {
                    return View();
                }
            }
        }


        [HttpGet]
        public IActionResult ConsultarUsuarios()
        {
            var consecutivo = long.Parse(HttpContext.Session.GetString("Consecutivo")!.ToString());

            using (var client = _http.CreateClient())
            {
                string url = _conf.GetSection("Variables:UrlApi").Value + "Usuario/ConsultarUsuarios";

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session.GetString("TokenUsuario"));
                var response = client.GetAsync(url).Result;
                var result = response.Content.ReadFromJsonAsync<Respuesta>().Result;

                if (result != null && result.Codigo == 0)
                {
                    var datosContenido = JsonSerializer.Deserialize<List<Usuario>>((JsonElement)result.Contenido!);
                    return View(datosContenido!.Where(x => x.Consecutivo != consecutivo).ToList());
                }

                return View(new List<Usuario>());
            }
        }

    }
}
