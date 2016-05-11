using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Test.Mocks;
using CritterHeroes.Web.Contracts.Email;
using CritterHeroes.Web.Contracts.Events;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Data.Models.Identity;
using CritterHeroes.Web.Features.Account;
using CritterHeroes.Web.Features.Account.Commands;
using CritterHeroes.Web.Features.Account.Models;
using CritterHeroes.Web.Models.LogEvents;
using CritterHeroes.Web.Shared.Commands;
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

            Mock<IAppEventPublisher> mockPublisher = new Mock<IAppEventPublisher>();
            mockPublisher.Setup(x => x.Publish(It.IsAny<UserLogEvent>())).Callback((UserLogEvent logEvent) =>
            {
                logEvent.MessageValues.Should().Contain(command.ResetPasswordEmail);
            });

            Mock<IAppUserManager> mockUserManager = new Mock<IAppUserManager>();

            MockUrlGenerator mockUrlGenerator = new MockUrlGenerator();

            Mock<IEmailService> mockEmailService = new Mock<IEmailService>();

            ForgotPasswordCommandHandler handler = new ForgotPasswordCommandHandler(mockPublisher.Object, mockUserManager.Object, mockEmailService.Object, mockUrlGenerator.Object);
            CommandResult result = await handler.ExecuteAsync(command);
            result.Succeeded.Should().BeTrue();

            mockPublisher.Verify(x => x.Publish(It.IsAny<UserLogEvent>()), Times.Once);
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

            Mock<IAppEventPublisher> mockPublisher = new Mock<IAppEventPublisher>();
            mockPublisher.Setup(x => x.Publish(It.IsAny<UserLogEvent>())).Callback((UserLogEvent logEvent) =>
            {
                logEvent.MessageValues.Should().Contain(user.Email);
            });

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

            ForgotPasswordCommandHandler handler = new ForgotPasswordCommandHandler(mockPublisher.Object, mockUserManager.Object, mockEmailService.Object, mockUrlGenerator.Object);
            CommandResult result = await handler.ExecuteAsync(command);
            result.Succeeded.Should().BeTrue();

            mockPublisher.Verify(x => x.Publish(It.IsAny<UserLogEvent>()), Times.Once);
            mockUserManager.Verify(x => x.FindByEmailAsync(command.ResetPasswordEmail), Times.Once);
            mockUserManager.Verify(x => x.GeneratePasswordResetTokenAsync(user.Id), Times.Once);
            mockEmailService.Verify(x => x.SendEmailAsync(It.IsAny<ResetPasswordEmailCommand>()), Times.Once);
            mockEmailService.Verify(x => x.SendEmailAsync(It.IsAny<ResetPasswordAttemptEmailCommand>()), Times.Never);
        }
    }
}
