using Moq;
using PetMe.Business.Services;
using PetMe.Data.Entities;
using PetMe.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMe.Tests.UnitTests.BusinessTest.ServiceTest
{
    public class LostPetAdServiceTest
    {
        private readonly Mock<ILostPetAdRepository> _lostPetAdRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IEmailService> _emailServiceMock;

        // System Under Test (SUT)
        private readonly LostPetAdService _lostPetAdService;

        public LostPetAdServiceTest()
        {
            _lostPetAdRepositoryMock = new Mock<ILostPetAdRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _emailServiceMock = new Mock<IEmailService>();

            _lostPetAdService = new LostPetAdService(
                _lostPetAdRepositoryMock.Object,
                _userRepositoryMock.Object,
                _emailServiceMock.Object
            );
        }


        [Fact]
        public async Task CreateLostPetAdAsync_ValidLostPetAd_CreateSuccessfully()
        {
            // Arrange
            var lostPetAd = new LostPetAd
            {
                Id = 1,
                UserId = 123,
            };
            var city = "TestCity";
            var district = "TestDistrict";
            var user = new User
            {
                Id = 123,
                Email = "user123@test.com"
            };

            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(lostPetAd.UserId))
                .ReturnsAsync(user);

            _userRepositoryMock
                .Setup(x => x.GetUsersByLocationAsync(city, district))
                .ReturnsAsync(new List<User>()); // empty list for simplicity

            // Act
            await _lostPetAdService.CreateLostPetAdAsync(lostPetAd, city, district);

            // Assert
            Assert.Equal(city, lostPetAd.LastSeenCity);
            Assert.Equal(district, lostPetAd.LastSeenDistrict);

            _lostPetAdRepositoryMock
                .Verify(x => x.CreateLostPetAdAsync(It.Is<LostPetAd>(
                    ad => ad.LastSeenCity == city && ad.LastSeenDistrict == district
                )), Times.Once);
        }

        [Fact]
        public async Task CreateLostPetAdAsync_UserNotFound_ThrowException()
        {
            // Arrange
            var lostPetAd = new LostPetAd
            {
                Id = 1,
                UserId = 999, // Non-existent user
            };

            _userRepositoryMock
                .Setup(x => x.GetByIdAsync(lostPetAd.UserId))
                .ReturnsAsync((User?)null); // user not found

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _lostPetAdService.CreateLostPetAdAsync(lostPetAd, "City", "District"));
        }
        



        [Fact]
        public async Task GetLostPetAdByIdAsync_ValidLostPetAdId_ReturnCorrectLostPetAd()
        {
            // Arrange
            var lostPetAd = new LostPetAd
            {
                Id = 10,
                UserId = 999
            };

            _lostPetAdRepositoryMock
                .Setup(x => x.GetByIdAsync(lostPetAd.Id))
                .ReturnsAsync(lostPetAd);

            // Act
            var result = await _lostPetAdService.GetLostPetAdByIdAsync(lostPetAd.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(lostPetAd.Id, result.Id);
        }

        [Fact]
        public async Task UpdateLostPetAdAsync_ValidChanges_UpdatesSuccessfully()
        {
            // Arrange
            var lostPetAd = new LostPetAd
            {
                Id = 1,
            };

            _lostPetAdRepositoryMock
                .Setup(x => x.UpdateLostPetAdAsync(lostPetAd))
                .Returns(Task.CompletedTask);

            // Act
            await _lostPetAdService.UpdateLostPetAdAsync(lostPetAd);

        }

        [Fact]
        public async Task DeleteLostPetAdAsync_ValidLostPetAdId_DeletesLostPetAdSuccess()
        {
            // Arrange
            var lostPetAd = new LostPetAd
            {
                Id = 3,
            };

            _lostPetAdRepositoryMock
                .Setup(x => x.DeleteLostPetAdAsync(lostPetAd))
                .Returns(Task.CompletedTask);

            // Act
            await _lostPetAdService.DeleteLostPetAdAsync(lostPetAd);

            // Assert
            _lostPetAdRepositoryMock.Verify(
                x => x.DeleteLostPetAdAsync(It.Is<LostPetAd>(ad => ad.Id == 3)),
                Times.Once
            );
        }
    }
}
