using Moq;
using PetMe.Business.Services;
using PetMe.Data.Entities;
using PetMe.Data.Enums;
using PetMe.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMe.Tests.UnitTests.BusinessTest.ServiceTest
{
    public class AdoptionServiceTest
    {
        private readonly Mock<IAdoptionRepository> _mockRepository;
        private readonly AdoptionService _service;

        public AdoptionServiceTest()
        {
            _mockRepository = new Mock<IAdoptionRepository>();
            _service = new AdoptionService(_mockRepository.Object);
        }

        [Fact]
        public async Task CreateAdoptionAsync_PetIsNotAdopted_CreateAdoptSuccessfully()
        {
            // Arrange
            var adoption = new Adoption
            {
                PetId = 1,
                UserId = 1,
                AdoptionDate = DateTime.Now,
                Status = AdoptionStatus.Approved
            };

            _mockRepository.Setup(r => r.IsPetAlreadyAdoptedAsync(adoption.PetId)).ReturnsAsync(false);
            _mockRepository.Setup(r => r.AddAsync(adoption)).Returns(Task.CompletedTask);

            // Act
            await _service.CreateAdoptionAsync(adoption);

            // Assert
            _mockRepository.Verify(r => r.AddAsync(adoption), Times.Once);
        }

        [Fact]
        public async Task CreateAdoptionAsync_PetIsAlreadyAdopted_Failure()
        {
            // Arrange
            var adoption = new Adoption
            {
                PetId = 1,
                UserId = 1,
                AdoptionDate = DateTime.Now,
                Status = AdoptionStatus.Approved
            };

            _mockRepository.Setup(r => r.IsPetAlreadyAdoptedAsync(adoption.PetId)).ReturnsAsync(true);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAdoptionAsync(adoption));
        }

        [Fact]
        public async Task GetAdoptionByPetIdAsync_WhenAdoptionExists_ReturnCorrectAdoption()
        {
            // Arrange
            var adoption = new Adoption
            {
                PetId = 1,
                UserId = 1,
                AdoptionDate = DateTime.Now,
                Status = AdoptionStatus.Approved
            };

            _mockRepository.Setup(r => r.GetAdoptionByPetIdAsync(adoption.PetId)).ReturnsAsync(adoption);

            // Act
            var result = await _service.GetAdoptionByPetIdAsync(adoption.PetId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(adoption.PetId, result.PetId);
        }
    }
}
