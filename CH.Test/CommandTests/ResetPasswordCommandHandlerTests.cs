using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Account.CommandHandlers;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Areas.Home;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.Identity;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Email;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Contracts.Notifications;
using CritterHeroes.Web.Models;
using CritterHeroes.Web.Models.Logging;
using FluentAssertions;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test.CommandTests
{
    [TestClass]
    public class ResetPasswordCommandHandlerTests
    {
        [TestMethod]
        public async Task ReturnsFailedIfUsernameIsInvalid()
        {
            ResetPasswordModel model = new ResetPasswordModel()
            {
                Username = "unit.test",
                Code = "code"
            };

            Mock<IUserLogger> mockUserLogger = new Mock<IUserLogger>();

            Mock<IApplicationUserManager> mockUserManager = new Mock<IApplicationUserManager>();
            mockUserManager.Setup(x => x.FindByNameAsync(model.Username)).Returns(Task.FromResult((IdentityUser)null));

            ResetPasswordCommandHandler handler = new ResetPasswordCommandHandler(mockUserLogger.Object, null, mockUserManager.Object, null, null, null);
            CommandResult result = await handler.ExecuteAsync(model);
            result.Succeeded.Should().BeFalse();
            result.Errors[""].First().Should().Be("There was an error resetting your password. Please try again.");

            mockUserManager.Verify(x => x.FindByNameAsync(model.Username), Times.Once);
            mockUserLogger.Verify(x => x.LogActionAsync(UserActions.ResetPasswordFailure, model.Username, It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task ReturnsFailedIfResetPasswordFails()
        {
            ResetPasswordModel model = new ResetPasswordModel()
            {
                Username = "unit.test",
                Code = "code",
                Password = "password"
            };

            IdentityUser user = new IdentityUser(model.Username);

            Mock<IUserLogger> mockUserLogger = new Mock<IUserLogger>();

            Mock<IApplicationUserManager> mockUserManager = new Mock<IApplicationUserManager>();
            mockUserManager.Setup(x => x.FindByNameAsync(model.Username)).Returns(Task.FromResult(user));
            mockUserManager.Setup(x => x.ResetPasswordAsync(user.Id, model.Code, model.Password)).Returns(Task.FromResult(IdentityResult.Failed("nope")));

            ResetPasswordCommandHandler handler = new ResetPasswordCommandHandler(mockUserLogger.Object, null, mockUserManager.Object, null, null, null);
            CommandResult result = await handler.ExecuteAsync(model);
            result.Succeeded.Should().BeFalse();
            result.Errors[""].First().Should().Be("There was an error resetting your password. Please try again.");

            mockUserManager.Verify(x => x.FindByNameAsync(model.Username), Times.Once);
            mockUserManager.Verify(x => x.ResetPasswordAsync(user.Id, model.Code, model.Password), Times.Once);
            mockUserLogger.Verify(x => x.LogActionAsync(UserActions.ResetPasswordFailure, user.UserName, It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task ReturnsFailedIfLoginFails()
        {
            ResetPasswordModel model = new ResetPasswordModel()
            {
                Username = "unit.test",
                Code = "code",
                Password = "password"
            };

            IdentityUser user = new IdentityUser(model.Username);

            Mock<IUserLogger> mockUserLogger = new Mock<IUserLogger>();

            Mock<IApplicationUserManager> mockUserManager = new Mock<IApplicationUserManager>();
            mockUserManager.Setup(x => x.FindByNameAsync(model.Username)).Returns(Task.FromResult(user));
            mockUserManager.Setup(x => x.ResetPasswordAsync(user.Id, model.Code, model.Password)).Returns(Task.FromResult(IdentityResult.Success));

            Mock<IApplicationSignInManager> mockSigninManager = new Mock<IApplicationSignInManager>();
            mockSigninManager.Setup(x => x.PasswordSignInAsync(model.Username, model.Password)).Returns(Task.FromResult(SignInStatus.Failure));

            ResetPasswordCommandHandler handler = new ResetPasswordCommandHandler(mockUserLogger.Object, mockSigninManager.Object, mockUserManager.Object, null, null, null);
            CommandResult result = await handler.ExecuteAsync(model);
            result.Succeeded.Should().BeFalse();
            result.Errors[""].First().Should().Be("There was an error resetting your password. Please try again.");

            mockUserManager.Verify(x => x.FindByNameAsync(model.Username), Times.Once);
            mockUserManager.Verify(x => x.ResetPasswordAsync(user.Id, model.Code, model.Password), Times.Once);
            mockSigninManager.Verify(x => x.PasswordSignInAsync(model.Username, model.Password), Times.Once);
            mockUserLogger.Verify(x => x.LogActionAsync(UserActions.ResetPasswordFailure, user.UserName, It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task ReturnsSuccessIfPasswordSuccessfullyResetAndLoginSucceeds()
        {
            ResetPasswordModel model = new ResetPasswordModel()
            {
                Username = "unit.test",
                Code = "code",
                Password = "password"
            };

            OrganizationContext organizationContext = new OrganizationContext()
            {
                FullName = "FullName",
                EmailAddress = "org@org.com"
            };

            IdentityUser user = new IdentityUser(model.Username)
            {
                Email = "email@email.com"
            };

            EmailMessage emailMessage = null;

            Mock<IUserLogger> mockUserLogger = new Mock<IUserLogger>();

            Mock<IApplicationUserManager> mockUserManager = new Mock<IApplicationUserManager>();
            mockUserManager.Setup(x => x.FindByNameAsync(model.Username)).Returns(Task.FromResult(user));
            mockUserManager.Setup(x => x.ResetPasswordAsync(user.Id, model.Code, model.Password)).Returns(Task.FromResult(IdentityResult.Success));

            Mock<IApplicationSignInManager> mockSigninManager = new Mock<IApplicationSignInManager>();
            mockSigninManager.Setup(x => x.PasswordSignInAsync(model.Username, model.Password)).Returns(Task.FromResult(SignInStatus.Success));

            Mock<IUrlGenerator> mockUrlGenerator = new Mock<IUrlGenerator>();
            mockUrlGenerator.Setup(x => x.GenerateSiteUrl<HomeController>(It.IsAny<Expression<Func<HomeController, ActionResult>>>())).Returns("account-url");

            Mock<IEmailClient> mockEmailClient = new Mock<IEmailClient>();
            mockEmailClient.Setup(x => x.SendAsync(It.IsAny<EmailMessage>())).Returns((EmailMessage msg) =>
            {
                emailMessage = msg;
                return Task.FromResult(0);
            });

            ResetPasswordCommandHandler handler = new ResetPasswordCommandHandler(mockUserLogger.Object, mockSigninManager.Object, mockUserManager.Object, mockUrlGenerator.Object, mockEmailClient.Object, organizationContext);
            CommandResult result = await handler.ExecuteAsync(model);
            result.Succeeded.Should().BeTrue();

            model.ModalDialog.Should().NotBeNull();
            model.ModalDialog.Text.Should().NotBeNullOrEmpty();
            model.ModalDialog.Buttons.Should().NotBeEmpty();
            model.ModalDialog.Buttons.Any(x => x.Url != null && x.Url.Contains("account-url")).Should().BeTrue();

            emailMessage.From.Should().Be(organizationContext.EmailAddress);
            emailMessage.To.Should().Contain(user.Email);
            emailMessage.Subject.Should().Contain(organizationContext.FullName);

            mockUserManager.Verify(x => x.FindByNameAsync(model.Username), Times.Once);
            mockUserManager.Verify(x => x.ResetPasswordAsync(user.Id, model.Code, model.Password), Times.Once);
            mockSigninManager.Verify(x => x.PasswordSignInAsync(model.Username, model.Password), Times.Once);
            mockUserLogger.Verify(x => x.LogActionAsync(UserActions.ResetPasswordSuccess, user.UserName), Times.Once);
            mockEmailClient.Verify(x => x.SendAsync(It.IsAny<EmailMessage>()), Times.Once);
        }
    }
}
