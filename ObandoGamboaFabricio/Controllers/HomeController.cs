// Importa las bibliotecas necesarias para autorización, manejo de vistas y registro de eventos.
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ObandoGamboaFabricio.Models;
using System.Diagnostics;

// Define el espacio de nombres del proyecto.
namespace ObandoGamboaFabricio.Controllers
{
    // Define el controlador HomeController para manejar la lógica de las vistas principales.
    public class HomeController : Controller
    {
        // Define un objeto para registrar eventos y mensajes.
        private readonly ILogger<HomeController> _logger;

        // Constructor que inicializa el objeto de registro.
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Acción para mostrar la vista principal (Index).
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        // Acción para mostrar la vista de privacidad.
        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        // Acción para manejar errores y mostrar la vista de error.
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Crea un modelo de error con el identificador de la solicitud actual.
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Acción para redirigir a la vista de login.
        public IActionResult Salida()
        {
            return RedirectToAction("Login", "Acceso");
        }
    }
}
