using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Account;
using CritterHeroes.Web.Areas.Account.CommandHandlers;
using CritterHeroes.Web.Areas.Account.Commands;
using CritterHeroes.Web.Areas.Account.Models;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.Identity;
using CritterHeroes.Web.Common.Notifications;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Email;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.Notifications;
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
        public async Task ReturnsFailedIfBothEmailAddressAndUsernameAreMissing()
        {
            ForgotPasswordCommandHandler handler = new ForgotPasswordCommandHandler(null, null, null, null);
            ForgotPasswordCommand command = new ForgotPasswordCommand(new ForgotPasswordModel(), new OrganizationContext());

            (await handler.ExecuteAsync(command)).Succeeded.Should().BeFalse("both email address and username are null");

            command.Model.EmailAddress = "";
            command.Model.Username = "";
            (await handler.ExecuteAsync(command)).Succeeded.Should().BeFalse("both email address and username are empty");

            command.Model.EmailAddress = "  ";
            command.Model.Username = "  ";
            (await handler.ExecuteAsync(command)).Succeeded.Should().BeFalse("both email address and username are only whitespace");
        }

        [TestMethod]
        public async Task ReturnsSuccessButDoesntSendEmailIfUserNotFound()
        {
            string email = "email@email.com";
            string username = "unit.test";

            ForgotPasswordModel model = new ForgotPasswordModel()
            {
                EmailAddress = email
            };

            OrganizationContext organizationContext = new OrganizationContext()
            {
                OrganizationID = Guid.NewGuid(),
                FullName = "Full Name",
                EmailAddress = "org@org.com"
            };

            ForgotPasswordCommand command = new ForgotPasswordCommand(model, organizationContext);

            Mock<INotificationPublisher> mockNotificationPublisher = new Mock<INotificationPublisher>();
            mockNotificationPublisher.Setup(x => x.PublishAsync(It.IsAny<UserActionNotification>())).Returns<UserActionNotification>((notification) =>
            {
                notification.Action.Should().Be(UserActions.ForgotPasswordFailure);
                notification.Username.Should().BeNull();
                notification.AdditionalData.Should().Be(model);
                return Task.FromResult(0);
            });

            Mock<IApplicationUserManager> mockUserManager = new Mock<IApplicationUserManager>();
            mockUserManager.Setup(x => x.FindByNameAsync(username)).Returns(Task.FromResult((IdentityUser)null));

            Mock<IEmailClient> mockEmailClient = new Mock<IEmailClient>();

            Mock<IUrlGenerator> mockUrlGenerator = new Mock<IUrlGenerator>();
            mockUrlGenerator.Setup(x => x.GenerateSiteUrl<AccountController>(It.IsAny<Expression<Func<AccountController, ActionResult>>>())).Returns("account-url");

            ForgotPasswordCommandHandler handler = new ForgotPasswordCommandHandler(mockNotificationPublisher.Object, mockUserManager.Object, mockEmailClient.Object, mockUrlGenerator.Object);
            CommandResult result = await handler.ExecuteAsync(command);
            result.Succeeded.Should().BeFalse();
            result.Errors.Should().BeEmpty();

            command.ModalDialog.Should().NotBeNull();
            command.ModalDialog.Text.Should().NotBeNullOrEmpty();
            command.ModalDialog.Buttons.Should().NotBeEmpty();
            command.ModalDialog.Buttons.Any(x => x.Url != null && x.Url.Contains("account-url")).Should().BeTrue();

            mockNotificationPublisher.Verify(x => x.PublishAsync(It.IsAny<UserActionNotification>()), Times.Once);
            mockUserManager.Verify(x => x.FindByEmailAsync(email), Times.Once);
            mockEmailClient.Verify(x => x.SendAsync(It.IsAny<EmailMessage>()), Times.Never);
            mockUrlGenerator.Verify(x => x.GenerateSiteUrl<AccountController>(It.IsAny<Expression<Func<AccountController, ActionResult>>>()), Times.Once);
        }

        [TestMethod]
        public async Task ResetsPasswordForValidUser()
        {
            ForgotPasswordModel model = new ForgotPasswordModel()
            {
                EmailAddress = "email@email.com",
            };

            IdentityUser user = new IdentityUser("unit.test")
            {
                Email = model.EmailAddress
            };

            OrganizationContext organizationContext = new OrganizationContext()
            {
                OrganizationID = Guid.NewGuid(),
                FullName = "Full Name",
                EmailAddress = "org@org.com"
            };

            ForgotPasswordCommand command = new ForgotPasswordCommand(model, organizationContext);

            string code = "code";
            string url = "http://www.password.reset.com/code";

            Mock<INotificationPublisher> mockNotificationPublisher = new Mock<INotificationPublisher>();
            mockNotificationPublisher.Setup(x => x.PublishAsync(It.IsAny<UserActionNotification>())).Returns<UserActionNotification>((notification) =>
            {
                notification.Action.Should().Be(UserActions.ForgotPasswordSuccess);
                notification.Username.Should().Be(model.EmailAddress);
                notification.AdditionalData.Should().BeNull();
                return Task.FromResult(0);
            });

            Mock<IApplicationUserManager> mockUserManager = new Mock<IApplicationUserManager>();
            mockUserManager.Setup(x => x.FindByEmailAsync(model.EmailAddress)).Returns(Task.FromResult(user));
            mockUserManager.Setup(x => x.GeneratePasswordResetTokenAsync(user.Id)).Returns(Task.FromResult(url));

            Mock<IEmailClient> mockEmailClient = new Mock<IEmailClient>();
            mockEmailClient.Setup(x => x.SendAsync(It.IsAny<EmailMessage>(), user.Id)).Returns<EmailMessage, string>((message, forUserID) =>
            {
                forUserID.Should().Be(user.Id);

                message.To.Should().HaveCount(1);
                message.To.First().Should().Be(user.Email);

                message.From.Should().Be(organizationContext.EmailAddress);
                message.Subject.Should().Be("Password Reset - " + organizationContext.FullName);
                message.HtmlBody.Should().Contain(url);
                message.TextBody.Should().Contain(url);

                return Task.FromResult(0);
            });

            Mock<IUrlGenerator> mockUrlGenerator = new Mock<IUrlGenerator>();
            mockUrlGenerator.Setup(x => x.GenerateAbsoluteUrl<AccountController>(It.IsAny<Expression<Func<AccountController, ActionResult>>>())).Returns(code);
            mockUrlGenerator.Setup(x => x.GenerateSiteUrl<AccountController>(It.IsAny<Expression<Func<AccountController, ActionResult>>>())).Returns("account-url");

            ForgotPasswordCommandHandler handler = new ForgotPasswordCommandHandler(mockNotificationPublisher.Object, mockUserManager.Object, mockEmailClient.Object, mockUrlGenerator.Object);
            CommandResult result = await handler.ExecuteAsync(command);
            result.Succeeded.Should().BeTrue();

            command.ModalDialog.Should().NotBeNull();
            command.ModalDialog.Text.Should().NotBeNullOrEmpty();
            command.ModalDialog.Buttons.Should().NotBeEmpty();
            command.ModalDialog.Buttons.Any(x => x.Url != null && x.Url.Contains("account-url")).Should().BeTrue();

            mockNotificationPublisher.Verify(x => x.PublishAsync(It.IsAny<UserActionNotification>()), Times.Once);
            mockUserManager.Verify(x => x.FindByEmailAsync(model.EmailAddress), Times.Once);
            mockUserManager.Verify(x => x.GeneratePasswordResetTokenAsync(user.Id), Times.Once);
            mockEmailClient.Verify(x => x.SendAsync(It.IsAny<EmailMessage>(), user.Id), Times.Once);
            mockUrlGenerator.Verify(x => x.GenerateAbsoluteUrl<AccountController>(It.IsAny<Expression<Func<AccountController, ActionResult>>>()), Times.Once);
            mockUrlGenerator.Verify(x => x.GenerateSiteUrl<AccountController>(It.IsAny<Expression<Func<AccountController, ActionResult>>>()), Times.Once);
        }
    }
}
