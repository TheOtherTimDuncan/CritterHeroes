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
    public class ForgotUsernameCommandTests
    {
        [TestMethod]
        public async Task ReturnsSuccessButDoesntSendEmailIfUserNotFound()
        {
            string email = "email@email.com";

            ForgotUsernameModel command = new ForgotUsernameModel()
            {
                EmailAddress = email
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

            ForgotUsernameCommandHandler handler = new ForgotUsernameCommandHandler(mockUserLogger.Object, mockUserManager.Object, mockEmailClient.Object, mockUrlGenerator.Object, organizationContext);
            CommandResult result = await handler.ExecuteAsync(command);
            result.Succeeded.Should().BeFalse();
            result.Errors.Should().BeEmpty();

            command.ModalDialog.Should().NotBeNull();
            command.ModalDialog.Text.Should().NotBeNullOrEmpty();
            command.ModalDialog.Buttons.Should().NotBeEmpty();
            command.ModalDialog.Buttons.Any(x => x.Url != null && x.Url.Contains("account-url")).Should().BeTrue();

            mockUserLogger.Verify(x => x.LogActionAsync(UserActions.ForgotUsernameFailure, null, command.EmailAddress), Times.Once);
            mockUserManager.Verify(x => x.FindByEmailAsync(email), Times.Once);
            mockEmailClient.Verify(x => x.SendAsync(It.IsAny<EmailMessage>()), Times.Never);
            mockUrlGenerator.Verify(x => x.GenerateSiteUrl<AccountController>(It.IsAny<Expression<Func<AccountController, ActionResult>>>()), Times.Once);
        }

        [TestMethod]
        public async Task SendsEmailWithUsernameForValidUser()
        {
            ForgotUsernameModel command = new ForgotUsernameModel()
            {
                EmailAddress = "email@email.com",
            };

            IdentityUser user = new IdentityUser("unit.test")
            {
                Email = command.EmailAddress
            };

            OrganizationContext organizationContext = new OrganizationContext()
            {
                OrganizationID = Guid.NewGuid(),
                FullName = "Full Name",
                EmailAddress = "org@org.com"
            };

            Mock<IUserLogger> mockUserLogger = new Mock<IUserLogger>();

            Mock<IApplicationUserManager> mockUserManager = new Mock<IApplicationUserManager>();
            mockUserManager.Setup(x => x.FindByEmailAsync(command.EmailAddress)).Returns(Task.FromResult(user));

            Mock<IEmailClient> mockEmailClient = new Mock<IEmailClient>();
            mockEmailClient.Setup(x => x.SendAsync(It.IsAny<EmailMessage>(), user.Id)).Returns((EmailMessage message, string forUserID) =>
            {
                forUserID.Should().Be(user.Id);

                message.To.Should().HaveCount(1);
                message.To.First().Should().Be(user.Email);

                message.From.Should().Be(organizationContext.EmailAddress);
                message.Subject.Should().Be("Forgot Username - " + organizationContext.FullName);
                message.HtmlBody.Should().Contain(user.UserName);
                message.TextBody.Should().Contain(user.UserName);

                return Task.FromResult(0);
            });

            Mock<IUrlGenerator> mockUrlGenerator = new Mock<IUrlGenerator>();
            mockUrlGenerator.Setup(x => x.GenerateSiteUrl<AccountController>(It.IsAny<Expression<Func<AccountController, ActionResult>>>())).Returns("account-url");

            ForgotUsernameCommandHandler handler = new ForgotUsernameCommandHandler(mockUserLogger.Object, mockUserManager.Object, mockEmailClient.Object, mockUrlGenerator.Object, organizationContext);
            CommandResult result = await handler.ExecuteAsync(command);
            result.Succeeded.Should().BeTrue();

            command.ModalDialog.Should().NotBeNull();
            command.ModalDialog.Text.Should().NotBeNullOrEmpty();
            command.ModalDialog.Buttons.Should().NotBeEmpty();
            command.ModalDialog.Buttons.Any(x => x.Url != null && x.Url.Contains("account-url")).Should().BeTrue();

            mockUserLogger.Verify(x => x.LogActionAsync(UserActions.ForgotUsernameSuccess, user.UserName), Times.Once);
            mockUserManager.Verify(x => x.FindByEmailAsync(command.EmailAddress), Times.Once);
            mockEmailClient.Verify(x => x.SendAsync(It.IsAny<EmailMessage>(), user.Id), Times.Once);
            mockUrlGenerator.Verify(x => x.GenerateSiteUrl<AccountController>(It.IsAny<Expression<Func<AccountController, ActionResult>>>()), Times.Once);
        }
    }
}
