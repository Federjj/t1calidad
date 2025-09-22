// Importa las anotaciones de datos para validar propiedades de la clase.
using System.ComponentModel.DataAnnotations;
// Importa las colecciones genéricas para manejar listas y conjuntos.
using System.Collections.Generic;

// Define el espacio de nombres del proyecto.
namespace ObandoGamboaFabricio.Models
{
    // Define la clase Cliente que representa un modelo de datos.
    public class Cliente
    {
        // Define la propiedad IdCliente como clave primaria de la tabla.
        [Key]
        public int IdCliente { get; set; }

        // Define la propiedad Nombre como obligatoria y con un máximo de 100 caracteres.
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        // Define la propiedad Direccion con un máximo de 200 caracteres.
        [StringLength(200)]
        public string Direccion { get; set; }

        // Define una colección de Pedido relacionada con el cliente.
        public ICollection<Pedido> Pedidos { get; set; }
    }
}
