﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CH.Domain.Commands;
using CH.Domain.Contracts.Identity;
using CH.Domain.Contracts.Logging;
using CH.Domain.Models.Logging;
using CH.Website;
using CH.Website.Models;
using CH.Website.Services.Commands;
using FluentAssertions;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test.ServicesTests
{
    [TestClass]
    public class CommandHandlerTests
    {
        [TestMethod]
        public async Task LoginCommandReturnsSuccessForSuccessfulLogin()
        {
            LoginModel model = new LoginModel()
            {
                Username = "username",
                Password = "password"
            };

            Mock<IApplicationSignInManager> mockSignInManager = new Mock<IApplicationSignInManager>();
            mockSignInManager.Setup(x => x.PasswordSignInAsync(model.Username, model.Password, false, false)).Returns(Task.FromResult(SignInStatus.Success));

            Mock<IUserLogger> mockUserLogger = new Mock<IUserLogger>();
            mockUserLogger.Setup(x => x.LogAction(UserActions.PasswordLoginSuccess, model.Username)).Returns(Task.FromResult(0));

            LoginCommand command = new LoginCommand(mockSignInManager.Object, mockUserLogger.Object);
            CommandResult result = await command.Execute(model);
            result.Succeeded.Should().BeTrue();
            result.Errors.Should().BeEmpty();

            mockSignInManager.Verify(x => x.PasswordSignInAsync(model.Username, model.Password, false, false), Times.Once);
            mockUserLogger.Verify(x => x.LogAction(UserActions.PasswordLoginSuccess, model.Username), Times.Once);
        }

        [TestMethod]
        public async Task LoginCommandReturnsFailureForFailedLogin()
        {
            LoginModel model = new LoginModel()
            {
                Username = "username",
                Password = "password"
            };

            Mock<IApplicationSignInManager> mockSignInManager = new Mock<IApplicationSignInManager>();
            mockSignInManager.Setup(x => x.PasswordSignInAsync(model.Username, model.Password, false, false)).Returns(Task.FromResult(SignInStatus.Failure));

            Mock<IUserLogger> mockUserLogger = new Mock<IUserLogger>();
            mockUserLogger.Setup(x => x.LogAction(UserActions.PasswordLoginFailure, model.Username)).Returns(Task.FromResult(0));

            LoginCommand command = new LoginCommand(mockSignInManager.Object, mockUserLogger.Object);
            CommandResult result = await command.Execute(model);
            result.Succeeded.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            result.Errors[""].Should().Be("The username or password that you entered was incorrect. Please try again.");

            mockSignInManager.Verify(x => x.PasswordSignInAsync(model.Username, model.Password, false, false), Times.Once);
            mockUserLogger.Verify(x => x.LogAction(UserActions.PasswordLoginFailure, model.Username), Times.Once);
        }
    }
}