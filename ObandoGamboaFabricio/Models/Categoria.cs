// Importa las anotaciones de datos para validar propiedades de la clase.
using System.ComponentModel.DataAnnotations;
// Importa las colecciones genéricas para manejar listas y conjuntos.
using System.Collections.Generic;

// Define el espacio de nombres del proyecto.
namespace ObandoGamboaFabricio.Models
{
    // Define la clase Categoria que representa un modelo de datos.
    public class Categoria
    {
        // Define la propiedad IdCategoria como clave primaria de la tabla.
        [Key]
        public int IdCategoria { get; set; }

        // Define la propiedad Nombre como obligatoria y con un máximo de 100 caracteres.
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        // Define una colección de Articulo relacionada con la categoría.
        public ICollection<Articulo> Articulos { get; set; }
    }
}
