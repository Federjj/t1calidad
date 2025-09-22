// Importa las bibliotecas necesarias para manejar datos.
using Microsoft.VisualBasic;

// Define el espacio de nombres del proyecto.
namespace ObandoGamboaFabricio.ViewModels
{
    // Define la clase UsuarioVM que representa un modelo de vista para el registro de usuarios.
    public class UsuarioVM
    {
        // Define la propiedad IdUsuario que almacena el identificador del usuario.
        public int IdUsuario { get; set; }
        // Define la propiedad NombreVM que almacena el nombre del usuario.
        public string NombreVM { get; set; }
        // Define la propiedad ApellidoVM que almacena el apellido del usuario.
        public string ApellidoVM { get; set; }
        // Define la propiedad CorreoVM que almacena el correo electrónico del usuario.
        public string CorreoVM { get; set; }
        // Define la propiedad PasswordVM que almacena la contraseña del usuario.
        public string PasswordVM { get; set; }
        // Define la propiedad RepPasswordVM que almacena la confirmación de la contraseña.
        public string RepPasswordVM { get; set; }
        // Define la propiedad RolID_VM que almacena el identificador del rol asignado al usuario.
        public int RolID_VM { get; set; }
    }
}
