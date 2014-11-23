﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CH.Domain.Commands;
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
            CommandResult result = CommandResult.Failed("key", "message");
            result.Succeeded.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            result.Errors["key"].Should().Be("message");
        }

        [TestMethod]
        public void ReturnsSuccessForSuccessfulIdentityResult()
        {
            CommandResult result = CommandResult.FromIdentityResult(IdentityResult.Success, "key");
            result.Succeeded.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [TestMethod]
        public void ReturnsFailedResultForFailedIdentityResult()
        {
            CommandResult result = CommandResult.FromIdentityResult(new IdentityResult("error"), "key");
            result.Succeeded.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            result.Errors["key"].Should().Be("error");
        }
    }
}