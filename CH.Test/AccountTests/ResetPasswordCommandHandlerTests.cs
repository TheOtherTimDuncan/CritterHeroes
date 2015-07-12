using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Account.CommandHandlers;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.Identity;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.Email;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Models;
using CritterHeroes.Web.Models.Logging;
using FluentAssertions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test.AccountTests
{
    [TestClass]
    public class ResetPasswordCommandHandlerTests
    {
        [TestMethod]
        public async Task ReturnsFailedIfUsernameIsInvalid()
        {
            ResetPasswordModel model = new ResetPasswordModel()
            {
                Email = "email@email.com",
                Code = "code"
            };

            Mock<IUserLogger> mockUserLogger = new Mock<IUserLogger>();

            Mock<IApplicationUserManager> mockUserManager = new Mock<IApplicationUserManager>();
            mockUserManager.Setup(x => x.FindByEmailAsync(model.Email)).Returns(Task.FromResult((IdentityUser)null));

            ResetPasswordCommandHandler handler = new ResetPasswordCommandHandler(mockUserLogger.Object, null, mockUserManager.Object, null, null);
            CommandResult result = await handler.ExecuteAsync(model);
            result.Succeeded.Should().BeFalse();
            result.Errors.Single().Should().Be("There was an error resetting your password. Please try again.");

            model.IsSuccess.Should().NotHaveValue();

            mockUserManager.Verify(x => x.FindByEmailAsync(model.Email), Times.Once);
            mockUserLogger.Verify(x => x.LogActionAsync(UserActions.ResetPasswordFailure, model.Email, It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task ReturnsFailedIfResetPasswordFails()
        {
            ResetPasswordModel model = new ResetPasswordModel()
            {
                Email = "email@email.com",
                Code = "code",
                Password = "password"
            };

            IdentityUser user = new IdentityUser(model.Email);

            Mock<IUserLogger> mockUserLogger = new Mock<IUserLogger>();

            Mock<IApplicationUserManager> mockUserManager = new Mock<IApplicationUserManager>();
            mockUserManager.Setup(x => x.FindByEmailAsync(model.Email)).Returns(Task.FromResult(user));
            mockUserManager.Setup(x => x.ResetPasswordAsync(user.Id, model.Code, model.Password)).Returns(Task.FromResult(IdentityResult.Failed("nope")));

            ResetPasswordCommandHandler handler = new ResetPasswordCommandHandler(mockUserLogger.Object, null, mockUserManager.Object, null, null);
            CommandResult result = await handler.ExecuteAsync(model);
            result.Succeeded.Should().BeFalse();
            result.Errors.Single().Should().Be("There was an error resetting your password. Please try again.");

            model.IsSuccess.Should().NotHaveValue();

            mockUserManager.Verify(x => x.FindByEmailAsync(model.Email), Times.Once);
            mockUserManager.Verify(x => x.ResetPasswordAsync(user.Id, model.Code, model.Password), Times.Once);
            mockUserLogger.Verify(x => x.LogActionAsync(UserActions.ResetPasswordFailure, user.Email, It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task ReturnsFailedIfLoginFails()
        {
            ResetPasswordModel model = new ResetPasswordModel()
            {
                Email = "email@email.com",
                Code = "code",
                Password = "password"
            };

            IdentityUser user = new IdentityUser(model.Email);

            Mock<IUserLogger> mockUserLogger = new Mock<IUserLogger>();

            Mock<IApplicationUserManager> mockUserManager = new Mock<IApplicationUserManager>();
            mockUserManager.Setup(x => x.FindByEmailAsync(model.Email)).Returns(Task.FromResult(user));
            mockUserManager.Setup(x => x.ResetPasswordAsync(user.Id, model.Code, model.Password)).Returns(Task.FromResult(IdentityResult.Success));

            Mock<IApplicationSignInManager> mockSigninManager = new Mock<IApplicationSignInManager>();
            mockSigninManager.Setup(x => x.PasswordSignInAsync(model.Email, model.Password)).Returns(Task.FromResult(SignInStatus.Failure));

            ResetPasswordCommandHandler handler = new ResetPasswordCommandHandler(mockUserLogger.Object, mockSigninManager.Object, mockUserManager.Object, null, null);
            CommandResult result = await handler.ExecuteAsync(model);
            result.Succeeded.Should().BeFalse();
            result.Errors.Single().Should().Be("There was an error resetting your password. Please try again.");

            model.IsSuccess.Should().NotHaveValue();

            mockUserManager.Verify(x => x.FindByEmailAsync(model.Email), Times.Once);
            mockUserManager.Verify(x => x.ResetPasswordAsync(user.Id, model.Code, model.Password), Times.Once);
            mockSigninManager.Verify(x => x.PasswordSignInAsync(model.Email, model.Password), Times.Once);
            mockUserLogger.Verify(x => x.LogActionAsync(UserActions.ResetPasswordFailure, user.Email, It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task ReturnsSuccessIfPasswordSuccessfullyResetAndLoginSucceeds()
        {
            ResetPasswordModel model = new ResetPasswordModel()
            {
                Email = "email@email.com",
                Code = "code",
                Password = "password"
            };

            OrganizationContext organizationContext = new OrganizationContext()
            {
                FullName = "FullName",
                EmailAddress = "org@org.com"
            };

            IdentityUser user = new IdentityUser(model.Email);

            EmailMessage emailMessage = null;

            Mock<IUserLogger> mockUserLogger = new Mock<IUserLogger>();

            Mock<IApplicationUserManager> mockUserManager = new Mock<IApplicationUserManager>();
            mockUserManager.Setup(x => x.FindByEmailAsync(model.Email)).Returns(Task.FromResult(user));
            mockUserManager.Setup(x => x.ResetPasswordAsync(user.Id, model.Code, model.Password)).Returns(Task.FromResult(IdentityResult.Success));

            Mock<IApplicationSignInManager> mockSigninManager = new Mock<IApplicationSignInManager>();
            mockSigninManager.Setup(x => x.PasswordSignInAsync(model.Email, model.Password)).Returns(Task.FromResult(SignInStatus.Success));

            Mock<IEmailClient> mockEmailClient = new Mock<IEmailClient>();
            mockEmailClient.Setup(x => x.SendAsync(It.IsAny<EmailMessage>())).Returns((EmailMessage msg) =>
            {
                emailMessage = msg;
                return Task.FromResult(0);
            });

            Mock<IStateManager<OrganizationContext>> mockOrganizationStateManager = new Mock<IStateManager<OrganizationContext>>();
            mockOrganizationStateManager.Setup(x => x.GetContext()).Returns(organizationContext);

            ResetPasswordCommandHandler handler = new ResetPasswordCommandHandler(mockUserLogger.Object, mockSigninManager.Object, mockUserManager.Object, mockEmailClient.Object, mockOrganizationStateManager.Object);
            CommandResult result = await handler.ExecuteAsync(model);
            result.Succeeded.Should().BeTrue();

            model.IsSuccess.Should().BeTrue();

            emailMessage.From.Should().Be(organizationContext.EmailAddress);
            emailMessage.To.Should().Contain(user.Email);
            emailMessage.Subject.Should().Contain(organizationContext.FullName);

            mockUserManager.Verify(x => x.FindByEmailAsync(model.Email), Times.Once);
            mockUserManager.Verify(x => x.ResetPasswordAsync(user.Id, model.Code, model.Password), Times.Once);
            mockSigninManager.Verify(x => x.PasswordSignInAsync(model.Email, model.Password), Times.Once);
            mockUserLogger.Verify(x => x.LogActionAsync(UserActions.ResetPasswordSuccess, user.Email), Times.Once);
            mockEmailClient.Verify(x => x.SendAsync(It.IsAny<EmailMessage>()), Times.Once);
        }
    }
}
