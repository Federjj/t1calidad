using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Moq;
using ObandoGamboaFabricio.Controllers;
using ObandoGamboaFabricio.Data;
using ObandoGamboaFabricio.Models;
using ObandoGamboaFabricio.ViewModels;
using System.Threading.Tasks;
using Xunit;

namespace UnitTesting.Controllers
{
    public class AccesoControllerTest
    {
        private appDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<appDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // 🔹 BD única por test
                .Options;

            var context = new appDbContext(options);
            context.Database.EnsureCreated();

            return context;
        }

        [Fact]
        public async Task Registro_Post_CorreoYaExiste_RetornaVistaConError()
        {
            // Arrange
            var context = GetInMemoryDbContext();

            // Insertamos un usuario existente con un correo
            context.Usuarios.Add(new Usuarios
            {
                Nombre = "Juan",
                Apellido = "Perez",
                Correo = "juan@test.com",
                RolID = 1,
                Password = "123"
            });
            await context.SaveChangesAsync();

            var controller = new AccesoController(context);

            // ⚡ Configuramos TempData manualmente
            controller.TempData = new TempDataDictionary(new DefaultHttpContext(), Mock.Of<ITempDataProvider>());

            var nuevoUsuarioVM = new UsuarioVM
            {
                NombreVM = "Pedro",
                ApellidoVM = "Gomez",
                CorreoVM = "juan@test.com", // ⚡ mismo correo que ya existe
                PasswordVM = "1234",
                RepPasswordVM = "1234"
            };

            // Act
            var result = await controller.Registro(nuevoUsuarioVM);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);

            // Validamos que el modelo devuelto contiene el correo duplicado
            var model = Assert.IsType<UsuarioVM>(viewResult.Model);
            Assert.Equal(nuevoUsuarioVM.CorreoVM, model.CorreoVM);

            // Validamos que TempData contiene el mensaje de error esperado
            Assert.True(controller.TempData.ContainsKey("ErrorMessage"));
            Assert.Equal("El usuaio con este correo ya existe.", controller.TempData["ErrorMessage"]);
        }
    }
}
