using Microsoft.AspNetCore.Mvc;

namespace SWeb.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult MostrarError()
        {
            return View("Error");
        }
    }
}
