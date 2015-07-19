using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Data.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CH.Test.Models
{
    [TestClass]
    public class AnimalStatusTests
    {
        [TestMethod]
        public void InstancesWithSameIDAreEqual()
        {
            AnimalStatus animalStatus1 = new AnimalStatus("1", "name", "description 1");
            AnimalStatus animalStatus2 = new AnimalStatus("1", "name", "description 2");
            animalStatus1.Should().Be(animalStatus2);
        }

        [TestMethod]
        public void InstancesWithDifferentIDAreNotEqual()
        {
            AnimalStatus animalStatus1 = new AnimalStatus("1", "name1", "description");
            AnimalStatus animalStatus2 = new AnimalStatus("2", "name2", "description");
            animalStatus1.Should().NotBe(animalStatus2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionIfCreatedWithInvalidName()
        {
            AnimalStatus test = new AnimalStatus(null, null, null);
        }
    }
}
