// Importa las anotaciones de datos para validar propiedades de la clase.
using System.ComponentModel.DataAnnotations;
// Importa las colecciones genéricas para manejar listas y conjuntos.
using System.Collections.Generic;
// Importa System para usar DateTime
using System;

// Define el espacio de nombres del proyecto.
namespace ObandoGamboaFabricio.Models
{
    // Define la clase Pedido que representa un modelo de datos.
    public class Pedido
    {
        // Define la propiedad IdPedido como clave primaria de la tabla.
        [Key]
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
        public DateTime FechaPedido { get; set; } = DateTime.Now;

        // Define la relación con la tabla Cliente mediante una clave foránea.
        [Required]
        public int ClienteId { get; set; }
        // Define la propiedad de navegación para acceder a la entidad relacionada Cliente.
        public Cliente Cliente { get; set; }

        // Define una colección de DetallePedido relacionada con el pedido.
        public ICollection<DetallePedido> DetallesPedido { get; set; }
    }
}
