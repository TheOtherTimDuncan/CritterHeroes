using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CH.Domain.Models.Data;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CH.Test.Models
{
    [TestClass]
    public class BreedTests
    {
        [TestMethod]
        public void InstancesWithSameIDAreEqual()
        {
            Breed animalStatus1 = new Breed("1", "species", "breed 1");
            Breed animalStatus2 = new Breed("1", "species", "breed 2");
            animalStatus1.Should().Be(animalStatus2);
        }

        [TestMethod]
        public void InstancesWithDifferentIDAreNotEqual()
        {
            Breed animalStatus1 = new Breed("1", "species", "breed 1");
            Breed animalStatus2 = new Breed("2", "species", "breed 2");
            animalStatus1.Should().NotBe(animalStatus2);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ThrowsExceptionIfCreatedWithInvalidName()
        {
            Breed test = new Breed(null, null, null);
        }
    }
}
