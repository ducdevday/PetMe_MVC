using PetMe.Data.Entities;
using PetMe.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMe.Tests.UnitTests.DataTest.EntityTest
{
    public class AdoptionRequestTest
    {
        [Fact]
        public void AdoptionRequest_ShouldHaveValidProperties_WhenInitialized()
        {
            // Arrange
            var adoptionRequest = new AdoptionRequest
            {
                PetId = 1,
                UserId = 1,
                Message = "I want to adopt this pet.",
                Status = AdoptionStatus.Pending,
                RequestDate = DateTime.UtcNow
            };

            // Act & Assert
            Assert.Equal(1, adoptionRequest.PetId);
            Assert.Equal(1, adoptionRequest.UserId);
            Assert.Equal("I want to adopt this pet.", adoptionRequest.Message);
            Assert.Equal(AdoptionStatus.Pending, adoptionRequest.Status);
            Assert.True(adoptionRequest.RequestDate <= DateTime.UtcNow);
        }
    }
}
