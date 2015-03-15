using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.Commands;
using FluentAssertions;
using Microsoft.AspNet.Identity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CH.Test.ServicesTests
{
    [TestClass]
    public class CommandResultTests
    {
        [TestMethod]
        public void SuccessfulResultHasNoErrors()
        {
            CommandResult result = CommandResult.Success();
            result.Succeeded.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [TestMethod]
        public void FailedResultHasErrors()
        {
            CommandResult result = CommandResult.Failed("message");
            result.Succeeded.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            result.Errors.Single().Should().Be("message");
        }

        [TestMethod]
        public void FailedResultHasMultipleErrors()
        {
            string[] errors = new string[] { "Error1", "Error2" };
            CommandResult result = CommandResult.Failed(errors);
            result.Succeeded.Should().BeFalse();
            result.Errors.Should().HaveCount(2);
            result.Errors.Should().Equal(errors);
        }

        [TestMethod]
        public void ReturnsSuccessForSuccessfulIdentityResult()
        {
            CommandResult result = CommandResult.FromIdentityResult(IdentityResult.Success);
            result.Succeeded.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [TestMethod]
        public void ReturnsFailedResultForFailedIdentityResult()
        {
            CommandResult result = CommandResult.FromIdentityResult(new IdentityResult("error"));
            result.Succeeded.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            result.Errors.Single().Should().Be("error");
        }
    }
}
