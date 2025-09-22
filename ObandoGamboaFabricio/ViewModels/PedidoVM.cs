// Importa las anotaciones de datos para validar propiedades de la clase.
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using ObandoGamboaFabricio.Models;

// Define el espacio de nombres del proyecto.
namespace ObandoGamboaFabricio.ViewModels
{
    // Define la clase PedidoVM que representa un modelo de vista para los pedidos.
    public class PedidoVM
    {
        // Define la propiedad IdPedido que almacena el identificador del pedido.
        public int IdPedido { get; set; }

        // Define la propiedad Estado para trackear el estado del pedido.
        [Required]
        [StringLength(50)]
        public string Estado { get; set; } = "Pendiente";

        // Define la propiedad Observaciones para notas del cliente.
        [StringLength(500)]
        public string Observaciones { get; set; }

        // Define la propiedad FechaPedido para orden cronológico.
        [Required]
        public System.DateTime FechaPedido { get; set; } = System.DateTime.Now;

        // Define la relación con la tabla Cliente mediante una clave foránea.
        [Required]
        public int ClienteId { get; set; }

        // Propiedades del cliente para mostrar en la vista
        public string ClienteNombre { get; set; }
        public string ClienteDireccion { get; set; }

        // Lista de detalles del pedido
        public List<DetallePedidoVM> Detalles { get; set; } = new List<DetallePedidoVM>();

        // Propiedad calculada para el total del pedido
        public decimal Total
        {
            get
            {
                decimal total = 0;
                foreach (var detalle in Detalles)
                {
                    total += detalle.Subtotal;
                }
                return total;
            }
        }
    }

    // Define la clase DetallePedidoVM para los detalles del pedido
    public class DetallePedidoVM
    {
        public int IdDetalle { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        public int IdArticulo { get; set; }

        public int PedidoId { get; set; }

        // Propiedades del artículo para mostrar en la vista
        public string ArticuloNombre { get; set; }
        public decimal ArticuloPrecio { get; set; }
        public string ArticuloDescripcion { get; set; }

        // Propiedad calculada para el subtotal
        public decimal Subtotal
        {
            get { return Cantidad * ArticuloPrecio; }
        }
    }
}