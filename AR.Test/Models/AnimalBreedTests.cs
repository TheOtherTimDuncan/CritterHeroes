using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AR.Domain.Models.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace AR.Test.Models
{
    [TestClass]
    public class AnimalBreedTests
    {
        [TestMethod]
        public void InstancesWithSameIDAreEqual()
        {
            AnimalBreed animalStatus1 = new AnimalBreed("1", "species", "breed 1");
            AnimalBreed animalStatus2 = new AnimalBreed("1", "species", "breed 2");
            animalStatus1.Should().Be(animalStatus2);
        }

        [TestMethod]
        public void InstancesWithDifferentIDAreNotEqual()
        {
            AnimalBreed animalStatus1 = new AnimalBreed("1", "species", "breed 1");
            AnimalBreed animalStatus2 = new AnimalBreed("2", "species", "breed 2");
            animalStatus1.Should().NotBe(animalStatus2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionIfCreatedWithInvalidName()
        {
            AnimalBreed test = new AnimalBreed(null, null, null);
        }
    }
}
