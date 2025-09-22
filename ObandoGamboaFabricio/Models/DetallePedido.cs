// Importa las anotaciones de datos para validar propiedades de la clase.
using System.ComponentModel.DataAnnotations;
// Importa las anotaciones de datos para definir relaciones entre tablas.
using System.ComponentModel.DataAnnotations.Schema;

// Define el espacio de nombres del proyecto.
namespace ObandoGamboaFabricio.Models
{
    // Define la clase DetallePedido que representa un modelo de datos.
    public class DetallePedido
    {
        // Define la propiedad IdDetalle como clave primaria de la tabla.
        [Key]
        public int IdDetalle { get; set; }

        // Define la propiedad Cantidad como obligatoria.
        [Required]
        public int Cantidad { get; set; }

        // Define la relación con la tabla Articulo mediante una clave foránea.
        [ForeignKey("Articulo")]
        public int IdArticulo { get; set; }
        // Define la propiedad de navegación para acceder a la entidad relacionada Articulo.
        public Articulo articulo { get; set; }

        // Define la relación con la tabla Pedido mediante una clave foránea.
        [ForeignKey("Pedido")]
        public int PedidoId { get; set; }
        // Define la propiedad de navegación para acceder a la entidad relacionada Pedido.
        public Pedido Pedido { get; set; }
    }
}
