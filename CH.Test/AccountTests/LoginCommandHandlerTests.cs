using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Events;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Features.Account.Commands;
using CritterHeroes.Web.Features.Account.Models;
using CritterHeroes.Web.Models.LogEvents;
using CritterHeroes.Web.Shared.Commands;
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

            Mock<IAppEventPublisher> mockPublisher = new Mock<IAppEventPublisher>();
            mockPublisher.Setup(x => x.Publish(It.IsAny<UserLogEvent>())).Callback((UserLogEvent logEvent) =>
            {
                logEvent.MessageValues.Should().Contain(model.Email);
            });

            Mock<IAppSignInManager> mockSignInManager = new Mock<IAppSignInManager>();
            mockSignInManager.Setup(x => x.PasswordSignInAsync(model.Email, model.Password)).Returns(Task.FromResult(SignInStatus.Success));

            LoginCommandHandler command = new LoginCommandHandler(mockPublisher.Object, mockSignInManager.Object);
            CommandResult result = await command.ExecuteAsync(model);
            result.Succeeded.Should().BeTrue();
            result.Errors.Should().BeEmpty();

            mockSignInManager.Verify(x => x.PasswordSignInAsync(model.Email, model.Password), Times.Once);
            mockPublisher.Verify(x => x.Publish(It.IsAny<UserLogEvent>()), Times.Once);
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

            Mock<IAppEventPublisher> mockPublisher = new Mock<IAppEventPublisher>();
            mockPublisher.Setup(x => x.Publish(It.IsAny<UserLogEvent>())).Callback((UserLogEvent logEvent) =>
            {
                logEvent.MessageValues.Should().Contain(model.Email);
            });

            Mock<IAppSignInManager> mockSignInManager = new Mock<IAppSignInManager>();
            mockSignInManager.Setup(x => x.PasswordSignInAsync(model.Email, model.Password)).Returns(Task.FromResult(signInStatus));

            LoginCommandHandler command = new LoginCommandHandler(mockPublisher.Object, mockSignInManager.Object);
            CommandResult result = await command.ExecuteAsync(model);
            result.Succeeded.Should().BeFalse();
            result.Errors.Should().HaveCount(1);
            result.Errors.Single().Should().Be("The username or password that you entered was incorrect. Please try again.");

            mockSignInManager.Verify(x => x.PasswordSignInAsync(model.Email, model.Password), Times.Once);
            mockPublisher.Verify(x => x.Publish(It.IsAny<UserLogEvent>()), Times.Once);
        }
    }
}
