﻿using Microsoft.AspNetCore.Http;
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
    public class AccountControllerTest
    {
        private readonly Mock<IUserService> _mockUserService;
        private readonly Mock<IVnAddressService> _mockAddressService;
        private readonly AccountController _controller;

        public AccountControllerTest()
        {
            _mockUserService = new Mock<IUserService>();
            _mockAddressService = new Mock<IVnAddressService>();
            _controller = new AccountController(_mockUserService.Object, _mockAddressService.Object);

            // Mocking HttpContext and Session
            var httpContext = new DefaultHttpContext();
            var sessionMock = new Mock<ISession>();
            httpContext.Session = sessionMock.Object;

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }

        [Fact]
        public void Login_Get_ReturnsView()
        {
            // Act
            var result = _controller.Login();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Login_Post_InvalidCredentials_ReturnsViewWithError()
        {
            // Arrange
            string username = "testuser";
            string password = "wrongpassword";
            _mockUserService.Setup(s => s.AuthenticateAsync(username, password)).ReturnsAsync((User?)null);

            // Act
            var result = await _controller.Login(username, password);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.False(_controller.ModelState.IsValid);
        }

        [Fact]
        public async Task Login_Post_ValidCredentials_RedirectsToHome()
        {
            // Arrange
            string username = "testuser";
            string password = "correctpassword";
            var user = new User { Id = 1, Username = username };
            _mockUserService.Setup(s => s.AuthenticateAsync(username, password)).ReturnsAsync(user);

            // Act
            var result = await _controller.Login(username, password);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName); 
            Assert.Equal("Home", redirectResult.ControllerName);
        }

        [Fact]
        public async Task Register_Get_ReturnsView()
        {
            // Act
            var result = await _controller.Register();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Register_Post_InvalidModelState_ReturnsViewWithError()
        {
            // Arrange
            _controller.ModelState.AddModelError("Username", "Username is required.");
            var user = new User
            {
                Username = "",
                Email = "test@example.com",
                PasswordHash = "password",
                PhoneNumber = "1234567890",
                Address = "123 Test St.",
                DateOfBirth = DateTime.Now.AddYears(-30),
                ProfileImageUrl = "profile.jpg",
                City = "TestCity",  // Add city
                District = "TestDistrict"  // Add district
            };

            // Act
            var result = await _controller.Register(user.Username, user.Email, user.PasswordHash, user.PhoneNumber, user.Address, user.DateOfBirth, user.ProfileImageUrl, user.City, user.District);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.False(_controller.ModelState.IsValid);
        }

        [Fact]
        public async Task Register_Post_ValidModelState_RedirectsToLogin()
        {
            // Arrange
            var user = new User
            {
                Username = "testuser",
                Email = "test@example.com",
                PasswordHash = "password",
                PhoneNumber = "1234567890",
                Address = "123 Test St.",
                DateOfBirth = DateTime.Now.AddYears(-30),
                ProfileImageUrl = "profile.jpg",
                City = "TestCity",  // Add city
                District = "TestDistrict"  // Add district
            };

            _mockUserService.Setup(s => s.RegisterAsync(It.IsAny<User>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Register(user.Username, user.Email, user.PasswordHash, user.PhoneNumber, user.Address, user.DateOfBirth, user.ProfileImageUrl, user.City, user.District);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Login", redirectResult.ActionName);
        }

        [Fact]
        public void Logout_ClearsSession_RedirectsToLogin()
        {
            // Act
            var result = _controller.Logout();

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result); 
            Assert.Equal("Login", redirectResult.ActionName);
        }
    }
}
