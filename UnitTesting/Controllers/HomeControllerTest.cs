using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using ObandoGamboaFabricio.Controllers;
using ObandoGamboaFabricio.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace UnitTesting.Controllers
{
    public class HomeControllerTest
    {
        private readonly HomeController _controller;

        public HomeControllerTest()
        {
            // Simular ILogger
            var mockLogger = new Mock<ILogger<HomeController>>();

            // Configurar DbContext en memoria
            var options = new DbContextOptionsBuilder<appDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDB")
                .Options;

            var context = new appDbContext(options);

            // Instanciar el controlador con logger y context
            _controller = new HomeController(mockLogger.Object, context);
        }

        [Fact]
        public void Index_ReturnsAViewResult()
        {
            // Act
            var result = _controller.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
        }
    }
}
