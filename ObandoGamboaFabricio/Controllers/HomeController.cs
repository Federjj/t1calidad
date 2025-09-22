// Importa las bibliotecas necesarias para autorización, manejo de vistas y registro de eventos.
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ObandoGamboaFabricio.Data;
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
        private readonly appDbContext _context;

        // Constructor que inicializa el objeto de registro y el contexto de la base de datos.
        public HomeController(ILogger<HomeController> logger, appDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // Acción para mostrar la vista principal (Index) con el menú de productos.
        public IActionResult Index()
        {
            // Cargar productos con sus categorías y filtrar solo los disponibles (stock > 0)
            var productos = _context.Articulos
                .Include(a => a.Categoria)
                .Where(a => a.Stock > 0)
                .OrderBy(a => a.Categoria.Nombre)
                .ThenBy(a => a.Nombre)
                .ToList();

            // Cargar todas las categorías para los filtros
            var categorias = _context.Categorias.ToList();
            ViewBag.Categorias = categorias;

            return View(productos);
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
