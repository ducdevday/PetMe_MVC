using Microsoft.EntityFrameworkCore;
using PetMe.Data;
using PetMe.Data.Entities;
using PetMe.Data.Enums;
using PetMe.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMe.Tests.UnitTests.DataAccessTest.RepositoriyTest
{
    public class AdoptionRepositoryTest :IDisposable
    {
        private readonly AdoptionRepository _adoptionRepository;
        private readonly PetMeDbContext _context;

        public AdoptionRepositoryTest()
        {
            // Using in-memory database for testing
            var options = new DbContextOptionsBuilder<PetMeDbContext>()
                .UseInMemoryDatabase(databaseName:Guid.NewGuid().ToString())
                .Options;

            _context = new PetMeDbContext(options);
            _adoptionRepository = new AdoptionRepository(_context);

            // Ensure the database is clean before each test
            _context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task AddAsync_ValidAdoption_AddsAdoptionSuccessfully()
        {
            // Arrange
            var adoption = new Adoption
            {
                Id = 1,  
                PetId = 100,
                UserId = 200,
                AdoptionDate = DateTime.UtcNow,
                Status = AdoptionStatus.Approved
            };

            // Act
            await _adoptionRepository.AddAsync(adoption);
            await _context.SaveChangesAsync();

            // Assert
            var addedAdoption = await _context.Adoptions.FindAsync(1);
            Assert.NotNull(addedAdoption);
            Assert.Equal(adoption.PetId, addedAdoption.PetId);
        }

        [Fact]
        public async Task GetAdoptionByPetIdAsync_ValidPetId_ReturnsCorrectAdoption()
        {
            // Arrange
            int petId = 100;
            var adoption = new Adoption
            {
                Id = 1,  
                PetId = petId,
                UserId = 200,
                AdoptionDate = DateTime.UtcNow,
                Status = AdoptionStatus.Approved
            };

            await _context.Adoptions.AddAsync(adoption);
            await _context.SaveChangesAsync();

            // Act
            var result = await _adoptionRepository.GetAdoptionByPetIdAsync(petId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(petId, result.PetId);
        }

        [Fact]
        public async Task IsPetAlreadyAdoptedAsync_PetAlreadyAdopted_ReturnsTrue()
        {
            // Arrange
            int petId = 100;
            var adoption = new Adoption
            {
                Id = 1,  
                PetId = petId,
                UserId = 200,
                AdoptionDate = DateTime.UtcNow,
                Status = AdoptionStatus.Approved
            };

            await _context.Adoptions.AddAsync(adoption);
            await _context.SaveChangesAsync();

            // Act
            var result = await _adoptionRepository.IsPetAlreadyAdoptedAsync(petId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetAdoptionRequestByUserAndPetAsync_ValidUserAndPetIds_ReturnsAdoptionRequest()
        {
            // Arrange
            int userId = 200;
            int petId = 100;
            var adoptionRequest = new AdoptionRequest
            {
                Id = 1,  // Ensure unique ID for each test
                PetId = petId,
                UserId = userId,
                Message = "I want to adopt this pet.",
                Status = AdoptionStatus.Pending,
                RequestDate = DateTime.UtcNow
            };

            await _context.AdoptionRequests.AddAsync(adoptionRequest);
            await _context.SaveChangesAsync();

            // Act
            var result = await _adoptionRepository.GetAdoptionRequestByUserAndPetAsync(userId, petId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
            Assert.Equal(petId, result.PetId);
        }

    }
}
