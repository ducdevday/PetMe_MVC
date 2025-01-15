using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PetMe.Web.Controllers;

namespace PetMe.Tests.UnitTests.WebTest.ControllerTest
{
    public class HomeControllerTest
    {
        private readonly Mock<ILogger<HomeController>> _mockLogger;
        private readonly HomeController _controller;

        public HomeControllerTest()
        {
            // Initialize the HomeController
            _mockLogger = new Mock<ILogger<HomeController>>();
            _controller = new HomeController(_mockLogger.Object);
        }

        [Fact]
        public void Index_ReturnsView()
        {
            // Act
            var result = _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);  // Verify it returns a ViewResult
        }

        [Fact]
        public void About_ReturnsView()
        {
            // Act
            var result = _controller.About();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);  // Verify it returns a ViewResult
        }
    }
}
