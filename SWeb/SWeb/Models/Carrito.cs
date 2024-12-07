namespace SWeb.Models
{
    public class Carrito
    {
        public long ConsecutivoUsuario { get; set; }
        public long ConsecutivoProducto { get; set; }
        public int Unidades { get; set; }
        public DateTime Fecha { get; set; }
    }
}
