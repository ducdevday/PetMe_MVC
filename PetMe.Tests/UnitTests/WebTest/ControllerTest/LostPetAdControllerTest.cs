using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PetMe.Business.Services;
using PetMe.Data.Entities;
using PetMe.Web.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMe.Tests.UnitTests.WebTest.ControllerTest
{
    public class LostPetAdControllerTest
    {
        private readonly Mock<ILostPetAdService> _lostPetAdServiceMock;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<IVnAddressService> _vpAddressServiceMock;
        private readonly LostPetAdController _controller;

        private readonly Mock<HttpContext> _httpContextMock;
        private readonly Mock<ISession> _sessionMock;
        public LostPetAdControllerTest()
        {
            _lostPetAdServiceMock = new Mock<ILostPetAdService>();
            _emailServiceMock = new Mock<IEmailService>();
            _userServiceMock = new Mock<IUserService>();
            _vpAddressServiceMock = new Mock<IVnAddressService>();

            _controller = new LostPetAdController(
                _lostPetAdServiceMock.Object,
                _emailServiceMock.Object,
                _userServiceMock.Object,
                _vpAddressServiceMock.Object
            );

            _sessionMock = new Mock<ISession>();
            _httpContextMock = new Mock<HttpContext>();
            _httpContextMock.Setup(x => x.Session).Returns(_sessionMock.Object);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = _httpContextMock.Object
            };
        }

        
        [Fact]
        public async Task Create_Get_WhenNotLoggedIn_RedirectsToLogin()
        {
            // Arrange
            // No "Username" in session => not logged in

            // Act
            var result = await _controller.Create() as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Login", result.ActionName);
            Assert.Equal("Account", result.ControllerName);
        }

        [Fact]
        public async Task Create_Get_WhenLoggedIn_ReturnsViewWithCities()
        {
            // Arrange
            _sessionMock.Object.SetString("Username", "TestUser"); // user is logged in

            // Act
            var result = (await _controller.Create() )as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Null(result.ViewName);  // Default view
            Assert.True(result.ViewData.ContainsKey("Cities"));
            Assert.True(result.ViewData.ContainsKey("Districts"));
        }

        [Fact]
        public async Task Create_Post_WhenNotLoggedIn_RedirectsToLogin()
        {
            // Arrange
            var lostPetAd = new LostPetAd { PetName = "Fluffy" };
            var city = "İzmir";
            var district = "Bornova";

            // Not logged in => no "Username" in session

            // Act
            var result = await _controller.Create(lostPetAd, city, district) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Login", result.ActionName);
            Assert.Equal("Account", result.ControllerName);
        }




        [Fact]
        public async Task Index_WhenCalled_ReturnsViewWithLostPetAds()
        {
            // Arrange
            var ads = new List<LostPetAd>
        {
            new LostPetAd { Id = 1, PetName = "Cat" },
            new LostPetAd { Id = 2, PetName = "Dog" }
        };
            _lostPetAdServiceMock.Setup(s => s.GetAllLostPetAdsAsync())
                .ReturnsAsync(ads);

            // Act
            var result = await _controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(ads, result.Model);
        }


        [Fact]
        public async Task Details_WhenAdFound_ReturnsViewWithAd()
        {
            // Arrange
            var ad = new LostPetAd { Id = 10, PetName = "Birdy", User = new User { Username = "AdOwner" } };
            _lostPetAdServiceMock.Setup(s => s.GetLostPetAdByIdAsync(10))
                .ReturnsAsync(ad);
            _sessionMock.Object.SetString("Username", "AnotherUser");

            // Act
            var result = await _controller.Details(10) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(ad, result.Model);
            Assert.Equal("AnotherUser", _controller.ViewBag.CurrentUser);
        }

        [Fact]
        public async Task Edit_Get_WhenNotLoggedIn_RedirectsToLogin()
        {
            // Arrange
            // no "Username" => not logged in

            // Act
            var result = await _controller.Edit(1) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Login", result.ActionName);
            Assert.Equal("Account", result.ControllerName);
        }



        [Fact]
        public async Task Edit_Get_WhenValidOwner_ReturnsViewWithAd()
        {
            // Arrange
            _sessionMock.Object.SetString("Username", "AdOwner");
            var ad = new LostPetAd
            {
                Id = 10,
                User = new User { Username = "AdOwner" },
                LastSeenCity = "Manisa"
            };
            _lostPetAdServiceMock.Setup(s => s.GetLostPetAdByIdAsync(10)).ReturnsAsync(ad);

            // Act
            var result = await _controller.Edit(10) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(ad, result.Model);
            Assert.True(result.ViewData.ContainsKey("Cities"));
            Assert.True(result.ViewData.ContainsKey("Districts"));
        }



        [Fact]
        public async Task Delete_Get_WhenValidOwner_ReturnsView()
        {
            // Arrange
            _sessionMock.Object.SetString("Username", "Owner");
            var ad = new LostPetAd { Id = 10, User = new User { Username = "Owner" } };
            _lostPetAdServiceMock.Setup(s => s.GetLostPetAdByIdAsync(10))
                .ReturnsAsync(ad);

            // Act
            var result = await _controller.Delete(10) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(ad, result.Model);
        }
    }
}
