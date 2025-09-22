// Importa las anotaciones de datos para validar propiedades de la clase.
using System.ComponentModel.DataAnnotations;
// Importa las colecciones genéricas para manejar listas y conjuntos.
using System.Collections.Generic;
// Importa las anotaciones de datos para definir relaciones entre tablas.
using System.ComponentModel.DataAnnotations.Schema;

// Define el espacio de nombres del proyecto.
namespace ObandoGamboaFabricio.Models
{
    // Define la clase Articulo que representa un modelo de datos.
    public class Articulo
    {
        // Define la propiedad IdArticulo como clave primaria de la tabla.
        [Key]
        public int IdArticulo { get; set; }

        // Define la propiedad Nombre como obligatoria y con un máximo de 100 caracteres.
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        // Define la propiedad Descripción como obligatoria.
        [Required]
        [StringLength(500)]
        public string Descripcion { get; set; }

        // Define la propiedad Precio como obligatoria.
        [Required]
        public decimal Precio { get; set; }

        // Define la propiedad Stock como obligatoria para control de inventario.
        [Required]
        public int Stock { get; set; }

        // Define la propiedad ImagenUrl para almacenar la ruta de la imagen del producto.
        [StringLength(300)]
        public string ImagenUrl { get; set; }

        // Define la relación con la tabla Categoria mediante una clave foránea.
        [ForeignKey("Categoria")]
        public int CategoriaId { get; set; }
        // Define la propiedad de navegación para acceder a la entidad relacionada Categoria.
        public Categoria Categoria { get; set; }

        // Define una colección de DetallePedido relacionada con el artículo.
        public ICollection<DetallePedido> DetallesPedido { get; set; }
    }
}
