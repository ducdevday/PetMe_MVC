using PetMe.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMe.Tests.UnitTests.DataTest.EnumTest
{
    public class AdoptionStatusTest
    {
        [Fact]
        public void AdoptionStatus_ShouldHaveCorrectValues()
        {
            // Act & Assert
            Assert.Equal(0, (int)AdoptionStatus.Pending);  
            Assert.Equal(1, (int)AdoptionStatus.Approved); 
            Assert.Equal(2, (int)AdoptionStatus.Rejected); 
        }

        [Fact]
        public void AdoptionStatus_ShouldContainAllValues()
        {
            // Act & Assert
            var statusValues = Enum.GetValues(typeof(AdoptionStatus)).Cast<AdoptionStatus>().ToArray();

            Assert.Contains(AdoptionStatus.Pending, statusValues);
            Assert.Contains(AdoptionStatus.Approved, statusValues);
            Assert.Contains(AdoptionStatus.Rejected, statusValues);
        }

        [Fact]
        public void AdoptionStatus_ShouldReturnCorrectStringRepresentation()
        {
            // Act & Assert
            Assert.Equal("Pending", AdoptionStatus.Pending.ToString());
            Assert.Equal("Approved", AdoptionStatus.Approved.ToString());
            Assert.Equal("Rejected", AdoptionStatus.Rejected.ToString());
        }
    }
}
