using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Test.Mocks;
using CritterHeroes.Web.Areas.Account;
using CritterHeroes.Web.Areas.Account.CommandHandlers;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Areas.Common.ActionExtensions;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Contracts.Email;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Data.Models.Identity;
using CritterHeroes.Web.Models.Logging;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test.AccountTests
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
            Mock<IAppUserManager> mockUserManager = new Mock<IAppUserManager>();

            MockUrlGenerator mockUrlGenerator = new MockUrlGenerator();

            Mock<IEmailService> mockEmailService = new Mock<IEmailService>();

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

            AppUser user = new AppUser("unit.test")
            {
                Email = command.ResetPasswordEmail
            };

            string code = "code";

            Mock<IUserLogger> mockUserLogger = new Mock<IUserLogger>();

            Mock<IAppUserManager> mockUserManager = new Mock<IAppUserManager>();
            mockUserManager.Setup(x => x.FindByEmailAsync(command.ResetPasswordEmail)).Returns(Task.FromResult(user));
            mockUserManager.Setup(x => x.GeneratePasswordResetTokenAsync(user.Id)).Returns(Task.FromResult(code));

            MockUrlGenerator mockUrlGenerator = new MockUrlGenerator();

            Mock<IEmailService> mockEmailService = new Mock<IEmailService>();
            mockEmailService.Setup(x => x.SendEmailAsync(It.IsAny<ResetPasswordEmailCommand>())).Returns((ResetPasswordEmailCommand emailCommand) =>
            {
                emailCommand.EmailData.Token.Should().Be(code);

                emailCommand.EmailData.UrlReset.Should().Be(mockUrlGenerator.UrlHelper.AbsoluteAction(nameof(AccountController.ResetPassword), AccountController.Route, new
                {
                    code = code
                }));

                return Task.FromResult(CommandResult.Success());
            });

            ForgotPasswordCommandHandler handler = new ForgotPasswordCommandHandler(mockUserLogger.Object, mockUserManager.Object, mockEmailService.Object, mockUrlGenerator.Object);
            CommandResult result = await handler.ExecuteAsync(command);
            result.Succeeded.Should().BeTrue();

            mockUserLogger.Verify(x => x.LogActionAsync(UserActions.ForgotPasswordSuccess, user.Email), Times.Once);
            mockUserManager.Verify(x => x.FindByEmailAsync(command.ResetPasswordEmail), Times.Once);
            mockUserManager.Verify(x => x.GeneratePasswordResetTokenAsync(user.Id), Times.Once);
            mockEmailService.Verify(x => x.SendEmailAsync(It.IsAny<ResetPasswordEmailCommand>()), Times.Once);
            mockEmailService.Verify(x => x.SendEmailAsync(It.IsAny<ResetPasswordAttemptEmailCommand>()), Times.Never);
        }
    }
}
