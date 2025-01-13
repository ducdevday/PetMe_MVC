using PetMe.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMe.Tests.UnitTests.DataTest.EnumTest
{
    public class SpeciesTest
    {
        [Fact]
        public void HelpRequestStatus_Values_ShouldMatchExpectedIntegers()
        {
            // By default, the first enum value is 0, the second is 1, etc.
            Assert.Equal(0, (int)HelpRequestStatus.Active);
            Assert.Equal(1, (int)HelpRequestStatus.Passive);
        }

        [Theory]
        [InlineData("Active", HelpRequestStatus.Active)]
        [InlineData("Passive", HelpRequestStatus.Passive)]
        public void HelpRequestStatus_ParseFromString_ReturnsCorrectEnum(string input, HelpRequestStatus expected)
        {
            // Act
            var parseSuccess = Enum.TryParse<HelpRequestStatus>(input, out var result);

            // Assert
            Assert.True(parseSuccess);
            Assert.Equal(expected, result);
        }
    }
}
