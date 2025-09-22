using ObandoGamboaFabricio.Models;

namespace ObandoGamboaFabricio.ViewModels
{
    public class PedidoVM
    {
        public Pedido NuevoPedido { get; set; }
        public List<DetallePedido> Detalles { get; set; }
        public List<Cliente> Clientes { get; set; }
        public List<Articulo> Articulos { get; set; }
        public List<Pedido> Pedidos { get; set; }
    }
}
