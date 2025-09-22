// Importa las bibliotecas necesarias para manejar datos, seguridad y vistas.
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ObandoGamboaFabricio.Data;
using ObandoGamboaFabricio.Models;
using System.Linq;
using ObandoGamboaFabricio.ViewModels;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;


// Define el espacio de nombres del proyecto.
namespace ObandoGamboaFabricio.Controllers
{
    // Define el controlador ArticuloController para manejar la lógica relacionada con los artículos.
    public class ArticuloController : Controller
    {
        // Define el contexto de base de datos para interactuar con la base de datos.
        private readonly appDbContext _context;

        // Constructor que inicializa el contexto de base de datos.
        public ArticuloController(appDbContext context)
        {
            _context = context;
        }

        // Acción para mostrar la lista de artículos.
        [Authorize]
        public async Task<IActionResult> Index()
        {
            // Obtiene la lista de artículos incluyendo sus categorías y proveedores.
            var articulos = await _context.Articulos
                .Include(a => a.Categoria)

                .ToListAsync();
            return View(articulos);
        }

        // Acción para redirigir a la lista de usuarios.
        public async Task<IActionResult> ListUsuarios()
        {
            return RedirectToAction("Index", "Acceso");
        }

        // Acción para mostrar la vista de creación de artículos.
        [Authorize]
        public IActionResult Create()
        {
            // Obtiene las listas de categorías y proveedores para mostrarlas en la vista.
            ViewBag.Categorias = _context.Categorias.ToList();

            return View();
        }

        // Acción para manejar la creación de artículos.
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(ViewModels.ArticuloVM articuloVM)
        {
            // Verifica si el modelo es válido.
            if (ModelState.IsValid)
            {
                // Crea un nuevo artículo con los datos proporcionados.
                Models.Articulo articulo = new Models.Articulo
                {
                    Nombre = articuloVM.Nombre,
                    Descripcion = articuloVM.Descripcion,
                    Precio = articuloVM.Precio,
                    Stock = articuloVM.Stock,
                    ImagenUrl = articuloVM.ImagenUrl,
                    CategoriaId = articuloVM.CategoriaId
                };

                // Agrega el artículo a la base de datos.
                _context.Add(articulo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Si el modelo no es válido, recarga las listas de categorías y proveedores.
            ViewBag.Categorias = _context.Categorias.ToList();

            return RedirectToAction("Index", "Articulo");
        }

        // Acción para mostrar la vista de edición de artículos.
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {

            // Busca el artículo por su ID.
            var articulo = await _context.Articulos.FindAsync(id);
            if (articulo == null)
            {
                return NotFound();
            }

            // Crea un modelo de vista para la edición del artículo.
            ViewModels.ArticuloVM articuloVM = new ViewModels.ArticuloVM()
            {
                IdArticulo = articulo.IdArticulo,
                Nombre = articulo.Nombre,
                Descripcion = articulo.Descripcion,
                Precio = articulo.Precio,
                Stock = articulo.Stock,
                ImagenUrl = articulo.ImagenUrl,
                CategoriaId = articulo.CategoriaId
            };

            // Obtiene las listas de categorías y proveedores para mostrarlas en la vista.
            ViewBag.Categorias = _context.Categorias.ToList();


            return View(articuloVM);
        }

        // Acción para confirmar la edición de artículos.
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditConfirmed(ArticuloVM articuloVM)
        {
            // Verifica si el modelo es válido.
            if (ModelState.IsValid)
            {
                // Busca el artículo por su ID.
                Articulo articulo = await _context.Articulos.FindAsync(articuloVM.IdArticulo);
                if (articulo == null)
                {
                    return NotFound();
                }

                // Actualiza los datos del artículo.
                articulo.Nombre = articuloVM.Nombre;
                articulo.Descripcion = articuloVM.Descripcion;
                articulo.Precio = articuloVM.Precio;
                articulo.Stock = articuloVM.Stock;
                articulo.ImagenUrl = articuloVM.ImagenUrl;
                articulo.CategoriaId = articuloVM.CategoriaId;

                // Guarda los cambios en la base de datos.
                _context.Articulos.Update(articulo);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            // Si el modelo no es válido, recarga las listas de categorías y proveedores.
            ViewBag.Categorias = _context.Categorias.ToList();

            return View(articuloVM);
        }

        // Acción para manejar la edición de artículos.
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(int id, Models.Articulo articulo)
        {
            // Verifica si el ID proporcionado coincide con el ID del artículo.
            if (id != articulo.IdArticulo)
            {
                return NotFound();
            }

            // Verifica si el modelo es válido.
            if (ModelState.IsValid)
            {
                try
                {
                    // Actualiza el artículo en la base de datos.
                    _context.Update(articulo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Verifica si el artículo existe en la base de datos.
                    if (!ArticuloExiste(articulo.IdArticulo))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // Si el modelo no es válido, recarga las listas de categorías y proveedores.
            ViewBag.Categorias = _context.Categorias.ToList();

            return RedirectToAction("Index", "Articulo");
        }

        // Acción para mostrar la vista de eliminación de artículos.
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            var rolUsuario = User.FindFirstValue(ClaimTypes.Role);
            if (rolUsuario != "Admin")
            {
                TempData["ErrorMessage"] = "No tiene permiso para eliminar articulos. Solo los administradores pueden realizar esta acción.";
            }
            else
            {
                if (id == null)
                {
                    return NotFound();
                }

                // Busca el artículo por su ID incluyendo su categoría y proveedor.
                var articulo = await _context.Articulos
                    .Include(a => a.Categoria)

                    .FirstOrDefaultAsync(m => m.IdArticulo == id);
                if (articulo == null)
                {
                    return NotFound();
                }

                return View(articulo);
            }
            return RedirectToAction("Index", "Articulo");
        }

        // Acción para confirmar la eliminación de artículos.
        [HttpPost, ActionName("Delete")]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Busca el artículo por su ID.
            var articulo = await _context.Articulos.FindAsync(id);
            // Elimina el artículo de la base de datos.
            _context.Articulos.Remove(articulo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Método para verificar si un artículo existe en la base de datos.
        private bool ArticuloExiste(int id)
        {
            return _context.Articulos.Any(e => e.IdArticulo == id);
        }
    }
}
