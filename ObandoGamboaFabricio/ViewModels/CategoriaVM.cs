// Importa las anotaciones de datos para validar propiedades de la clase.
using System.ComponentModel.DataAnnotations;

// Define el espacio de nombres del proyecto.
namespace ObandoGamboaFabricio.ViewModels
{
    // Define la clase CategoriaVM que representa un modelo de vista para las categorías.
    public class CategoriaVM
    {
        // Define la propiedad Nombre como obligatoria y con un máximo de 100 caracteres.
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }
    }
}
