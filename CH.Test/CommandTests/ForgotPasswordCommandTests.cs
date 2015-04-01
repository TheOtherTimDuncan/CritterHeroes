using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Account;
using CritterHeroes.Web.Areas.Account.CommandHandlers;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.Identity;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Email;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Models;
using CritterHeroes.Web.Models.Logging;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test.CommandTests
{
    [TestClass]
    public class ForgotPasswordCommandTests
    {
        [TestMethod]
        public async Task ReturnsSuccessAndSendsAttemptEmailIfUserNotFound()
        {
            string email = "email@email.com";

            ForgotPasswordModel command = new ForgotPasswordModel()
            {
                ResetPasswordEmail = email
            };

            Mock<IUserLogger> mockUserLogger = new Mock<IUserLogger>();
            Mock<IApplicationUserManager> mockUserManager = new Mock<IApplicationUserManager>();
            Mock<IEmailService> mockEmailService = new Mock<IEmailService>();
            Mock<IUrlGenerator> mockUrlGenerator = new Mock<IUrlGenerator>();

            ForgotPasswordCommandHandler handler = new ForgotPasswordCommandHandler(mockUserLogger.Object, mockUserManager.Object, mockEmailService.Object, mockUrlGenerator.Object);
            CommandResult result = await handler.ExecuteAsync(command);
            result.Succeeded.Should().BeTrue();

            mockUserLogger.Verify(x => x.LogActionAsync(UserActions.ForgotPasswordFailure, command.ResetPasswordEmail), Times.Once);
            mockUserManager.Verify(x => x.FindByEmailAsync(email), Times.Once);
            mockEmailService.Verify(x => x.SendEmailAsync(It.IsAny<ResetPasswordEmailCommand>()), Times.Never);
            mockEmailService.Verify(x => x.SendEmailAsync(It.IsAny<ResetPasswordAttemptEmailCommand>()), Times.Once);
        }

        [TestMethod]
        public async Task ResetsPasswordForValidUser()
        {
            ForgotPasswordModel command = new ForgotPasswordModel()
            {
                ResetPasswordEmail = "email@email.com",
            };

            IdentityUser user = new IdentityUser("unit.test")
            {
                Email = command.ResetPasswordEmail
            };

            string code = "code";
            string url = "http://www.password.reset.com/code";

            Mock<IUserLogger> mockUserLogger = new Mock<IUserLogger>();

            Mock<IApplicationUserManager> mockUserManager = new Mock<IApplicationUserManager>();
            mockUserManager.Setup(x => x.FindByEmailAsync(command.ResetPasswordEmail)).Returns(Task.FromResult(user));
            mockUserManager.Setup(x => x.GeneratePasswordResetTokenAsync(user.Id)).Returns(Task.FromResult(code));

            Mock<IEmailService> mockEmailService = new Mock<IEmailService>();
            mockEmailService.Setup(x => x.SendEmailAsync(It.IsAny<ResetPasswordEmailCommand>())).Returns((ResetPasswordEmailCommand emailCommand) =>
            {
                emailCommand.Token.Should().Be(code);
                emailCommand.Url.Should().Be(url);

                return Task.FromResult(CommandResult.Success());
            });

            Mock<IUrlGenerator> mockUrlGenerator = new Mock<IUrlGenerator>();
            mockUrlGenerator.Setup(x => x.GenerateAbsoluteUrl<AccountController>(It.IsAny<Expression<Func<AccountController, ActionResult>>>())).Returns(url);

            ForgotPasswordCommandHandler handler = new ForgotPasswordCommandHandler(mockUserLogger.Object, mockUserManager.Object, mockEmailService.Object, mockUrlGenerator.Object);
            CommandResult result = await handler.ExecuteAsync(command);
            result.Succeeded.Should().BeTrue();

            mockUserLogger.Verify(x => x.LogActionAsync(UserActions.ForgotPasswordSuccess, user.Email), Times.Once);
            mockUserManager.Verify(x => x.FindByEmailAsync(command.ResetPasswordEmail), Times.Once);
            mockUserManager.Verify(x => x.GeneratePasswordResetTokenAsync(user.Id), Times.Once);
            mockEmailService.Verify(x => x.SendEmailAsync(It.IsAny<ResetPasswordEmailCommand>()), Times.Once);
            mockUrlGenerator.Verify(x => x.GenerateAbsoluteUrl<AccountController>(It.IsAny<Expression<Func<AccountController, ActionResult>>>()), Times.Once);
        }
    }
}
