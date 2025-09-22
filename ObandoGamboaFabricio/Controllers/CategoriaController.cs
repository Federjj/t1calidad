// Importa las bibliotecas necesarias para manejar datos y vistas.
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ObandoGamboaFabricio.Data;
using ObandoGamboaFabricio.Models;
using System.Linq;
using ObandoGamboaFabricio.ViewModels;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

// Define el espacio de nombres del proyecto.
namespace ObandoGamboaFabricio.Controllers
{
    // Define el controlador CategoriaController para manejar la lógica relacionada con las categorías.
    public class CategoriaController : Controller
    {
        // Define el contexto de base de datos para interactuar con la base de datos.
        private readonly appDbContext _context;

        // Constructor que inicializa el contexto de base de datos.
        public CategoriaController(appDbContext context)
        {
            _context = context;
        }

        // Acción para mostrar la lista de categorías.
        [Authorize]
        public async Task<IActionResult> Index()
        {
            // Obtiene la lista de categorías desde la base de datos.
            var categorias = await _context.Categorias.ToListAsync();
            return View(categorias);
        }

        // Acción para mostrar la vista de creación de categorías.
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // Acción para manejar la creación de categorías.
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(CategoriaVM categoriaVM)
        {
            // Verifica si el modelo es válido.
            if (ModelState.IsValid)
            {
                // Crea una nueva categoría con los datos proporcionados.
                Categoria categoria = new Categoria
                {
                    Nombre = categoriaVM.Nombre
                };

                // Agrega la categoría a la base de datos.
                _context.Add(categoria);
                await _context.SaveChangesAsync();
                // Redirige a la vista de creación de artículos.
                return RedirectToAction("Create", "Articulo");
            }
            return View(categoriaVM);
        }
    }
}
