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
    public class PetOwnerRepositoryTest : IDisposable
    {
        private readonly PetOwnerRepository _petOwnerRepository;
        private readonly PetMeDbContext _context;

        public PetOwnerRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<PetMeDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            _context = new PetMeDbContext(options);
            _petOwnerRepository = new PetOwnerRepository(_context);

            // Ensure the database is clean before each test
            _context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task AddAsync_ValidPetOwner_AddPetOwnerSuccess()
        {
            // Arrange
            var petOwner = new PetOwner
            {
                PetId = 1,
                UserId = 1,
                OwnershipDate = DateTime.UtcNow
            };

            // Act
            await _petOwnerRepository.AddAsync(petOwner);

            var petOwnersInDb = await _context.PetOwners.ToListAsync();

            // Assert
            Assert.Single(petOwnersInDb);
            Assert.Equal(1, petOwnersInDb[0].PetId);
            Assert.Equal(1, petOwnersInDb[0].UserId);
        }

        [Fact]
        public async Task GetPetOwnerByPetIdAsync_ValidPetId_ReturnPetOwner()
        {
            // Arrange
            // Create and add a User with required properties
            var user = new User
            {
                Id = 1,
                Username = "TestUser",
                Email = "testuser@example.com",
                PasswordHash = "hashedpassword",  // Add missing required properties
                PhoneNumber = "1234567890",       // Add missing required properties
                Address = "Test Address",         // Add missing required properties
                IsActive = true,
                CreatedDate = DateTime.UtcNow,
                ProfileImageUrl = "http://example.com/profile.jpg" // Add missing required properties
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync(); // Save the user to the database

            // Create and add a Pet with required properties
            var pet = new Pet
            {
                Id = 1,
                Name = "Buddy",
                Species = Species.Dog,
                Breed = "Golden Retriever",
                Age = 3,
                Gender = Gender.Male,
                Weight = 30.5,
                Description = "Friendly dog", // Add missing required property
                ImageUrl = "http://example.com/pet.jpg"
            };
            _context.Pets.Add(pet);
            await _context.SaveChangesAsync(); // Save the pet to the database

            // Create and add a PetOwner
            var petOwner = new PetOwner
            {
                PetId = 1,
                UserId = 1,
                OwnershipDate = DateTime.UtcNow
            };
            _context.PetOwners.Add(petOwner);
            await _context.SaveChangesAsync(); // Save the petOwner to the database

            // Act
            var result = await _petOwnerRepository.GetPetOwnerByPetIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.PetId);
            Assert.Equal(1, result.UserId);
        }

    }
}
