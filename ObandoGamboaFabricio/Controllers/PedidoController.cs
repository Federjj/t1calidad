using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ObandoGamboaFabricio.Data;
using ObandoGamboaFabricio.Models;
using ObandoGamboaFabricio.ViewModels;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ObandoGamboaFabricio.Controllers
{
    [Authorize]
    public class PedidoController : Controller
    {
        private readonly appDbContext _context;

        public PedidoController(appDbContext context)
        {
            _context = context;
        }

        // Acción para mostrar la lista de pedidos (panel de administración)
        public async Task<IActionResult> Index()
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.DetallesPedido)
                    .ThenInclude(d => d.articulo)
                .OrderByDescending(p => p.FechaPedido)
                .ToListAsync();

            return View(pedidos);
        }

        // Acción para mostrar el formulario de creación de pedidos
        public async Task<IActionResult> Create()
        {
            ViewBag.Clientes = await _context.Clientes.ToListAsync();
            ViewBag.Articulos = await _context.Articulos
                .Where(a => a.Stock > 0)
                .Include(a => a.Categoria)
                .ToListAsync();

            return View();
        }

        // Acción para manejar la creación de pedidos
        [HttpPost]
        public async Task<IActionResult> Create(PedidoVM pedidoVM)
        {
            if (ModelState.IsValid)
            {
                // Crear el pedido
                var pedido = new Pedido
                {
                    Estado = pedidoVM.Estado,
                    Observaciones = pedidoVM.Observaciones,
                    FechaPedido = pedidoVM.FechaPedido,
                    ClienteId = pedidoVM.ClienteId
                };

                _context.Pedidos.Add(pedido);
                await _context.SaveChangesAsync();

                // Crear los detalles del pedido y actualizar stock
                foreach (var detalleVM in pedidoVM.Detalles)
                {
                    var articulo = await _context.Articulos.FindAsync(detalleVM.IdArticulo);
                    if (articulo != null && articulo.Stock >= detalleVM.Cantidad)
                    {
                        var detalle = new DetallePedido
                        {
                            Cantidad = detalleVM.Cantidad,
                            IdArticulo = detalleVM.IdArticulo,
                            PedidoId = pedido.IdPedido
                        };

                        _context.DetallesPedido.Add(detalle);

                        // Actualizar stock
                        articulo.Stock -= detalleVM.Cantidad;
                        _context.Articulos.Update(articulo);
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Clientes = await _context.Clientes.ToListAsync();
            ViewBag.Articulos = await _context.Articulos
                .Where(a => a.Stock > 0)
                .Include(a => a.Categoria)
                .ToListAsync();

            return View(pedidoVM);
        }

        // Acción para cambiar el estado de un pedido
        [HttpPost]
        public async Task<IActionResult> CambiarEstado(int id, string nuevoEstado)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido != null)
            {
                pedido.Estado = nuevoEstado;
                _context.Pedidos.Update(pedido);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        // Acción para mostrar los detalles de un pedido
        public async Task<IActionResult> Details(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.DetallesPedido)
                    .ThenInclude(d => d.articulo)
                .FirstOrDefaultAsync(p => p.IdPedido == id);

            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        // Acción para buscar pedidos por estado
        public async Task<IActionResult> Buscar(string estado)
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.DetallesPedido)
                    .ThenInclude(d => d.articulo)
                .Where(p => string.IsNullOrEmpty(estado) || p.Estado == estado)
                .OrderByDescending(p => p.FechaPedido)
                .ToListAsync();

            ViewBag.EstadoFiltro = estado;
            return View("Index", pedidos);
        }
    }
}
