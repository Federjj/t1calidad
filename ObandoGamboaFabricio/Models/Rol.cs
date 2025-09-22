// Importa el espacio de nombres de los ViewModels.
using ObandoGamboaFabricio.ViewModels;
// Importa las anotaciones de datos para validar propiedades de la clase.
using System.ComponentModel.DataAnnotations;

// Define el espacio de nombres del proyecto.
namespace ObandoGamboaFabricio.Models
{
    // Define la clase Rol que representa un modelo de datos.
    public class Rol
    {
        // Define la propiedad IdRol como clave primaria de la tabla.
        [Key]
        public int IdRol { get; set; }

        // Define la propiedad Nombre como obligatoria y con un máximo de 50 caracteres.
        [Required]
        [StringLength(50)]
        public string Nombre { get; set; }

        // Define una colección de Usuarios relacionada con el rol, indicando una relación 1:N.
        public ICollection<Usuarios> Usuarios { get; set; }
    }
}
