using PetMe.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMe.Tests.UnitTests.DataTest.EntityTest
{
    public class LostPetAdTests
    {
        [Fact]
        public void LostPetAd_ShouldHaveValidProperties_WhenInitialized()
        {
            // Arrange
            var now = DateTime.UtcNow;
            var user = new User
            {
                Id = 10,
                Username = "PetLover",
                Email = "petlover@example.com"
            };

            var lostPetAd = new LostPetAd
            {
                Id = 1,
                PetName = "Rex",
                Description = "Lost dog near the park",
                LastSeenDate = new DateTime(2023, 12, 25),
                ImageUrl = "http://example.com/images/rex.jpg",
                UserId = user.Id,
                User = user,
                LastSeenCity = "TestCity",
                LastSeenDistrict = "TestDistrict",
                CreatedAt = now
            };

            // Act & Assert
            Assert.Equal(1, lostPetAd.Id);
            Assert.Equal("Rex", lostPetAd.PetName);
            Assert.Equal("Lost dog near the park", lostPetAd.Description);
            Assert.Equal(new DateTime(2023, 12, 25), lostPetAd.LastSeenDate);
            Assert.Equal("http://example.com/images/rex.jpg", lostPetAd.ImageUrl);
            Assert.Equal(10, lostPetAd.UserId);

            Assert.NotNull(lostPetAd.User);
            Assert.Equal("PetLover", lostPetAd.User.Username);

            Assert.Equal("TestCity", lostPetAd.LastSeenCity);
            Assert.Equal("TestDistrict", lostPetAd.LastSeenDistrict);
            Assert.Equal(now, lostPetAd.CreatedAt);  // Confirm we can set a custom time
        }

        [Fact]
        public void PetOwner_ShouldHaveValidRelationships_WhenInitialized()
        {
            // Arrange
            var petOwner = new PetOwner
            {
                PetId = 1,
                UserId = 1,
                OwnershipDate = DateTime.UtcNow,
                Pet = new Pet { Id = 1, Name = "Fluffy" },
                User = new User { Id = 1, Username = "JohnDoe" }
            };

            // Act & Assert
            Assert.Equal(1, petOwner.PetId);
            Assert.Equal(1, petOwner.UserId);
            Assert.True(petOwner.OwnershipDate <= DateTime.UtcNow);
            Assert.NotNull(petOwner.Pet);
            Assert.Equal("Fluffy", petOwner.Pet.Name);
            Assert.NotNull(petOwner.User);
            Assert.Equal("JohnDoe", petOwner.User.Username);
        }
    }
}
