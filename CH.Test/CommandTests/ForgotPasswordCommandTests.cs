using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using CH.Domain.Contracts;
using CH.Domain.Contracts.Email;
using CH.Domain.Contracts.Identity;
using CH.Domain.Contracts.Logging;
using CH.Domain.Identity;
using CH.Domain.Models;
using CH.Domain.Models.Logging;
using CH.Domain.Services.Commands;
using CH.Website.Controllers;
using CH.Website.Models;
using CH.Website.Services.CommandHandlers;
using CH.Website.Services.Commands;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test.CommandTests
{
    [TestClass]
    public class ForgotPasswordCommandTests
    {
        [TestMethod]
        public async Task ReturnsFailedIfBothEmailAddressAndUsernameAreMissing()
        {
            ForgotPasswordCommandHandler handler = new ForgotPasswordCommandHandler(null, null, null);
            ForgotPasswordCommand model = new ForgotPasswordCommand();

            (await handler.ExecuteAsync(model)).Succeeded.Should().BeFalse("both email address and username are null");

            model.EmailAddress = "";
            model.Username = "";
            (await handler.ExecuteAsync(model)).Succeeded.Should().BeFalse("both email address and username are empty");

            model.EmailAddress = "  ";
            model.Username = "  ";
            (await handler.ExecuteAsync(model)).Succeeded.Should().BeFalse("both email address and username are only whitespace");
        }

        [TestMethod]
        public async Task ReturnsSuccessButDoesntSendEmailIfUserNotFound()
        {
            string email = "email@email.com";
            string username = "unit.test";

            ForgotPasswordCommand command = new ForgotPasswordCommand()
            {
                EmailAddress = email
            };


            Mock<IApplicationUserManager> mockUserManager = new Mock<IApplicationUserManager>();
            mockUserManager.Setup(x => x.FindByNameAsync(username)).Returns(Task.FromResult((IdentityUser)null));

            Mock<IEmailClient> mockEmailClient = new Mock<IEmailClient>();
            mockEmailClient.Setup(x => x.SendAsync(It.IsAny<EmailMessage>()));

            Mock<IUserLogger> mockLogger = new Mock<IUserLogger>();
            mockLogger.Setup(x => x.LogActionAsync(UserActions.ForgotPasswordFailure, null, command)).Returns(Task.FromResult(0));

            Mock<IUrlGenerator> mockUrlGenerator = new Mock<IUrlGenerator>();
            mockUrlGenerator.Setup(x => x.GenerateSiteUrl<AccountController>(It.IsAny<Expression<Func<AccountController, ActionResult>>>())).Returns("account-url");
            command.UrlGenerator = mockUrlGenerator.Object;

            ForgotPasswordCommandHandler handler = new ForgotPasswordCommandHandler(mockUserManager.Object, mockEmailClient.Object, mockLogger.Object);
            ModalDialogCommandResult result = await handler.ExecuteAsync(command);
            result.Succeeded.Should().BeTrue();
            result.ModalDialog.Should().NotBeNull();
            result.ModalDialog.Text.Should().NotBeNullOrEmpty();
            result.ModalDialog.Buttons.Should().NotBeEmpty();
            result.ModalDialog.Buttons.Any(x => x.Url != null && x.Url.Contains("account-url")).Should().BeTrue();

            mockUserManager.Verify(x => x.FindByEmailAsync(email), Times.Once);
            mockEmailClient.Verify(x => x.SendAsync(It.IsAny<EmailMessage>()), Times.Never);
            mockLogger.Verify(x => x.LogActionAsync(UserActions.ForgotPasswordFailure, null, command), Times.Once);
            mockUrlGenerator.Verify(x => x.GenerateSiteUrl<AccountController>(It.IsAny<Expression<Func<AccountController, ActionResult>>>()), Times.Once);
        }

        [TestMethod]
        public async Task ResetsPasswordForValidUser()
        {
            ForgotPasswordCommand command = new ForgotPasswordCommand()
            {
                EmailAddress = "email@email.com",
                OrganizationFullName = "Full Name",
                OrganizationEmailAddress = "org@org.com"
            };

            IdentityUser user = new IdentityUser("unit.test")
            {
                Email = command.EmailAddress
            };

            string code = "code";
            string url = "http://www.password.reset.com/code";

            Mock<IApplicationUserManager> mockUserManager = new Mock<IApplicationUserManager>();
            mockUserManager.Setup(x => x.FindByEmailAsync(command.EmailAddress)).Returns(Task.FromResult(user));
            mockUserManager.Setup(x => x.GeneratePasswordResetTokenAsync(user.Id)).Returns(Task.FromResult(url));

            Mock<IUserLogger> mockLogger = new Mock<IUserLogger>();
            mockLogger.Setup(x => x.LogActionAsync(UserActions.ForgotPasswordSuccess, user.UserName, command)).Returns(Task.FromResult(0));

            Mock<IEmailClient> mockEmailClient = new Mock<IEmailClient>();
            mockEmailClient.Setup(x => x.SendAsync(It.IsAny<EmailMessage>(), user.Id)).Returns<EmailMessage, string>((message, forUserID) =>
            {
                forUserID.Should().Be(user.Id);

                message.To.Should().HaveCount(1);
                message.To.First().Should().Be(user.Email);

                message.From.Should().Be(command.OrganizationEmailAddress);
                message.Subject.Should().Be("Password Reset - " + command.OrganizationFullName);
                message.HtmlBody.Should().Contain(url);
                message.TextBody.Should().Contain(url);

                return Task.FromResult(0);
            });

            Mock<IUrlGenerator> mockUrlGenerator = new Mock<IUrlGenerator>();
            command.UrlGenerator = mockUrlGenerator.Object;
            mockUrlGenerator.Setup(x => x.GenerateAbsoluteUrl<AccountController>(It.IsAny<Expression<Func<AccountController, ActionResult>>>())).Returns(code);
            mockUrlGenerator.Setup(x => x.GenerateSiteUrl<AccountController>(It.IsAny<Expression<Func<AccountController, ActionResult>>>())).Returns("account-url");

            ForgotPasswordCommandHandler handler = new ForgotPasswordCommandHandler(mockUserManager.Object, mockEmailClient.Object, mockLogger.Object);
            ModalDialogCommandResult result = await handler.ExecuteAsync(command);
            result.Succeeded.Should().BeTrue();
            result.ModalDialog.Should().NotBeNull();
            result.ModalDialog.Text.Should().NotBeNullOrEmpty();
            result.ModalDialog.Buttons.Should().NotBeEmpty();
            result.ModalDialog.Buttons.Any(x => x.Url != null && x.Url.Contains("account-url")).Should().BeTrue();

            mockUserManager.Verify(x => x.FindByEmailAsync(command.EmailAddress), Times.Once);
            mockUserManager.Verify(x => x.GeneratePasswordResetTokenAsync(user.Id), Times.Once);
            mockEmailClient.Verify(x => x.SendAsync(It.IsAny<EmailMessage>(), user.Id), Times.Once);
            mockLogger.Verify(x => x.LogActionAsync(UserActions.ForgotPasswordSuccess, user.UserName, command), Times.Once);
            mockUrlGenerator.Verify(x => x.GenerateAbsoluteUrl<AccountController>(It.IsAny<Expression<Func<AccountController, ActionResult>>>()), Times.Once);
            mockUrlGenerator.Verify(x => x.GenerateSiteUrl<AccountController>(It.IsAny<Expression<Func<AccountController, ActionResult>>>()), Times.Once);
        }
    }
}
