// Importa las anotaciones de datos para validar propiedades de la clase.
using System.ComponentModel.DataAnnotations;

// Define el espacio de nombres del proyecto.
namespace ObandoGamboaFabricio.ViewModels
{
    // Define la clase ProveedorVM que representa un modelo de vista para los proveedores.
    public class ProveedorVM
    {
        // Define la propiedad Nombre como obligatoria y con un máximo de 100 caracteres.
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        // Define la propiedad Direccion con un máximo de 200 caracteres.
        [StringLength(200)]
        public string Direccion { get; set; }
    }
}
