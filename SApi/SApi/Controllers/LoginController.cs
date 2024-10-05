using Microsoft.AspNetCore.Mvc;
using SApi.Models;

namespace SApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        [Route("CrearCuenta")]
        public IActionResult CrearCuenta(Usuario model)
        {
            model.Nombre = "Maripas";
            model.Identificacion = "X-XXXX-0416";
            model.Contrasenna = string.Empty;

            return Ok(model);
        }

    }
}
