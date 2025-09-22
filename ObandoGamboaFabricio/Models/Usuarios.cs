// Importa las anotaciones de datos para definir relaciones entre tablas y validar propiedades.
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

// Define el espacio de nombres del proyecto.
namespace ObandoGamboaFabricio.Models
{
    // Define la clase Usuarios que representa un modelo de datos para la tabla Usuarios.
    public class Usuarios
    {
        // Define la propiedad IdUsuario como clave primaria de la tabla.
        [Key]
        public int IdUsuario { get; set; }

        // Define la propiedad Nombre como obligatoria y con un máximo de 100 caracteres.
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        // Define la propiedad Apellido como obligatoria y con un máximo de 100 caracteres.
        [Required]
        [StringLength(100)]
        public string Apellido { get; set; }

        // Define la propiedad Correo como obligatoria y con un máximo de 100 caracteres.
        [Required]
        [StringLength(100)]
        public string Correo { get; set; }

        // Define la propiedad Password como obligatoria y con un máximo de 255 caracteres.
        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        // Define la relación con la tabla Rol mediante una clave foránea.
        [ForeignKey("Rol")]
        public int RolID { get; set; } // Clave foránea que referencia a Rol.
        // Define la propiedad de navegación para acceder a la entidad relacionada Rol.
        public Rol Rol { get; set; } // Relación 1:1 (un usuario tiene un solo rol, como "Admin" o "Usuario").
    }
}
