// Importa las bibliotecas necesarias para manejar el contexto de base de datos y los modelos.
using Microsoft.EntityFrameworkCore;
using ObandoGamboaFabricio.Models;
using Microsoft.EntityFrameworkCore.SqlServer;
using ObandoGamboaFabricio.ViewModels;
using System.Threading;
using System;

// Define el espacio de nombres del proyecto.
namespace ObandoGamboaFabricio.Data
{
    // Define la clase appDbContext que hereda de DbContext para manejar el contexto de base de datos.
    public class appDbContext : DbContext
    {
        // Constructor que inicializa el contexto con las opciones proporcionadas.
        public appDbContext(DbContextOptions<appDbContext> options) : base(options)
        {
        }

        // Define las tablas de la base de datos como conjuntos de datos.
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Articulo> Articulos { get; set; }
        public DbSet<DetallePedido> DetallesPedido { get; set; }
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Cliente> Clientes { get; set; }

        // Método para configurar las relaciones entre las entidades.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Llama al método base para la configuración inicial.
            base.OnModelCreating(modelBuilder);

            // Configura datos iniciales para la tabla Rol.
            modelBuilder.Entity<Rol>().HasData(
                new Rol { IdRol = 1, Nombre = "Admin" },
                new Rol { IdRol = 2, Nombre = "Usuario" }
            );

            // Configura las relaciones para el modelo entidad-relación (ER).
            modelBuilder.Entity<Articulo>(e =>
            {
                // Configura la relación entre Articulo y Categoria.
                e.HasOne(e => e.Categoria).WithMany(r => r.Articulos)
                .HasForeignKey(e => e.CategoriaId);
            });

            modelBuilder.Entity<Pedido>(e =>
            {
                // Configura la relación entre Pedido y Cliente.
                e.HasOne(e => e.Cliente).WithMany(r => r.Pedidos).HasForeignKey(e => e.ClienteId);
            });

            modelBuilder.Entity<DetallePedido>(e =>
            {
                // Configura la relación entre DetallePedido y Pedido.
                e.HasOne(e => e.Pedido).WithMany(r => r.DetallesPedido).HasForeignKey(e => e.PedidoId);
                // Configura la relación entre DetallePedido y Articulo.
                e.HasOne(e => e.articulo).WithMany(r => r.DetallesPedido).HasForeignKey(e => e.IdArticulo);
            });
        }
    }
}
