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

        // Acción para mostrar el formulario de creación de pedidos (para administradores)
        [Authorize]
        public async Task<IActionResult> Create()
        {
            ViewBag.Clientes = await _context.Clientes.ToListAsync();
            ViewBag.Articulos = await _context.Articulos
                .Where(a => a.Stock > 0)
                .Include(a => a.Categoria)
                .ToListAsync();

            return View();
        }

        // Acción para manejar la creación de pedidos (para administradores)
        [HttpPost]
        [Authorize]
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

        // Acción para crear pedido desde el menú público (sin autenticación)
        [HttpPost]
        public async Task<IActionResult> CrearDesdeMenu(string clienteNombre, string clienteDireccion, string observaciones, string productosJson)
        {
            try
            {
                // Crear o buscar cliente
                var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Nombre == clienteNombre);
                if (cliente == null)
                {
                    cliente = new Cliente
                    {
                        Nombre = clienteNombre,
                        Direccion = clienteDireccion
                       
                    };
                    _context.Clientes.Add(cliente);
                    await _context.SaveChangesAsync();
                }

                // Crear el pedido
                var pedido = new Pedido
                {
                    Estado = "Pendiente",
                    Observaciones = observaciones,
                    FechaPedido = DateTime.Now,
                    ClienteId = cliente.IdCliente
                };

                _context.Pedidos.Add(pedido);
                await _context.SaveChangesAsync();

                // Procesar productos del carrito (simulado por ahora)
                // En una implementación real, procesaríamos el JSON de productos
                // Por ahora, creamos un detalle de ejemplo
                var articuloDisponible = await _context.Articulos
                    .Where(a => a.Stock > 0)
                    .FirstOrDefaultAsync();

                if (articuloDisponible != null)
                {
                    var detalle = new DetallePedido
                    {
                        Cantidad = 1,
                        IdArticulo = articuloDisponible.IdArticulo,
                        PedidoId = pedido.IdPedido
                    };

                    _context.DetallesPedido.Add(detalle);

                    // Actualizar stock
                    articuloDisponible.Stock -= 1;
                    _context.Articulos.Update(articuloDisponible);
                }

                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Pedido realizado con éxito. Pronto nos comunicaremos contigo.", pedidoId = pedido.IdPedido });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al realizar el pedido: " + ex.Message });
            }
        }

        // Acción para cambiar el estado de un pedido (solo administradores)
        [HttpPost]
        [Authorize]
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

        // Acción para mostrar los detalles de un pedido (solo administradores)
        [Authorize]
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

        // Acción para buscar pedidos por estado (solo administradores)
        [Authorize]
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
