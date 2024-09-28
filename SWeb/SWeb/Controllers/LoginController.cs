using Microsoft.AspNetCore.Mvc;

namespace SWeb.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult InicioSesion()
        {
            return View();
        }

        public IActionResult CrearCuenta()
        {
            return View();
        }

        public IActionResult RecuperarAcceso()
        {
            return View();
        }        

    }
}
