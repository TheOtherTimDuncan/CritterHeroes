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

            (await handler.Execute(model)).Succeeded.Should().BeFalse("both email address and username are null");

            model.EmailAddress = "";
            model.Username = "";
            (await handler.Execute(model)).Succeeded.Should().BeFalse("both email address and username are empty");

            model.EmailAddress = "  ";
            model.Username = "  ";
            (await handler.Execute(model)).Succeeded.Should().BeFalse("both email address and username are only whitespace");
        }

        [TestMethod]
        public async Task ReturnsSuccessButDoesntSendEmailIfUserNotFound()
        {
            ForgotPasswordCommand model = new ForgotPasswordCommand()
            {
            };

            string email = "email@email.com";
            string username = "unit.test";

            Mock<IApplicationUserManager> mockUserManager = new Mock<IApplicationUserManager>();
            mockUserManager.Setup(x => x.FindByEmailAsync(email)).Returns(Task.FromResult((IdentityUser)null));
            mockUserManager.Setup(x => x.FindByNameAsync(username)).Returns(Task.FromResult((IdentityUser)null));

            Mock<IEmailClient> mockEmailClient = new Mock<IEmailClient>();
            mockEmailClient.Setup(x => x.SendAsync(It.IsAny<EmailMessage>()));

            Mock<IUserLogger> mockLogger = new Mock<IUserLogger>();
            mockLogger.Setup(x => x.LogActionAsync(UserActions.ForgotPasswordFailure, null, model)).Returns(Task.FromResult(0));

            ForgotPasswordCommandHandler handler = new ForgotPasswordCommandHandler(mockUserManager.Object, mockEmailClient.Object, mockLogger.Object);

            model.EmailAddress = email;
            (await handler.Execute(model)).Succeeded.Should().BeTrue();

            model.EmailAddress = null;
            model.Username = username;
            (await handler.Execute(model)).Succeeded.Should().BeTrue();

            mockUserManager.Verify(x => x.FindByEmailAsync(email), Times.Once);
            mockUserManager.Verify(x => x.FindByNameAsync(username), Times.Once);
            mockEmailClient.Verify(x => x.SendAsync(It.IsAny<EmailMessage>()), Times.Never);
            mockLogger.Verify(x => x.LogActionAsync(UserActions.ForgotPasswordFailure, null, model), Times.Exactly(2));
        }

        [TestMethod]
        public async Task ResetsPasswordForValidUser()
        {
            ForgotPasswordCommand model = new ForgotPasswordCommand()
            {
                EmailAddress = "email@email.com",
                OrganizationFullName = "Full Name"
            };

            IdentityUser user = new IdentityUser("unit.test")
            {
                Email = model.EmailAddress
            };

            string code = "code";
            string url = "http://www.password.reset.com/code";

            Mock<IApplicationUserManager> mockUserManager = new Mock<IApplicationUserManager>();
            mockUserManager.Setup(x => x.FindByEmailAsync(model.EmailAddress)).Returns(Task.FromResult(user));
            mockUserManager.Setup(x => x.GeneratePasswordResetTokenAsync(user.Id)).Returns(Task.FromResult(url));

            Mock<IUserLogger> mockLogger = new Mock<IUserLogger>();
            mockLogger.Setup(x => x.LogActionAsync(UserActions.ForgotPasswordSuccess, user.UserName, model)).Returns(Task.FromResult(0));

            Mock<IEmailClient> mockEmailClient = new Mock<IEmailClient>();
            mockEmailClient.Setup(x => x.SendAsync(It.IsAny<EmailMessage>())).Returns<EmailMessage>((message) =>
            {
                message.To.Should().HaveCount(1);
                message.To.First().Should().Be(user.Email);

                message.Subject.Should().Be("Password Reset - " + model.OrganizationFullName);
                message.HtmlBody.Should().Contain(url);
                message.TextBody.Should().Contain(url);

                return Task.FromResult(0);
            });

            Mock<IUrlGenerator> mockUrlGenerator = new Mock<IUrlGenerator>();
            model.UrlGenerator = mockUrlGenerator.Object;
            mockUrlGenerator.Setup(x => x.GenerateAbsoluteUrl<AccountController>(It.IsAny<Expression<Func<AccountController, ActionResult>>>())).Returns(code);

            ForgotPasswordCommandHandler handler = new ForgotPasswordCommandHandler(mockUserManager.Object, mockEmailClient.Object, mockLogger.Object);
            (await handler.Execute(model)).Succeeded.Should().BeTrue();

            mockUserManager.Verify(x => x.FindByEmailAsync(model.EmailAddress), Times.Once);
            mockUserManager.Verify(x => x.GeneratePasswordResetTokenAsync(user.Id), Times.Once);
            mockEmailClient.Verify(x => x.SendAsync(It.IsAny<EmailMessage>()), Times.Once);
            mockLogger.Verify(x => x.LogActionAsync(UserActions.ForgotPasswordSuccess, user.UserName, model), Times.Once);
            mockUrlGenerator.Verify(x => x.GenerateAbsoluteUrl<AccountController>(It.IsAny<Expression<Func<AccountController, ActionResult>>>()), Times.Once);
        }
    }
}
