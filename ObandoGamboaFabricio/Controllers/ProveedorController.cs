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
    // Define el controlador ProveedorController para manejar la lógica relacionada con los proveedores.
    public class ProveedorController : Controller
    {
        // Define el contexto de base de datos para interactuar con la base de datos.
        private readonly appDbContext _context;

        // Constructor que inicializa el contexto de base de datos.
        public ProveedorController(appDbContext context)
        {
            _context = context;
        }

        // Acción para mostrar la lista de proveedores.


        // Acción para mostrar la vista de creación de proveedores.
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // Acción para manejar la creación de proveedores.
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(ProveedorVM proveedorVM)
        {
            // Verifica si el modelo es válido.
            if (ModelState.IsValid)
            {
                // Crea un nuevo proveedor con los datos proporcionados.
                Proveedor proveedor = new Proveedor
                {
                    Nombre = proveedorVM.Nombre,
                    Direccion = proveedorVM.Direccion
                };

                // Agrega el proveedor a la base de datos.
                _context.Add(proveedor);
                await _context.SaveChangesAsync();
                // Redirige a la vista de creación de artículos.
                return RedirectToAction("Create", "Articulo");
            }
            return View(proveedorVM);
        }
    }
}
