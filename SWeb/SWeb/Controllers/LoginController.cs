using Microsoft.AspNetCore.Mvc;
using SWeb.Models;

namespace SWeb.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult CrearCuenta()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CrearCuenta(Usuario model)
        {
            return View();
        }



        public IActionResult InicioSesion()
        {
            return View();
        }

        public IActionResult RecuperarAcceso()
        {
            return View();
        }        

    }
}
