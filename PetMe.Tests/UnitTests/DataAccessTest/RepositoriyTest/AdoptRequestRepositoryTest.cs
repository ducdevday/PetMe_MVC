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
    public class AdoptRequestRepositoryTest : IDisposable
    {
        private readonly AdoptionRequestRepository _adoptionRequestRepository;
        private readonly PetMeDbContext _context;

        public AdoptRequestRepositoryTest()
        {
            // Using in-memory database for testing
            var options = new DbContextOptionsBuilder<PetMeDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new PetMeDbContext(options);
            _adoptionRequestRepository = new AdoptionRequestRepository(_context);

            // Ensure the database is clean before each test
            _context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task GetAdoptionRequestsByPetIdAsync_ValidPetId_ReturnCorrectAdoptRequestList() {
            // Arrange
            var petId = 1;
            var userId = 1;

            // Create and add Pet and User entities
            var pet = new Pet
            {
                Id = petId,
                Name = "Buddy",
                Species = Species.Dog,
                Breed = "Golden Retriever",
                Age = 3,
                Gender = Gender.Male,
                Weight = 30.5,
                Description = "Friendly dog",
                ImageUrl = "http://example.com/pet.jpg"
            };

            var user = new User
            {
                Id = userId,
                Username = "TestUser",
                Email = "testuser@example.com",
                PasswordHash = "hashedpassword",
                PhoneNumber = "1234567890",
                Address = "Test Address",
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                ProfileImageUrl = "http://example.com/profile.jpg"
            };

            var adoptionRequests = new List<AdoptionRequest>
            {
                new AdoptionRequest { Id = 1, PetId = petId, UserId = userId, Status = AdoptionStatus.Pending, RequestDate = DateTime.Now, Pet = pet, User = user },
                new AdoptionRequest { Id = 2, PetId = petId, UserId = userId, Status = AdoptionStatus.Approved, RequestDate = DateTime.Now, Pet = pet, User = user }
            };

            _context.Pets.Add(pet);
            _context.Users.Add(user);
            _context.AdoptionRequests.AddRange(adoptionRequests);

            await _context.SaveChangesAsync();

            // Act
            var result = await _adoptionRequestRepository.GetAdoptionRequestsByPetIdAsync(petId);

            // Assert
            Assert.Equal(2, result.Count); // Verify two requests for the pet
            Assert.All(result, r => Assert.Equal(petId, r.PetId)); // Ensure each request is for the correct pet
        }

        [Fact]
        public async Task GetPendingRequestsByPetIdAsync_ValidPetId_ReturnCorrectAdoptRequestList() {
            // Arrange
            var petId = 1;
            var userId = 1;

            // Create and add Pet and User entities
            var pet = new Pet
            {
                Id = petId,
                Name = "Buddy",
                Species = Species.Dog,
                Breed = "Golden Retriever",
                Age = 3,
                Gender = Gender.Male,
                Weight = 30.5,
                Description = "Friendly dog",
                ImageUrl = "http://example.com/pet.jpg"
            };

            var user = new User
            {
                Id = userId,
                Username = "TestUser",
                Email = "testuser@example.com",
                PasswordHash = "hashedpassword",
                PhoneNumber = "1234567890",
                Address = "Test Address",
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                ProfileImageUrl = "http://example.com/profile.jpg"
            };

            var adoptionRequests = new List<AdoptionRequest>
            {
                new AdoptionRequest { Id = 1, PetId = petId, UserId = userId, Status = AdoptionStatus.Pending, RequestDate = DateTime.Now, Pet = pet, User = user },
                new AdoptionRequest { Id = 2, PetId = petId, UserId = userId, Status = AdoptionStatus.Approved, RequestDate = DateTime.Now, Pet = pet, User = user }
            };

            _context.Pets.Add(pet);
            _context.Users.Add(user);
            _context.AdoptionRequests.AddRange(adoptionRequests);
            await _context.SaveChangesAsync();

            // Act
            var result = await _adoptionRequestRepository.GetPendingRequestsByPetIdAsync(petId);

            // Assert
            Assert.Single(result); // Only one request should be "Pending"
            Assert.Equal(AdoptionStatus.Pending, result.First().Status); // Ensure the status is "Pending"
        }

        [Fact]
        public async Task GetByIdAsync_ValidAdoptRequestId_ReturnCorrectAdoptRequest() {
            // Arrange
            var petId = 1;
            var userId = 1;

            // Create and add Pet and User entities
            var pet = new Pet
            {
                Id = petId,
                Name = "Buddy",
                Species = Species.Dog,
                Breed = "Golden Retriever",
                Age = 3,
                Gender = Gender.Male,
                Weight = 30.5,
                Description = "Friendly dog",
                ImageUrl = "http://example.com/pet.jpg"
            };

            var user = new User
            {
                Id = userId,
                Username = "TestUser",
                Email = "testuser@example.com",
                PasswordHash = "hashedpassword",
                PhoneNumber = "1234567890",
                Address = "Test Address",
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                ProfileImageUrl = "http://example.com/profile.jpg"
            };

            var adoptionRequest = new AdoptionRequest
            {
                Id = 1,
                PetId = petId,
                UserId = userId,
                Status = AdoptionStatus.Pending,
                RequestDate = DateTime.Now,
                Pet = pet,
                User = user
            };

            _context.Pets.Add(pet);
            _context.Users.Add(user);
            _context.AdoptionRequests.Add(adoptionRequest);
            await _context.SaveChangesAsync();

            // Act
            var result = await _adoptionRequestRepository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result); // Ensure the result is not null
            Assert.Equal(adoptionRequest.Id, result.Id); // Verify the correct adoption request is returned
            Assert.Equal(AdoptionStatus.Pending, result.Status); // Verify the status is "Pending"
        }

        [Fact]
        public async Task GetByIdAsync_NotExistID_ReturnNull()
        {
            // Act
            var result = await _adoptionRequestRepository.GetByIdAsync(-1);

            // Assert
            Assert.Null(result); // Ensure null is returned when the request doesn't exist
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateRequestStatus()
        {
            // Arrange
            var petId = 1;
            var userId = 1;

            // Create and add Pet and User entities
            var pet = new Pet
            {
                Id = petId,
                Name = "Buddy",
                Species = Species.Dog,
                Breed = "Golden Retriever",
                Age = 3,
                Gender = Gender.Male,
                Weight = 30.5,
                Description = "Friendly dog",
                ImageUrl = "http://example.com/pet.jpg"
            };

            var user = new User
            {
                Id = userId,
                Username = "TestUser",
                Email = "testuser@example.com",
                PasswordHash = "hashedpassword",
                PhoneNumber = "1234567890",
                Address = "Test Address",
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                ProfileImageUrl = "http://example.com/profile.jpg"
            };

            var adoptionRequest = new AdoptionRequest
            {
                Id = 1,
                PetId = petId,
                UserId = userId,
                Status = AdoptionStatus.Pending,
                RequestDate = DateTime.Now,
                Pet = pet,
                User = user
            };

            _context.Pets.Add(pet);
            _context.Users.Add(user);
            _context.AdoptionRequests.Add(adoptionRequest);
            await _context.SaveChangesAsync();

            // Act
            adoptionRequest.Status = AdoptionStatus.Approved; // Update the status to Approved
            await _adoptionRequestRepository.UpdateAsync(adoptionRequest);

            // Assert
            var updatedRequest = await _context.AdoptionRequests.FindAsync(1);
            Assert.Equal(AdoptionStatus.Approved, updatedRequest.Status); // Ensure the status was updated to "Approved"
        }
    }
}
