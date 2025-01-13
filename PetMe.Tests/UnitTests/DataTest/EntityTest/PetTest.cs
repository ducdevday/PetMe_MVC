using PetMe.Data.Entities;
using PetMe.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMe.Tests.UnitTests.DataTest.EntityTest
{
    public class PetTest
    {
        [Fact]
        public void Pet_ShouldHaveDefaultValues_WhenInitialized()
        {
            // Arrange & Act
            var pet = new Pet();

            // Assert
            Assert.Equal(0, pet.Id);
            Assert.Null(pet.Name);
            Assert.Equal(Species.Dog,pet.Species);
            Assert.Null(pet.Breed);
            Assert.Equal(0, pet.Age);
            Assert.Equal(Gender.Male,pet.Gender);
            Assert.Equal(0.0, pet.Weight);
            Assert.Null(pet.Description);
            Assert.Null(pet.ImageUrl);
            Assert.Empty(pet.AdoptionRequests);
            Assert.Empty(pet.PetOwners);
        }
    }
}
