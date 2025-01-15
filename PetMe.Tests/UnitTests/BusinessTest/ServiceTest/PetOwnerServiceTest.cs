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
    public class PetOwnerServiceTest
    {
        private readonly Mock<IPetOwnerRepository> _mockPetOwnerRepository;
        private readonly PetOwnerService _petOwnerService;

        public PetOwnerServiceTest()
        {
            _mockPetOwnerRepository = new Mock<IPetOwnerRepository>();
            _petOwnerService = new PetOwnerService(_mockPetOwnerRepository.Object);
        }
       

        [Fact]
        public async Task GetPetOwnerByPetIdAsync_ValidPetId_ReturnPetOwner()
        {
            // Arrange
            var petId = 1;
            var expectedPetOwner = new PetOwner
            {
                PetId = petId,
                UserId = 1,
                OwnershipDate = DateTime.Now,
                Pet = new Pet { Id = petId, Name = "Fluffy", Species = Species.Cat },
                User = new User { Id = 1, Username = "jane_doe" }
            };

            _mockPetOwnerRepository
                .Setup(r => r.GetPetOwnerByPetIdAsync(petId))
                .ReturnsAsync(expectedPetOwner);

            // Act
            var result = await _petOwnerService.GetPetOwnerByPetIdAsync(petId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedPetOwner.PetId, result.PetId);
            Assert.Equal(expectedPetOwner.UserId, result.UserId);
            Assert.Equal(expectedPetOwner.Pet.Name, result.Pet.Name);
            Assert.Equal(expectedPetOwner.User.Username, result.User.Username);
        }

        [Fact]
        public async Task GetPetOwnerByPetIdAsync_InvalidPetId_ThrowException()
        {
            // Arrange
            var petId = 1;

            _mockPetOwnerRepository
                .Setup(r => r.GetPetOwnerByPetIdAsync(petId))
                .ReturnsAsync((PetOwner?)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _petOwnerService.GetPetOwnerByPetIdAsync(petId));
        }

    }
}
