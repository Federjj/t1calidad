using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ObandoGamboaFabricio.Data;
using ObandoGamboaFabricio.Models;
using System.Threading.Tasks;
using System.Linq;

namespace ObandoGamboaFabricio.Controllers
{
    public class InventarioController : Controller
    {
        private readonly appDbContext _context;

        public InventarioController(appDbContext context)
        {
            _context = context;
        }

        // GET: Inventario
        public async Task<IActionResult> Index()
        {
            var inventario = await _context.Articulos
                .Include(a => a.Categoria)
                .Include(a => a.Proveedor)
                .ToListAsync();

            return View(inventario);
        }

        // GET: Inventario/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var articulo = await _context.Articulos.FindAsync(id);
            if (articulo == null)
            {
                return NotFound();
            }
            return View(articulo);
        }

        // POST: Inventario/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdArticulo,Nombre,Precio,CategoriaId,ProveedorId,Stock")] Articulo articulo)
        {
            if (id != articulo.IdArticulo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _context.Update(articulo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(articulo);
        }
    }
}
