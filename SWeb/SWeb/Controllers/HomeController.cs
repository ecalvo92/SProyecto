using Microsoft.AspNetCore.Mvc;

namespace SWeb.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Inicio()
        {
            return View();
        }
    }
}
