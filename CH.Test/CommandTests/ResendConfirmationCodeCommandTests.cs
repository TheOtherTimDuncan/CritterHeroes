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
    public class ResendConfirmationCodeCommandTests
    {
        [TestMethod]
        public async Task ReturnsFailedAndDoesNotSendEmailIfUserNotFound()
        {
            ResendConfirmationCodeModel command = new ResendConfirmationCodeModel()
            {
                EmailAddress = "email@email.com"
            };

            OrganizationContext organizationContext = new OrganizationContext()
            {
                OrganizationID = Guid.NewGuid(),
                FullName = "Full Name",
                EmailAddress = "org@org.com"
            };

            Mock<IUserLogger> mockUserLogger = new Mock<IUserLogger>();
            Mock<IApplicationUserManager> mockUserManager = new Mock<IApplicationUserManager>();
            Mock<IEmailClient> mockEmailClient = new Mock<IEmailClient>();

            Mock<IUrlGenerator> mockUrlGenerator = new Mock<IUrlGenerator>();
            mockUrlGenerator.Setup(x => x.GenerateSiteUrl<AccountController>(It.IsAny<Expression<Func<AccountController, ActionResult>>>())).Returns("account-url");

            ResendConfirmationCodeCommandHandler handler = new ResendConfirmationCodeCommandHandler(mockUserLogger.Object, mockUserManager.Object, mockEmailClient.Object, mockUrlGenerator.Object, organizationContext);
            CommandResult commandResult = await handler.ExecuteAsync(command);

            commandResult.Succeeded.Should().BeFalse();
            commandResult.Errors.Should().BeEmpty();

            command.ModalDialog.Should().NotBeNull();
            command.ModalDialog.Text.Should().NotBeNullOrEmpty();
            command.ModalDialog.Buttons.Should().NotBeEmpty();
            command.ModalDialog.Buttons.Any(x => x.Url != null && x.Url.Contains("account-url")).Should().BeTrue();

            mockUserLogger.Verify(x => x.LogActionAsync(UserActions.ResendConfirmationCodeFailure, null, command.EmailAddress), Times.Once);
            mockUserManager.Verify(x => x.FindByEmailAsync(command.EmailAddress), Times.Once);
            mockEmailClient.Verify(x => x.SendAsync(It.IsAny<EmailMessage>()), Times.Never);
            mockUrlGenerator.Verify(x => x.GenerateSiteUrl<AccountController>(It.IsAny<Expression<Func<AccountController, ActionResult>>>()), Times.Once);
        }

        [TestMethod]
        public async Task ResendsEmailConfirmationEmailIfUserEmailIsNotConfirmed()
        {
            ResendConfirmationCodeModel command = new ResendConfirmationCodeModel()
            {
                EmailAddress = "email@email.com"
            };

            IdentityUser user = new IdentityUser("unit.test")
            {
                Email = command.EmailAddress,
                IsEmailConfirmed = false
            };

            OrganizationContext organizationContext = new OrganizationContext()
            {
                OrganizationID = Guid.NewGuid(),
                FullName = "Full Name",
                EmailAddress = "org@org.com"
            };

            Mock<IUserLogger> mockUserLogger = new Mock<IUserLogger>();

            Mock<IEmailClient> mockEmailClient = new Mock<IEmailClient>();
            mockEmailClient.Setup(x => x.SendAsync(It.IsAny<EmailMessage>(), user.Id)).Returns((EmailMessage message, string forUserID) =>
            {
                forUserID.Should().Be(user.Id);

                message.To.Should().HaveCount(1);
                message.To.First().Should().Be(user.Email);

                message.From.Should().Be(organizationContext.EmailAddress);
                message.Subject.Should().Be("Confirm Email - " + organizationContext.FullName);

                return Task.FromResult(0);
            });

            Mock<IApplicationUserManager> mockUserManager = new Mock<IApplicationUserManager>();
            mockUserManager.Setup(x => x.FindByEmailAsync(command.EmailAddress)).Returns(Task.FromResult(user));

            Mock<IUrlGenerator> mockUrlGenerator = new Mock<IUrlGenerator>();
            mockUrlGenerator.Setup(x => x.GenerateSiteUrl<AccountController>(It.IsAny<Expression<Func<AccountController, ActionResult>>>())).Returns("account-url");

            ResendConfirmationCodeCommandHandler handler = new ResendConfirmationCodeCommandHandler(mockUserLogger.Object, mockUserManager.Object, mockEmailClient.Object, mockUrlGenerator.Object, organizationContext);
            CommandResult commandResult = await handler.ExecuteAsync(command);

            commandResult.Succeeded.Should().BeTrue();
            commandResult.Errors.Should().BeEmpty();

            command.ModalDialog.Should().NotBeNull();
            command.ModalDialog.Text.Should().NotBeNullOrEmpty();
            command.ModalDialog.Buttons.Should().NotBeEmpty();
            command.ModalDialog.Buttons.Any(x => x.Url != null && x.Url.Contains("account-url")).Should().BeTrue();

            mockUserLogger.Verify(x => x.LogActionAsync(UserActions.ResendConfirmationCodeSuccess, user.UserName), Times.Once);
            mockUserManager.Verify(x => x.FindByEmailAsync(command.EmailAddress), Times.Once);
            mockEmailClient.Verify(x => x.SendAsync(It.IsAny<EmailMessage>()), Times.Never);
            mockUrlGenerator.Verify(x => x.GenerateSiteUrl<AccountController>(It.IsAny<Expression<Func<AccountController, ActionResult>>>()), Times.Once);
        }

        [TestMethod]
        public async Task ResendsResetPasswordEmailIfUserEmailIsAlreadyConfirmed()
        {
            ResendConfirmationCodeModel command = new ResendConfirmationCodeModel()
            {
                EmailAddress = "email@email.com"
            };

            IdentityUser user = new IdentityUser("unit.test")
            {
                Email = command.EmailAddress,
                IsEmailConfirmed = true
            };

            OrganizationContext organizationContext = new OrganizationContext()
            {
                OrganizationID = Guid.NewGuid(),
                FullName = "Full Name",
                EmailAddress = "org@org.com"
            };

            Mock<IUserLogger> mockUserLogger = new Mock<IUserLogger>();

            Mock<IEmailClient> mockEmailClient = new Mock<IEmailClient>();
            mockEmailClient.Setup(x => x.SendAsync(It.IsAny<EmailMessage>(), user.Id)).Returns((EmailMessage message, string forUserID) =>
            {
                forUserID.Should().Be(user.Id);

                message.To.Should().HaveCount(1);
                message.To.First().Should().Be(user.Email);

                message.From.Should().Be(organizationContext.EmailAddress);
                message.Subject.Should().Be("Reset Password - " + organizationContext.FullName);

                return Task.FromResult(0);
            });

            Mock<IApplicationUserManager> mockUserManager = new Mock<IApplicationUserManager>();
            mockUserManager.Setup(x => x.FindByEmailAsync(command.EmailAddress)).Returns(Task.FromResult(user));

            Mock<IUrlGenerator> mockUrlGenerator = new Mock<IUrlGenerator>();
            mockUrlGenerator.Setup(x => x.GenerateSiteUrl<AccountController>(It.IsAny<Expression<Func<AccountController, ActionResult>>>())).Returns("account-url");

            ResendConfirmationCodeCommandHandler handler = new ResendConfirmationCodeCommandHandler(mockUserLogger.Object, mockUserManager.Object, mockEmailClient.Object, mockUrlGenerator.Object, organizationContext);
            CommandResult commandResult = await handler.ExecuteAsync(command);

            commandResult.Succeeded.Should().BeTrue();
            commandResult.Errors.Should().BeEmpty();

            command.ModalDialog.Should().NotBeNull();
            command.ModalDialog.Text.Should().NotBeNullOrEmpty();
            command.ModalDialog.Buttons.Should().NotBeEmpty();
            command.ModalDialog.Buttons.Any(x => x.Url != null && x.Url.Contains("account-url")).Should().BeTrue();

            mockUserLogger.Verify(x => x.LogActionAsync(UserActions.ResendConfirmationCodeSuccess, user.UserName), Times.Once);
            mockUserManager.Verify(x => x.FindByEmailAsync(command.EmailAddress), Times.Once);
            mockEmailClient.Verify(x => x.SendAsync(It.IsAny<EmailMessage>()), Times.Never);
            mockUrlGenerator.Verify(x => x.GenerateSiteUrl<AccountController>(It.IsAny<Expression<Func<AccountController, ActionResult>>>()), Times.Once);
        }
    }
}
