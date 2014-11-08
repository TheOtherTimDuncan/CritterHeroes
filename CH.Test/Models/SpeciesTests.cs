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
    public class SpeciesTests
    {
        [TestMethod]
        public void InstancesWithSameNameAreEqual()
        {
            Species species1 = new Species("1", "singular-1", "plural-1", "young-singular-1", "young-plural-1");
            Species species2 = new Species("1", "singular-1", "plural-2", "young-singular-2", "young-plural-2");
            species1.Should().Be(species2);
        }

        [TestMethod]
        public void InstancesWithDifferentNameAreNotEqual()
        {
            Species species1 = new Species("1", "singular-1", "plural-1", "young-singular-1", "young-plural-1");
            Species species2 = new Species("2", "singular-2", "plural-2", "young-singular-2", "young-plural-2");
            species1.Should().NotBe(species2);
        }

        [TestMethod]
        public void ThrowsExceptionIfCreatedWithNullName()
        {
            Action action = () => new Species(null, "singular", "plural", "youngSingular", "youngPlural");
            action.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("name");
        }

        [TestMethod]
        public void ThrowsExceptionIfCreatedWithEmptyName()
        {
            Action action = () => new Species("", "singular", "plural", "youngSingular", "youngPlural");
            action.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("name");
        }

        [TestMethod]
        public void ThrowsExceptionIfCreatedWithNullSingular()
        {
            Action action = () => new Species("name", null, "plural", "youngSingular", "youngPlural");
            action.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("singular");
        }

        [TestMethod]
        public void ThrowsExceptionIfCreatedWithEmptySingular()
        {
            Action action = () => new Species("name", "", "plural", "youngSingular", "youngPlural");
            action.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("singular");
        }

        [TestMethod]
        public void ThrowsExceptionIfCreatedWithNullPlural()
        {
            Action action = () => new Species("name", "singular", null, "youngSingular", "youngPlural");
            action.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("plural");
        }

        [TestMethod]
        public void ThrowsExceptionIfCreatedWithEmptyPlural()
        {
            Action action = () => new Species("name", "singular", "", "youngSingular", "youngPlural");
            action.ShouldThrow<ArgumentNullException>().And.ParamName.Should().Be("plural");
        }
    }
}
