using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SApi.Models;

namespace SApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _conf;
        public LoginController(IConfiguration conf)
        {
            _conf = conf;
        }

        [HttpPost]
        [Route("CrearCuenta")]
        public IActionResult CrearCuenta(Usuario model)
        {
            using (var context = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                var result = context.Execute("CrearCuenta", new { model.Identificacion, model.Nombre, model.CorreoElectronico, model.Contrasenna });

                if (result > 0)
                { 
                
                }
            }

            return Ok(model);
        }

    }
}
