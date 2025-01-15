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
    public class PetServiceTest
    {
        private readonly Mock<IPetRepository> _mockPetRepository;
        private readonly Mock<IPetOwnerRepository> _mockPetOwnerRepository;
        private readonly PetService _petService;

        public PetServiceTest()
        {
            _mockPetRepository = new Mock<IPetRepository>();
            _mockPetOwnerRepository = new Mock<IPetOwnerRepository>();
            _petService = new PetService(_mockPetRepository.Object, _mockPetOwnerRepository.Object);
        }

        [Fact]
        public async Task CreatePetAsync_PetIsValid_CreatePetSuccess()
        {
            // Arrange
            var pet = new Pet { Id = 1, Name = "Fluffy" };

            // Act
            await _petService.CreatePetAsync(pet);

            // Assert
            _mockPetRepository.Verify(r => r.AddAsync(It.Is<Pet>(p => p.Name == "Fluffy")), Times.Once);
        }

        [Fact]
        public async Task CreatePetAsync_PetIsNull_ThrowException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _petService.CreatePetAsync(null));
        }

        [Fact]
        public async Task GetPetByIdAsync_PetNotFound_ThrowException()
        {
            // Arrange
            var petId = 1;
            _mockPetRepository.Setup(r => r.GetByIdAsync(petId)).ReturnsAsync((Pet)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _petService.GetPetByIdAsync(petId));
        }

        [Fact]
        public async Task GetPetByIdAsync_ValidPetId_ReturnCorrentPet()
        {
            // Arrange
            var petId = 1;
            var pet = new Pet { Id = petId, Name = "Fluffy" };
            _mockPetRepository.Setup(r => r.GetByIdAsync(petId)).ReturnsAsync(pet);

            // Act
            var result = await _petService.GetPetByIdAsync(petId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Fluffy", result.Name);
        }

        [Fact]
        public async Task IsUserOwnerOfPetAsync_UserIsOwner_ReturnTrue()
        {
            // Arrange
            var petId = 1;
            var userId = 1;
            var petOwners = new List<PetOwner>
            {
                new PetOwner { PetId = petId, UserId = userId }
            };

            _mockPetRepository.Setup(r => r.GetPetOwnersAsync(petId)).ReturnsAsync(petOwners);

            // Act
            var result = await _petService.IsUserOwnerOfPetAsync(petId, userId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UpdatePetAsync_PetNotFound_ThrowException()
        {
            // Arrange
            var petId = 1;
            var updatedPet = new Pet { Id = petId, Name = "Fluffy" };
            _mockPetRepository.Setup(r => r.GetByIdAsync(petId)).ReturnsAsync((Pet)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _petService.UpdatePetAsync(petId, updatedPet, 1));
        }

        [Fact]
        public async Task UpdatePetAsync_UserIsNotOwner_ThrowException()
        {
            // Arrange
            var petId = 1;
            var updatedPet = new Pet { Id = petId, Name = "Fluffy" };
            var existingPet = new Pet { Id = petId, Name = "Fluffy" };

            _mockPetRepository.Setup(r => r.GetByIdAsync(petId)).ReturnsAsync(existingPet);
            _mockPetRepository.Setup(r => r.GetPetOwnersAsync(petId)).ReturnsAsync(new List<PetOwner>());

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _petService.UpdatePetAsync(petId, updatedPet, 2));
        }

        [Fact]
        public async Task AssignPetOwnerAsync_PetOwnerIsNull_ThrowException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _petService.AssignPetOwnerAsync(null));
        }

        [Fact]
        public async Task AssignPetOwnerAsync_ValidPetOwner_AssignSuccess()
        {
            // Arrange
            var petOwner = new PetOwner { PetId = 1, UserId = 1 };

            // Act
            await _petService.AssignPetOwnerAsync(petOwner);

            // Assert
            _mockPetOwnerRepository.Verify(r => r.AddAsync(It.Is<PetOwner>(po => po.PetId == 1 && po.UserId == 1)), Times.Once);
        }

        [Fact]
        public async Task DeletePetAsync_PetNotFound_ThrowException()
        {
            // Arrange
            var petId = 1;
            _mockPetRepository.Setup(r => r.GetByIdAsync(petId)).ReturnsAsync((Pet)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _petService.DeletePetAsync(petId, 1));
        }

        [Fact]
        public async Task DeletePetAsync_UserIsNotOwner_ThrowException()
        {
            // Arrange
            var petId = 1;
            var pet = new Pet { Id = petId, Name = "Fluffy" };
            _mockPetRepository.Setup(r => r.GetByIdAsync(petId)).ReturnsAsync(pet);
            _mockPetRepository.Setup(r => r.GetPetOwnersAsync(petId)).ReturnsAsync(new List<PetOwner>());

            // Act & Assert
            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _petService.DeletePetAsync(petId, 2));
        }

        [Fact]
        public async Task DeletePetAsync_UserIsOwner_DeleteSuccess()
        {
            // Arrange
            var petId = 1;
            var pet = new Pet { Id = petId, Name = "Fluffy" };
            _mockPetRepository.Setup(r => r.GetByIdAsync(petId)).ReturnsAsync(pet);
            _mockPetRepository.Setup(r => r.GetPetOwnersAsync(petId)).ReturnsAsync(new List<PetOwner>
            {
                new PetOwner { PetId = petId, UserId = 1 }
            });

            // Act
            await _petService.DeletePetAsync(petId, 1);

            // Assert
            _mockPetRepository.Verify(r => r.DeleteAsync(It.Is<Pet>(p => p.Id == petId)), Times.Once);
        }
    }
}
