using Microsoft.EntityFrameworkCore;
using PetMe.Data;
using PetMe.Data.Entities;
using PetMe.Data.Enums;
using PetMe.DataAccess.Repositories;
using PetMe.Tests.UnitTests.DataTest.EnumTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMe.Tests.UnitTests.DataAccessTest.RepositoriyTest
{
    public class PetRepositoryTest : IDisposable
    {
        private readonly PetRepository _petRepository;
        private readonly PetMeDbContext _context;

        public PetRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<PetMeDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            _context = new PetMeDbContext(options);
            _petRepository = new PetRepository(_context);

            _context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task AddAsync_ValidPet_AddPetSuccessfully()
        {
            // Arrange
            var pet = new Pet
            {
                Id = 1,
                Name = "Fluffy",
                Age = 2,
                Breed = "Golden Retriever",
                Description = "Friendly dog",
                Gender = Gender.Male,
                ImageUrl = "https://example.com/fluffy.jpg",
                Species = Species.Dog,
            };

            // Act
            await _petRepository.AddAsync(pet);
            var petsInDb = await _context.Pets.ToListAsync();

            // Assert
            Assert.Single(petsInDb);
            Assert.Equal("Fluffy", petsInDb[0].Name);
        }

        [Fact]
        public async Task GetAllAsync_ExistingLostPetAds_ReturnAllPetAds()
        {
            // Arrange
            _context.Pets.AddRange(
                new Pet
                {
                    Id = 1,
                    Name = "Fluffy",
                    Age = 2,
                    Breed = "Golden Retriever",
                    Description = "Friendly dog",
                    Gender = Gender.Female,
                    ImageUrl = "https://example.com/fluffy.jpg",
                    Species = Species.Dog,
                },
                new Pet
                {
                    Id = 2,
                    Name = "Buddy",
                    Age = 3,
                    Breed = "Labrador",
                    Description = "Energetic dog",
                    Gender = Gender.Male,
                    ImageUrl = "https://example.com/buddy.jpg",
                    Species = Species.Hamster,
                }
            );
            await _context.SaveChangesAsync();

            // Act
            var pets = await _petRepository.GetAllAsync();

            // Assert
            Assert.Equal(2, pets.Count);
        }

        [Fact]
        public async Task GetByIdAsync_ValidPetId_ReturnCorrectPet()
        {
            // Arrange
            var pet = new Pet
            {
                Id = 1,
                Name = "Fluffy",
                Age = 2,
                Breed = "Golden Retriever",
                Description = "Friendly dog",
                Gender = Gender.Male,
                ImageUrl = "https://example.com/fluffy.jpg",
                Species = Species.Dog,
            };
            _context.Pets.Add(pet);
            await _context.SaveChangesAsync();

            // Act
            var result = await _petRepository.GetByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Fluffy", result.Name);
        }

        [Fact]
        public async Task GetPetOwnersAsync_ValidPetId_ReturnCorrectPetOwner()
        {
            // Arrange
            _context.PetOwners.AddRange(
                new PetOwner { PetId = 1, UserId = 1, OwnershipDate = DateTime.UtcNow },
                new PetOwner { PetId = 2, UserId = 2, OwnershipDate = DateTime.UtcNow }
            );
            await _context.SaveChangesAsync();

            // Act
            var petOwners = await _petRepository.GetPetOwnersAsync(1);

            // Assert
            Assert.Single(petOwners);
            Assert.Equal(1, petOwners[0].UserId);
        }

        [Fact]
        public async Task UpdateAsync_ValidPet_UpdatePetSuccessfully()
        {
            // Arrange
            var pet = new Pet
            {
                Id = 1,
                Name = "Fluffy",
                Age = 2,
                Breed = "Golden Retriever",
                Description = "Friendly dog",
                Gender = Gender.Male,
                ImageUrl = "https://example.com/fluffy.jpg",
                Species = Species.Dog,
            };
            _context.Pets.Add(pet);
            await _context.SaveChangesAsync();

            // Act
            pet.Name = "FluffyUpdated";
            await _petRepository.UpdateAsync(pet);
            var updatedPet = await _context.Pets.FindAsync(1);

            // Assert
            Assert.Equal("FluffyUpdated", updatedPet.Name);
        }

        [Fact]
        public async Task DeleteAsync_ValidPet_DeletePetSuccessfully()
        {
            // Arrange
            var pet = new Pet
            {
                Id = 1,
                Name = "Fluffy",
                Age = 2,
                Breed = "Golden Retriever",
                Description = "Friendly dog",
                Gender = Gender.Male,
                ImageUrl = "https://example.com/fluffy.jpg",
                Species = Species.Dog
            };
            _context.Pets.Add(pet);
            await _context.SaveChangesAsync();

            // Act
            await _petRepository.DeleteAsync(pet);
            var petsInDb = await _context.Pets.ToListAsync();

            // Assert
            Assert.Empty(petsInDb);
        }
    }
}
