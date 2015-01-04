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
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.Logging;
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
            mockUserLogger.Setup(x => x.LogActionAsync(UserActions.ResetPasswordFailure, model.Username, It.IsAny<string>())).Returns<UserActions, string, string>((userAction, username, additionalData) =>
            {
                additionalData.Should().Contain(model.Code);
                return Task.FromResult(0);
            });

            Mock<IApplicationUserManager> mockUserManager = new Mock<IApplicationUserManager>();
            mockUserManager.Setup(x => x.FindByNameAsync(model.Username)).Returns(Task.FromResult((IdentityUser)null));

            ResetPasswordCommandHandler handler = new ResetPasswordCommandHandler(null, mockUserLogger.Object, mockUserManager.Object, null);
            CommandResult result = await handler.ExecuteAsync(model);
            result.Succeeded.Should().BeFalse();
            result.Errors[""].First().Should().Be("There was an error resetting your password. Please try again.");

            mockUserLogger.Verify(x => x.LogActionAsync(UserActions.ResetPasswordFailure, model.Username, It.IsAny<string>()), Times.Once);
            mockUserManager.Verify(x => x.FindByNameAsync(model.Username), Times.Once);
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
            mockUserLogger.Setup(x => x.LogActionAsync(UserActions.ResetPasswordFailure, model.Username, It.IsAny<string>())).Returns<UserActions, string, string>((userAction, username, additionalData) =>
            {
                additionalData.Should().Contain(model.Code);
                return Task.FromResult(0);
            });

            Mock<IApplicationUserManager> mockUserManager = new Mock<IApplicationUserManager>();
            mockUserManager.Setup(x => x.FindByNameAsync(model.Username)).Returns(Task.FromResult(user));
            mockUserManager.Setup(x => x.ResetPasswordAsync(user.Id, model.Code, model.Password)).Returns(Task.FromResult(IdentityResult.Failed("nope")));

            ResetPasswordCommandHandler handler = new ResetPasswordCommandHandler(null, mockUserLogger.Object, mockUserManager.Object, null);
            CommandResult result = await handler.ExecuteAsync(model);
            result.Succeeded.Should().BeFalse();
            result.Errors[""].First().Should().Be("There was an error resetting your password. Please try again.");

            mockUserLogger.Verify(x => x.LogActionAsync(UserActions.ResetPasswordFailure, model.Username, It.IsAny<string>()), Times.Once);
            mockUserManager.Verify(x => x.FindByNameAsync(model.Username), Times.Once);
            mockUserManager.Verify(x => x.ResetPasswordAsync(user.Id, model.Code, model.Password), Times.Once);
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
            mockUserLogger.Setup(x => x.LogActionAsync(UserActions.ResetPasswordFailure, model.Username, It.IsAny<string>())).Returns<UserActions, string, string>((userAction, username, additionalData) =>
            {
                additionalData.Should().Contain(model.Code);
                return Task.FromResult(0);
            });

            Mock<IApplicationUserManager> mockUserManager = new Mock<IApplicationUserManager>();
            mockUserManager.Setup(x => x.FindByNameAsync(model.Username)).Returns(Task.FromResult(user));
            mockUserManager.Setup(x => x.ResetPasswordAsync(user.Id, model.Code, model.Password)).Returns(Task.FromResult(IdentityResult.Success));

            Mock<IApplicationSignInManager> mockSigninManager = new Mock<IApplicationSignInManager>();
            mockSigninManager.Setup(x => x.PasswordSignInAsync(model.Username, model.Password, false, false)).Returns(Task.FromResult(SignInStatus.Failure));

            ResetPasswordCommandHandler handler = new ResetPasswordCommandHandler(mockSigninManager.Object, mockUserLogger.Object, mockUserManager.Object, null);
            CommandResult result = await handler.ExecuteAsync(model);
            result.Succeeded.Should().BeFalse();
            result.Errors[""].First().Should().Be("There was an error resetting your password. Please try again.");

            mockUserLogger.Verify(x => x.LogActionAsync(UserActions.ResetPasswordFailure, model.Username, It.IsAny<string>()), Times.Once);
            mockUserManager.Verify(x => x.FindByNameAsync(model.Username), Times.Once);
            mockUserManager.Verify(x => x.ResetPasswordAsync(user.Id, model.Code, model.Password), Times.Once);
            mockSigninManager.Verify(x => x.PasswordSignInAsync(model.Username, model.Password, false, false), Times.Once);
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

            IdentityUser user = new IdentityUser(model.Username);

            Mock<IUserLogger> mockUserLogger = new Mock<IUserLogger>();
            mockUserLogger.Setup(x => x.LogActionAsync(UserActions.ResetPasswordFailure, model.Username, It.IsAny<string>())).Returns<UserActions, string, string>((userAction, username, additionalData) =>
            {
                additionalData.Should().Contain(model.Code);
                return Task.FromResult(0);
            });

            Mock<IApplicationUserManager> mockUserManager = new Mock<IApplicationUserManager>();
            mockUserManager.Setup(x => x.FindByNameAsync(model.Username)).Returns(Task.FromResult(user));
            mockUserManager.Setup(x => x.ResetPasswordAsync(user.Id, model.Code, model.Password)).Returns(Task.FromResult(IdentityResult.Success));

            Mock<IApplicationSignInManager> mockSigninManager = new Mock<IApplicationSignInManager>();
            mockSigninManager.Setup(x => x.PasswordSignInAsync(model.Username, model.Password, false, false)).Returns(Task.FromResult(SignInStatus.Success));

            Mock<IUrlGenerator> mockUrlGenerator = new Mock<IUrlGenerator>();
            mockUrlGenerator.Setup(x => x.GenerateSiteUrl<HomeController>(It.IsAny<Expression<Func<HomeController, ActionResult>>>())).Returns("account-url");

            ResetPasswordCommandHandler handler = new ResetPasswordCommandHandler(mockSigninManager.Object, mockUserLogger.Object, mockUserManager.Object, mockUrlGenerator.Object);
            CommandResult result = await handler.ExecuteAsync(model);
            result.Succeeded.Should().BeTrue();

            model.ModalDialog.Should().NotBeNull();
            model.ModalDialog.Text.Should().NotBeNullOrEmpty();
            model.ModalDialog.Buttons.Should().NotBeEmpty();
            model.ModalDialog.Buttons.Any(x => x.Url != null && x.Url.Contains("account-url")).Should().BeTrue();

            mockUserLogger.Verify(x => x.LogActionAsync(UserActions.ResetPasswordSuccess, model.Username), Times.Once);
            mockUserManager.Verify(x => x.FindByNameAsync(model.Username), Times.Once);
            mockUserManager.Verify(x => x.ResetPasswordAsync(user.Id, model.Code, model.Password), Times.Once);
            mockSigninManager.Verify(x => x.PasswordSignInAsync(model.Username, model.Password, false, false), Times.Once);
        }
    }
}
