// Define el espacio de nombres del proyecto.
namespace ObandoGamboaFabricio.Models
{
    // Define la clase ErrorViewModel que representa un modelo para manejar errores.
    public class ErrorViewModel
    {
        // Define la propiedad RequestId que almacena el identificador de la solicitud.
        public string? RequestId { get; set; }

        // Define una propiedad calculada que indica si el RequestId no está vacío o nulo.
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
