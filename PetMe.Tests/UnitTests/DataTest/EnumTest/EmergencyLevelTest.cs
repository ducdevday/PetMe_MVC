using PetMe.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetMe.Tests.UnitTests.DataTest.EnumTest
{
    public class EmergencyLevelTest
    {
        [Fact]
        void EmergencyLevel_ShouldHaveCorrentValue() {
            Assert.Equal(0,(int) EmergencyLevel.Low);
            Assert.Equal(1, (int) EmergencyLevel.Medium);
            Assert.Equal(2, (int)EmergencyLevel.High);
        }

        [Fact]
        void EmergencyLevel_ShouldContainAllValue() { 
            var levels = Enum.GetValues(typeof(EmergencyLevel)).Cast<EmergencyLevel>().ToList();
            Assert.Contains(EmergencyLevel.Low, levels);
            Assert.Contains(EmergencyLevel.Low, levels);
            Assert.Contains(EmergencyLevel.Low, levels);
        }
    }
}
