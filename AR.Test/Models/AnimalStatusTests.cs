using System;
using AR.Domain.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AR.Test.Models
{
    [TestClass]
    public class AnimalStatusTests
    {
        [TestMethod]
        public void InstancesWithSameNameAreEqual()
        {
            AnimalStatus animalStatus1 = new AnimalStatus("name", "description 1");
            AnimalStatus animalStatus2 = new AnimalStatus("name", "description 2");
            Assert.IsTrue(animalStatus1 == animalStatus2);
        }

        [TestMethod]
        public void InstancesWithDifferentNameAreNotEqual()
        {
            AnimalStatus animalStatus1 = new AnimalStatus("name1", "description");
            AnimalStatus animalStatus2 = new AnimalStatus("name2", "description");
            Assert.IsTrue(animalStatus1 != animalStatus2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionIfCreatedWithInvalidName()
        {
            AnimalStatus test = new AnimalStatus(null, null);
        }
    }
}
