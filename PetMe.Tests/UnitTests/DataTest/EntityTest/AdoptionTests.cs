using PetMe.Data.Entities;
using PetMe.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMe.Tests.UnitTests.DataTest.EntityTest
{
    public class AdoptionTests
    {
        [Fact]
        public void Adoption_ShouldHaveCorrectRelationships_WhenInitialized()
        {
            // Arrange
            var adoption = new Adoption
            {
                PetId = 1,
                UserId = 1,
                AdoptionDate = DateTime.UtcNow,
                Status = AdoptionStatus.Approved,
                Pet = new Pet { Id = 1, Name = "Fluffy" },
                User = new User { Id = 1, Username = "JohnDoe" }
            };

            // Act & Assert
            Assert.Equal(1, adoption.PetId);
            Assert.Equal(1, adoption.UserId);
            Assert.Equal(AdoptionStatus.Approved, adoption.Status);
            Assert.NotNull(adoption.Pet);
            Assert.Equal("Fluffy", adoption.Pet.Name);
            Assert.NotNull(adoption.User);
            Assert.Equal("JohnDoe", adoption.User.Username);
        }
    }
}
