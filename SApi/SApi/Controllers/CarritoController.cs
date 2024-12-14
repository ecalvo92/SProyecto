using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using SApi.Models;

namespace SApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CarritoController : ControllerBase
    {
        private readonly IConfiguration _conf;
        public CarritoController(IConfiguration conf)
        {
            _conf = conf;
        }

        [HttpPost]
        [Route("RegistrarCarrito")]
        public IActionResult RegistrarCarrito(Carrito model)
        {
            using (var context = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                var respuesta = new Respuesta();
                var result = context.Execute("RegistrarCarrito", new { model.ConsecutivoUsuario, model.ConsecutivoProducto, model.Unidades });

                if (result > 0)
                {
                    respuesta.Codigo = 0;
                }
                else
                {
                    respuesta.Codigo = -1;
                    respuesta.Mensaje = "El producto no se ha actualizado correctamente en su carrito";
                }

                return Ok(respuesta);
            }
        }

        [HttpGet]
        [Route("ConsultarCarrito")]
        public IActionResult ConsultarCarrito(long Consecutivo)
        {
            using (var context = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                var respuesta = new Respuesta();
                var result = context.Query<Carrito>("ConsultarCarrito", new { Consecutivo });

                if (result.Any())
                {
                    respuesta.Codigo = 0;
                    respuesta.Contenido = result;
                }
                else
                {
                    respuesta.Codigo = -1;
                    respuesta.Mensaje = "No hay productos en su carrito";
                }

                return Ok(respuesta);
            }
        }

        [HttpPost]
        [Route("RemoverProductoCarrito")]
        public IActionResult RemoverProductoCarrito(Carrito model)
        {
            using (var context = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                var respuesta = new Respuesta();
                var result = context.Execute("RemoverProductoCarrito", new { model.ConsecutivoUsuario, model.ConsecutivoProducto });

                if (result > 0)
                {
                    respuesta.Codigo = 0;
                }
                else
                {
                    respuesta.Codigo = -1;
                    respuesta.Mensaje = "El producto no se ha removido dn su carrito";
                }

                return Ok(respuesta);
            }
        }

        [HttpPost]
        [Route("PagarCarrito")]
        public IActionResult PagarCarrito(Carrito model)
        {
            using (var context = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                var respuesta = new Respuesta();
                var result = context.Execute("PagarCarrito", new { model.ConsecutivoUsuario });

                if (result > 0)
                {
                    respuesta.Codigo = 0;
                }
                else
                {
                    respuesta.Codigo = -1;
                    respuesta.Mensaje = "No se realizó el pago de su carrito";
                }

                return Ok(respuesta);
            }
        }

        [HttpGet]
        [Route("ConsultarFacturas")]
        public IActionResult ConsultarFacturas(long Consecutivo)
        {
            using (var context = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                var respuesta = new Respuesta();
                var result = context.Query<Carrito>("ConsultarFacturas", new { Consecutivo });

                if (result.Any())
                {
                    respuesta.Codigo = 0;
                    respuesta.Contenido = result;
                }
                else
                {
                    respuesta.Codigo = -1;
                    respuesta.Mensaje = "No hay facturas registradas en este momento";
                }

                return Ok(respuesta);
            }
        }

        [HttpGet]
        [Route("ConsultarDetallesFactura")]
        public IActionResult ConsultarDetallesFactura(long Consecutivo)
        {
            using (var context = new SqlConnection(_conf.GetSection("ConnectionStrings:DefaultConnection").Value))
            {
                var respuesta = new Respuesta();
                var result = context.Query<Carrito>("ConsultarDetallesFactura", new { Consecutivo });

                if (result.Any())
                {
                    respuesta.Codigo = 0;
                    respuesta.Contenido = result;
                }
                else
                {
                    respuesta.Codigo = -1;
                    respuesta.Mensaje = "No hay detalles para esa factura en este momento";
                }

                return Ok(respuesta);
            }
        }

    }
}
