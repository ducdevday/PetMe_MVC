using Microsoft.EntityFrameworkCore;
using PetMe.Data;
using PetMe.Data.Entities;
using PetMe.DataAccess.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMe.Tests.UnitTests.DataAccessTest.RepositoriyTest
{
    public class LostPetAdRepositoryTest : IDisposable
    {

        private readonly LostPetAdRepository _lostPetAdRepository;
        private readonly PetMeDbContext _context;

        public LostPetAdRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<PetMeDbContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            _context = new PetMeDbContext(options);
            _lostPetAdRepository = new LostPetAdRepository(_context);

            // Ensure the database is clean before each test
            _context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Fact]
        public async Task CreateLostPetAdAsync_ValidLostPetAd_CreateLostPetAdSuccess()
        {
            // Arrange

            var lostPetAd = new LostPetAd
            {
                Id = 1,
                PetName = "Fluffy",
                Description = "White cat with blue eyes",
                LastSeenDate = DateTime.UtcNow.AddDays(-1),
                LastSeenCity = "CityX",
                LastSeenDistrict = "DistrictY",
                CreatedAt = DateTime.UtcNow,
                ImageUrl = "imageUrl",
                UserId = 123
            };

            // Act
            await _lostPetAdRepository.CreateLostPetAdAsync(lostPetAd);

            // Assert
            var inserted = await _context.LostPetAds.FindAsync(1);
            Assert.NotNull(inserted);
            Assert.Equal("Fluffy", inserted.PetName);
            Assert.Equal("White cat with blue eyes", inserted.Description);
            Assert.Equal("CityX", inserted.LastSeenCity);
            Assert.Equal("DistrictY", inserted.LastSeenDistrict);
            Assert.Equal(123, inserted.UserId);
        }

        [Fact]
        public async Task GetAllAsync_ExistingLostPetAds_ReturnsAllAds()
        {
            // Arrange
            var ads = new List<LostPetAd>
            {
                new LostPetAd { Id = 1, PetName = "Cat1", Description = "White cat with blue eyes", ImageUrl = "imageUrl", LastSeenDate = DateTime.UtcNow.AddDays(-1) },
                new LostPetAd { Id = 2, PetName = "Cat2", Description = "White cat with blue eyes", ImageUrl = "imageUrl", LastSeenDate = DateTime.UtcNow.AddDays(-1) }
            };
            _context.LostPetAds.AddRange(ads);
            await _context.SaveChangesAsync();

            // Act
            var result = await _lostPetAdRepository.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            var list = result.ToList();
            Assert.Equal(2, list.Count);
            Assert.Contains(list, ad => ad.PetName == "Cat1");
            Assert.Contains(list, ad => ad.PetName == "Cat2");
        }

        [Fact]
        public async Task GetByIdAsync_ValidId_ReturnsCorrectLostPetAd()
        {
            // Arrange
            var ad = new LostPetAd { Id = 10, PetName = "Doggo", Description = "White cat with blue eyes", ImageUrl = "imageUrl", LastSeenDate = DateTime.UtcNow.AddDays(-1) };
            _context.LostPetAds.Add(ad);
            await _context.SaveChangesAsync();

            // Act
            var result = await _lostPetAdRepository.GetByIdAsync(10);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Doggo", result.PetName);
        }

        [Fact]
        public async Task UpdateLostPetAdAsync_ValidLostPetAd_UpdatesAdSuccessfully()
        {
            // Arrange
            var ad = new LostPetAd
            {
                Id = 2,
                PetName = "Lost Kitty",
                Description = "Initial description",
                ImageUrl = "imageUrl",
            };
            _context.LostPetAds.Add(ad);
            await _context.SaveChangesAsync();

            // Act
            ad.Description = "Updated description";
            await _lostPetAdRepository.UpdateLostPetAdAsync(ad);

            // Assert
            var updatedAd = await _context.LostPetAds.FindAsync(2);
            Assert.NotNull(updatedAd);
            Assert.Equal("Updated description", updatedAd.Description);
        }

        [Fact]
        public async Task DeleteLostPetAdAsync_ValidLostPetAd_DeletesAdSuccessfully()
        {
            // Arrange
            var ad = new LostPetAd { Id = 3, PetName = "DogLost", Description = "White cat with blue eyes", ImageUrl = "imageUrl" };
            _context.LostPetAds.Add(ad);
            await _context.SaveChangesAsync();

            // Act
            await _lostPetAdRepository.DeleteLostPetAdAsync(ad);

            // Assert
            var deletedAd = await _context.LostPetAds.FindAsync(3);
            Assert.Null(deletedAd);
        }
    }
}
