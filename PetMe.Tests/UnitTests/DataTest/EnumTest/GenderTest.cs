using PetMe.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMe.Tests.UnitTests.DataTest.EnumTest
{
    public class GenderTest
    {
        [Fact]
        public void Gender_ShouldHaveCorrentValue() { 
            Assert.Equal(0, (int) Gender.Male);
            Assert.Equal(1, (int)Gender.Female);
        }

        [Fact]
        public void Gender_ShouldContainAllValue() { 
            var genders = Enum.GetValues(typeof(Gender)).Cast<Gender>().ToList();
            Assert.Contains(Gender.Male, genders);
            Assert.Contains(Gender.Female, genders);
        }
    }
}
