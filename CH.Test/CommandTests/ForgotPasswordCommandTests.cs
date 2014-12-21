using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using CH.Domain.Contracts;
using CH.Domain.Contracts.Configuration;
using CH.Domain.Contracts.Email;
using CH.Domain.Contracts.Identity;
using CH.Domain.Contracts.Logging;
using CH.Domain.Contracts.Queries;
using CH.Domain.Identity;
using CH.Domain.Models;
using CH.Domain.Models.Logging;
using CH.Domain.Services.Commands;
using CH.Domain.Services.Queries;
using CH.Domain.StateManagement;
using CH.Website.Controllers;
using CH.Website.Models.Account;
using CH.Website.Services.CommandHandlers;
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
            ForgotPasswordCommandHandler handler = new ForgotPasswordCommandHandler(null, null, null, null, null, null);
            ForgotPasswordModel model = new ForgotPasswordModel();

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

            ForgotPasswordModel command = new ForgotPasswordModel()
            {
                EmailAddress = email
            };

            OrganizationContext organizationContext = new OrganizationContext()
            {
                OrganizationID = Guid.NewGuid(),
                FullName = "Full Name",
                EmailAddress = "org@org.com"
            };

            Mock<IApplicationUserManager> mockUserManager = new Mock<IApplicationUserManager>();
            mockUserManager.Setup(x => x.FindByNameAsync(username)).Returns(Task.FromResult((IdentityUser)null));

            Mock<IEmailClient> mockEmailClient = new Mock<IEmailClient>();
            mockEmailClient.Setup(x => x.SendAsync(It.IsAny<EmailMessage>()));

            Mock<IUserLogger> mockLogger = new Mock<IUserLogger>();
            mockLogger.Setup(x => x.LogActionAsync(UserActions.ForgotPasswordFailure, null, command)).Returns(Task.FromResult(0));

            Mock<IUrlGenerator> mockUrlGenerator = new Mock<IUrlGenerator>();
            mockUrlGenerator.Setup(x => x.GenerateSiteUrl<AccountController>(It.IsAny<Expression<Func<AccountController, ActionResult>>>())).Returns("account-url");

            Mock<IAsyncQueryHandler<OrganizationQuery, OrganizationContext>> mockHandler = new Mock<IAsyncQueryHandler<OrganizationQuery, OrganizationContext>>();
            mockHandler.Setup(x => x.RetrieveAsync(It.IsAny<OrganizationQuery>())).Returns(Task.FromResult(organizationContext));

            Mock<IAppConfiguration> mockAppConfiguration = new Mock<IAppConfiguration>();
            mockAppConfiguration.Setup(x => x.OrganizationID).Returns(organizationContext.OrganizationID);

            ForgotPasswordCommandHandler handler = new ForgotPasswordCommandHandler(mockUserManager.Object, mockEmailClient.Object, mockLogger.Object, mockHandler.Object, mockAppConfiguration.Object, mockUrlGenerator.Object);
            CommandResult result = await handler.ExecuteAsync(command);
            result.Succeeded.Should().BeTrue();

            command.ModalDialog.Should().NotBeNull();
            command.ModalDialog.Text.Should().NotBeNullOrEmpty();
            command.ModalDialog.Buttons.Should().NotBeEmpty();
            command.ModalDialog.Buttons.Any(x => x.Url != null && x.Url.Contains("account-url")).Should().BeTrue();

            mockUserManager.Verify(x => x.FindByEmailAsync(email), Times.Once);
            mockEmailClient.Verify(x => x.SendAsync(It.IsAny<EmailMessage>()), Times.Never);
            mockLogger.Verify(x => x.LogActionAsync(UserActions.ForgotPasswordFailure, null, command), Times.Once);
            mockUrlGenerator.Verify(x => x.GenerateSiteUrl<AccountController>(It.IsAny<Expression<Func<AccountController, ActionResult>>>()), Times.Once);
            mockHandler.Verify(x => x.RetrieveAsync(It.IsAny<OrganizationQuery>()), Times.Once);
            mockAppConfiguration.Verify(x => x.OrganizationID, Times.Once);
        }

        [TestMethod]
        public async Task ResetsPasswordForValidUser()
        {
            ForgotPasswordModel command = new ForgotPasswordModel()
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

                message.From.Should().Be(organizationContext.EmailAddress);
                message.Subject.Should().Be("Password Reset - " + organizationContext.FullName);
                message.HtmlBody.Should().Contain(url);
                message.TextBody.Should().Contain(url);

                return Task.FromResult(0);
            });

            Mock<IAsyncQueryHandler<OrganizationQuery, OrganizationContext>> mockHandler = new Mock<IAsyncQueryHandler<OrganizationQuery, OrganizationContext>>();
            mockHandler.Setup(x => x.RetrieveAsync(It.IsAny<OrganizationQuery>())).Returns(Task.FromResult(organizationContext));

            Mock<IAppConfiguration> mockAppConfiguration = new Mock<IAppConfiguration>();
            mockAppConfiguration.Setup(x => x.OrganizationID).Returns(organizationContext.OrganizationID);

            Mock<IUrlGenerator> mockUrlGenerator = new Mock<IUrlGenerator>();
            mockUrlGenerator.Setup(x => x.GenerateAbsoluteUrl<AccountController>(It.IsAny<Expression<Func<AccountController, ActionResult>>>())).Returns(code);
            mockUrlGenerator.Setup(x => x.GenerateSiteUrl<AccountController>(It.IsAny<Expression<Func<AccountController, ActionResult>>>())).Returns("account-url");

            ForgotPasswordCommandHandler handler = new ForgotPasswordCommandHandler(mockUserManager.Object, mockEmailClient.Object, mockLogger.Object, mockHandler.Object, mockAppConfiguration.Object, mockUrlGenerator.Object);
            CommandResult result = await handler.ExecuteAsync(command);
            result.Succeeded.Should().BeTrue();
            
            command.ModalDialog.Should().NotBeNull();
            command.ModalDialog.Text.Should().NotBeNullOrEmpty();
            command.ModalDialog.Buttons.Should().NotBeEmpty();
            command.ModalDialog.Buttons.Any(x => x.Url != null && x.Url.Contains("account-url")).Should().BeTrue();

            mockUserManager.Verify(x => x.FindByEmailAsync(command.EmailAddress), Times.Once);
            mockUserManager.Verify(x => x.GeneratePasswordResetTokenAsync(user.Id), Times.Once);
            mockEmailClient.Verify(x => x.SendAsync(It.IsAny<EmailMessage>(), user.Id), Times.Once);
            mockLogger.Verify(x => x.LogActionAsync(UserActions.ForgotPasswordSuccess, user.UserName, command), Times.Once);
            mockUrlGenerator.Verify(x => x.GenerateAbsoluteUrl<AccountController>(It.IsAny<Expression<Func<AccountController, ActionResult>>>()), Times.Once);
            mockUrlGenerator.Verify(x => x.GenerateSiteUrl<AccountController>(It.IsAny<Expression<Func<AccountController, ActionResult>>>()), Times.Once);
            mockHandler.Verify(x => x.RetrieveAsync(It.IsAny<OrganizationQuery>()), Times.Once);
            mockAppConfiguration.Verify(x => x.OrganizationID, Times.Once);
        }
    }
}
