using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PetMe.Business.Services;
using PetMe.Data.Entities;
using PetMe.Data.Enums;
using PetMe.DataAccess.Repositories;
using PetMe.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PetMe.Tests.UnitTests.WebTest.ControllerTest
{
    public class AdoptControllerTest
    {
        private readonly Mock<IAdoptionService> _adoptionServiceMock;
        private readonly Mock<IPetService> _petServiceMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly Mock<IPetOwnerService> _petOwnerServiceMock;
        private readonly Mock<IAdoptionRequestRepository> _adoptionRequestRepositoryMock;
        private readonly Mock<IAdoptionRequestService> _adoptionRequestServiceMock;

        private readonly Mock<HttpContext> _httpContextMock;
        private readonly Mock<ISession> _sessionMock;
        private readonly AdoptionController _controller;

        public AdoptControllerTest()
        {
            _adoptionServiceMock = new Mock<IAdoptionService>();
            _petServiceMock = new Mock<IPetService>();
            _userServiceMock = new Mock<IUserService>();
            _emailServiceMock = new Mock<IEmailService>();
            _petOwnerServiceMock = new Mock<IPetOwnerService>();
            _adoptionRequestRepositoryMock = new Mock<IAdoptionRequestRepository>();
            _adoptionRequestServiceMock = new Mock<IAdoptionRequestService>();

            _sessionMock = new Mock<ISession>();
            _httpContextMock = new Mock<HttpContext>();
            _httpContextMock.Setup(x => x.Session).Returns(_sessionMock.Object);

            _controller = new AdoptionController(
                _adoptionServiceMock.Object,
                _adoptionRequestServiceMock.Object,
                _petServiceMock.Object,
                _userServiceMock.Object,
                _emailServiceMock.Object,
                _petOwnerServiceMock.Object
            );
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = _httpContextMock.Object
            };
        }

        [Fact]
        public async Task Adopt_ShouldRedirectToLogin_WhenUserIsNotLoggedIn()
        {
            // Arrange
            var sessionMock = new Mock<ISession>();

            sessionMock.Setup(s => s.TryGetValue("Username", out It.Ref<byte[]>.IsAny))
                .Returns(false);

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(x => x.Session).Returns(sessionMock.Object);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContextMock.Object
            };

            // Act
            var result = await _controller.Adopt(1);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Login", redirectResult.ActionName);
            Assert.Equal("Account", redirectResult.ControllerName);
        }


        [Fact]
        public async Task Adopt_ShouldReturnBadRequest_WhenPetDoesNotExist()
        {
            // Arrange
            var mockSession = new Mock<ISession>();

            mockSession.Setup(s => s.TryGetValue(It.IsAny<string>(), out It.Ref<byte[]>.IsAny))
                .Returns((string key, out byte[] value) =>
                {
                    value = key == "Username" ? Encoding.UTF8.GetBytes("user1") : null;
                    return true;
                });

            var context = new DefaultHttpContext
            {
                Session = mockSession.Object
            };

            _controller.ControllerContext.HttpContext = context;

            _petServiceMock.Setup(p => p.GetPetByIdAsync(It.IsAny<int>())).ReturnsAsync((Pet)null);  // Pet null döndürülüyor

            // Act
            var result = await _controller.Adopt(1);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);  // Pet bulunamazsa BadRequest dönecek
        }

        [Fact]
        public async Task Index_ShouldReturnViewWithPets()
        {
            // Arrange
            var pets = new List<Pet>() {
                    new Pet { Id = 1, Name = "Dog" },
                    new Pet { Id = 2, Name = "Cat" }
                };
            _petServiceMock.Setup(service => service.GetAllPetsAsync()).ReturnsAsync(pets);

            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<List<Pet>> (viewResult.Model);
            Assert.Equal(2, model.Count);
        }

        [Fact]
        public async Task ApproveRequest_ShouldReturnNotFound_WhenRequestDoesNotExist()
        {
            // Arrange
            _adoptionRequestServiceMock.Setup(service => service.GetAdoptionRequestByIdAsync(It.IsAny<int>())).ReturnsAsync((AdoptionRequest)null);

            // Act
            var result = await _controller.ApproveRequest(1, 1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task ApproveRequest_ShouldReturnUnauthorized_WhenUserIsNotOwner()
        {
            // Arrange
            var pet = new Pet { Id = 1, Name = "Dog", PetOwners = { new PetOwner { UserId = 2 } } };
            var adoptionRequest = new AdoptionRequest { PetId = pet.Id, Status = AdoptionStatus.Pending, UserId = 1 };
            _adoptionRequestServiceMock.Setup(service => service.GetAdoptionRequestByIdAsync(1)).ReturnsAsync(adoptionRequest);
            _petServiceMock.Setup(service => service.GetPetByIdAsync(1)).ReturnsAsync(pet);

            // Act
            var result = await _controller.ApproveRequest(1, 1);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task ApproveRequest_ShouldApproveRequestAndSendEmails()
        {
            // Arrange
            var user = new User
            {
                Id = 1,
                Username = "john_doe",
                Email = "john@example.com"
            };

            var pet = new Pet
            {
                Id = 1,
                Name = "Dog",
                PetOwners = new List<PetOwner>
            {
                new PetOwner
                {
                    UserId = 1,
                    User = user, 
                }
            } // PetOwner listesi başlatıldı
            };

            var adoptionRequest = new AdoptionRequest
            {
                PetId = pet.Id,
                Status = AdoptionStatus.Pending,
                UserId = 1
            };

            _adoptionRequestServiceMock.Setup(service => service.GetAdoptionRequestByIdAsync(1))
                .ReturnsAsync(adoptionRequest); 
            _petServiceMock.Setup(service => service.GetPetByIdAsync(1))
                .ReturnsAsync(pet);

            var claims = new[]
            {
            new Claim(ClaimTypes.Name, "1")
        };
            var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, "mock"));
            _controller.ControllerContext.HttpContext = new DefaultHttpContext { User = userPrincipal };

            _adoptionRequestServiceMock.Setup(service => service.UpdateAdoptionRequestAsync(It.IsAny<AdoptionRequest>()))
                .Returns(Task.CompletedTask);

            // E-posta servisi mock'lama
            _emailServiceMock.Setup(service => service.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.ApproveRequest(1, 1);

            // Assert
            _adoptionRequestServiceMock.Verify(service => service.UpdateAdoptionRequestAsync(It.Is<AdoptionRequest>(a => a.Status == AdoptionStatus.Approved)), Times.Once);

            _emailServiceMock.Verify(service => service.SendEmailAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.AtMost(0));
        }
    }
}
