using SWeb.Models;

namespace SWeb.Servicios
{
    public interface IMetodosComunes
    {
        string Encrypt(string texto);
        List<Carrito> ConsultarCarrito();
    }
}
