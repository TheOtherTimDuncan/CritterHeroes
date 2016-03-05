using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Account.CommandHandlers;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Models.LogEvents;
using FluentAssertions;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test.AccountTests
{
    [TestClass]
    public class LoginCommandHandlerTests
    {
        [TestMethod]
        public async Task LoginCommandReturnsSuccessForSuccessfulLogin()
        {
            LoginModel model = new LoginModel()
            {
                Email = "username",
                Password = "password"
            };

            Mock<IAppLogger> mockLogger = new Mock<IAppLogger>();
            mockLogger.Setup(x => x.LogEvent(It.IsAny<UserLogEvent>())).Callback((UserLogEvent logEvent) =>
            {
                logEvent.MessageValues.Should().Contain(model.Email);
            });

            Mock<IAppSignInManager> mockSignInManager = new Mock<IAppSignInManager>();
            mockSignInManager.Setup(x => x.PasswordSignInAsync(model.Email, model.Password)).Returns(Task.FromResult(SignInStatus.Success));

            LoginCommandHandler command = new LoginCommandHandler(mockLogger.Object, mockSignInManager.Object);
            CommandResult result = await command.ExecuteAsync(model);
            result.Succeeded.Should().BeTrue();
            result.Errors.Should().BeEmpty();

            mockSignInManager.Verify(x => x.PasswordSignInAsync(model.Email, model.Password), Times.Once);
            mockLogger.Verify(x => x.LogEvent(It.IsAny<UserLogEvent>()), Times.Once);
        }

        [TestMethod]
        public async Task LoginCommandReturnsFailureForFailedLogin()
        {
            LoginModel model = new LoginModel()
            {
                Email = "username",
                Password = "password"
            };

            SignInStatus signInStatus = SignInStatus.Failure;

            Mock<IAppLogger> mockLogger = new Mock<IAppLogger>();
            mockLogger.Setup(x => x.LogEvent(It.IsAny<UserLogEvent>())).Callback((UserLogEvent logEvent) =>
            {
                logEvent.MessageValues.Should().Contain(model.Email);
            });

            Mock<IAppSignInManager> mockSignInManager = new Mock<IAppSignInManager>();
            mockSignInManager.Setup(x => x.PasswordSignInAsync(model.Email, model.Password)).Returns(Task.FromResult(signInStatus));

            LoginCommandHandler command = new LoginCommandHandler(mockLogger.Object, mockSignInManager.Object);
            CommandResult result = await command.ExecuteAsync(model);
            result.Succeeded.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            result.Errors.Single().Should().Be("The username or password that you entered was incorrect. Please try again.");

            mockSignInManager.Verify(x => x.PasswordSignInAsync(model.Email, model.Password), Times.Once);
            mockLogger.Verify(x => x.LogEvent(It.IsAny<UserLogEvent>()), Times.Once);
        }
    }
}
