﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Account.CommandHandlers;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Contracts.Email;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Data.Models.Identity;
using CritterHeroes.Web.Models.LogEvents;
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

            Mock<IAppLogger> mockLogger = new Mock<IAppLogger>();
            mockLogger.Setup(x => x.LogEvent(It.IsAny<UserLogEvent>())).Callback((UserLogEvent logEvent) =>
            {
                logEvent.MessageValues.Should().Contain(model.Email);
                logEvent.MessageValues.Should().Contain(model.Code);
            });

            Mock<IAppUserManager> mockUserManager = new Mock<IAppUserManager>();
            mockUserManager.Setup(x => x.FindByEmailAsync(model.Email)).Returns(Task.FromResult((AppUser)null));


            ResetPasswordCommandHandler handler = new ResetPasswordCommandHandler(mockLogger.Object, null, mockUserManager.Object, null);
            CommandResult result = await handler.ExecuteAsync(model);
            result.Succeeded.Should().BeFalse();
            result.Errors.Single().Should().Be("There was an error resetting your password. Please try again.");

            model.IsSuccess.Should().NotHaveValue();

            mockUserManager.Verify(x => x.FindByEmailAsync(model.Email), Times.Once);
            mockLogger.Verify(x => x.LogEvent(It.IsAny<UserLogEvent>()), Times.Once);
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

            AppUser user = new AppUser(model.Email);

            IdentityResult identityResult = IdentityResult.Failed("nope");

            Mock<IAppLogger> mockLogger = new Mock<IAppLogger>();
            mockLogger.Setup(x => x.LogEvent(It.IsAny<UserLogEvent>())).Callback((UserLogEvent logEvent) =>
            {
                logEvent.MessageValues.Should().Contain(model.Email);
                logEvent.MessageValues.Should().Contain(model.Code);
                logEvent.MessageValues.Should().Contain(x => (x is IEnumerable<string>) && ((IEnumerable<string>)x == identityResult.Errors));
            });

            Mock<IAppUserManager> mockUserManager = new Mock<IAppUserManager>();
            mockUserManager.Setup(x => x.FindByEmailAsync(model.Email)).Returns(Task.FromResult(user));
            mockUserManager.Setup(x => x.ResetPasswordAsync(user.Id, model.Code, model.Password)).Returns(Task.FromResult(identityResult));

            ResetPasswordCommandHandler handler = new ResetPasswordCommandHandler(mockLogger.Object, null, mockUserManager.Object, null);
            CommandResult result = await handler.ExecuteAsync(model);
            result.Succeeded.Should().BeFalse();
            result.Errors.Single().Should().Be("There was an error resetting your password. Please try again.");

            model.IsSuccess.Should().NotHaveValue();

            mockUserManager.Verify(x => x.FindByEmailAsync(model.Email), Times.Once);
            mockUserManager.Verify(x => x.ResetPasswordAsync(user.Id, model.Code, model.Password), Times.Once);
            mockLogger.Verify(x => x.LogEvent(It.IsAny<UserLogEvent>()), Times.Once);
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

            AppUser user = new AppUser(model.Email);

            Mock<IAppLogger> mockLogger = new Mock<IAppLogger>();
            mockLogger.Setup(x => x.LogEvent(It.IsAny<UserLogEvent>())).Callback((UserLogEvent logEvent) =>
            {
                logEvent.MessageValues.Should().Contain(model.Email);
            });

            Mock<IAppUserManager> mockUserManager = new Mock<IAppUserManager>();
            mockUserManager.Setup(x => x.FindByEmailAsync(model.Email)).Returns(Task.FromResult(user));
            mockUserManager.Setup(x => x.ResetPasswordAsync(user.Id, model.Code, model.Password)).Returns(Task.FromResult(IdentityResult.Success));

            Mock<IAppSignInManager> mockSigninManager = new Mock<IAppSignInManager>();
            mockSigninManager.Setup(x => x.PasswordSignInAsync(model.Email, model.Password)).Returns(Task.FromResult(SignInStatus.Failure));

            ResetPasswordCommandHandler handler = new ResetPasswordCommandHandler(mockLogger.Object, mockSigninManager.Object, mockUserManager.Object, null);
            CommandResult result = await handler.ExecuteAsync(model);
            result.Succeeded.Should().BeFalse();
            result.Errors.Single().Should().Be("There was an error resetting your password. Please try again.");

            model.IsSuccess.Should().NotHaveValue();

            mockUserManager.Verify(x => x.FindByEmailAsync(model.Email), Times.Once);
            mockUserManager.Verify(x => x.ResetPasswordAsync(user.Id, model.Code, model.Password), Times.Once);
            mockSigninManager.Verify(x => x.PasswordSignInAsync(model.Email, model.Password), Times.Once);
            mockLogger.Verify(x => x.LogEvent(It.IsAny<UserLogEvent>()), Times.Once);
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

            AppUser user = new AppUser(model.Email);

            Mock<IAppLogger> mockLogger = new Mock<IAppLogger>();
            mockLogger.Setup(x => x.LogEvent(It.IsAny<UserLogEvent>())).Callback((UserLogEvent logEvent) =>
            {
                logEvent.MessageValues.Should().Contain(model.Email);
            });

            Mock<IAppUserManager> mockUserManager = new Mock<IAppUserManager>();
            mockUserManager.Setup(x => x.FindByEmailAsync(model.Email)).Returns(Task.FromResult(user));
            mockUserManager.Setup(x => x.ResetPasswordAsync(user.Id, model.Code, model.Password)).Returns(Task.FromResult(IdentityResult.Success));

            Mock<IAppSignInManager> mockSigninManager = new Mock<IAppSignInManager>();
            mockSigninManager.Setup(x => x.PasswordSignInAsync(model.Email, model.Password)).Returns(Task.FromResult(SignInStatus.Success));

            Mock<IEmailService> mockEmailService = new Mock<IEmailService>();

            ResetPasswordCommandHandler handler = new ResetPasswordCommandHandler(mockLogger.Object, mockSigninManager.Object, mockUserManager.Object, mockEmailService.Object);
            CommandResult result = await handler.ExecuteAsync(model);
            result.Succeeded.Should().BeTrue();

            model.IsSuccess.Should().BeTrue();

            mockUserManager.Verify(x => x.FindByEmailAsync(model.Email), Times.Once);
            mockUserManager.Verify(x => x.ResetPasswordAsync(user.Id, model.Code, model.Password), Times.Once);
            mockSigninManager.Verify(x => x.PasswordSignInAsync(model.Email, model.Password), Times.Once);
            mockLogger.Verify(x => x.LogEvent(It.IsAny<UserLogEvent>()), Times.Once);
            mockEmailService.Verify(x => x.SendEmailAsync(It.IsAny<ResetPasswordNotificationEmailCommand>()), Times.Once);
        }
    }
}
