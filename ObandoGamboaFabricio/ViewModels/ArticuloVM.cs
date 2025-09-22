// Importa las anotaciones de datos para validar propiedades de la clase.
using System.ComponentModel.DataAnnotations;

// Define el espacio de nombres del proyecto.
namespace ObandoGamboaFabricio.ViewModels
{
    // Define la clase ArticuloVM que representa un modelo de vista para los artículos.
    public class ArticuloVM
    {
        // Define la propiedad IdArticulo que almacena el identificador del artículo.
        public int IdArticulo { get; set; }

        // Define la propiedad Nombre como obligatoria y con un máximo de 100 caracteres.
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        // Define la propiedad Precio como obligatoria.
        [Required]
        public decimal Precio { get; set; }

        // Define la propiedad CategoriaId como obligatoria, que almacena el identificador de la categoría.
        [Required]
        public int CategoriaId { get; set; }

        // Define la propiedad ProveedorId como obligatoria, que almacena el identificador del proveedor.
        [Required]
        public int ProveedorId { get; set; }
    }
}
