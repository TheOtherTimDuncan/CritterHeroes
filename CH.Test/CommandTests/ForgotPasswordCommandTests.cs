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
        public async Task ReturnsSuccessButDoesntSendEmailIfUserNotFound()
        {
            string email = "email@email.com";

            ForgotPasswordModel command = new ForgotPasswordModel()
            {
                ResetPasswordEmail = email
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

            ForgotPasswordCommandHandler handler = new ForgotPasswordCommandHandler(mockUserLogger.Object, mockUserManager.Object, mockEmailClient.Object, mockUrlGenerator.Object, organizationContext);
            CommandResult result = await handler.ExecuteAsync(command);
            result.Succeeded.Should().BeTrue();

            mockUserLogger.Verify(x => x.LogActionAsync(UserActions.ForgotPasswordFailure, command.ResetPasswordEmail), Times.Once);
            mockUserManager.Verify(x => x.FindByEmailAsync(email), Times.Once);
            mockEmailClient.Verify(x => x.SendAsync(It.IsAny<EmailMessage>()), Times.Never);
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

            OrganizationContext organizationContext = new OrganizationContext()
            {
                OrganizationID = Guid.NewGuid(),
                FullName = "Full Name",
                EmailAddress = "org@org.com"
            };

            string code = "code";
            string url = "http://www.password.reset.com/code";

            Mock<IUserLogger> mockUserLogger = new Mock<IUserLogger>();

            Mock<IApplicationUserManager> mockUserManager = new Mock<IApplicationUserManager>();
            mockUserManager.Setup(x => x.FindByEmailAsync(command.ResetPasswordEmail)).Returns(Task.FromResult(user));
            mockUserManager.Setup(x => x.GeneratePasswordResetTokenAsync(user.Id)).Returns(Task.FromResult(url));

            Mock<IEmailClient> mockEmailClient = new Mock<IEmailClient>();
            mockEmailClient.Setup(x => x.SendAsync(It.IsAny<EmailMessage>(), user.Id)).Returns((EmailMessage message, string forUserID) =>
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

            ForgotPasswordCommandHandler handler = new ForgotPasswordCommandHandler(mockUserLogger.Object, mockUserManager.Object, mockEmailClient.Object, mockUrlGenerator.Object, organizationContext);
            CommandResult result = await handler.ExecuteAsync(command);
            result.Succeeded.Should().BeTrue();

            mockUserLogger.Verify(x => x.LogActionAsync(UserActions.ForgotPasswordSuccess, user.Email), Times.Once);
            mockUserManager.Verify(x => x.FindByEmailAsync(command.ResetPasswordEmail), Times.Once);
            mockUserManager.Verify(x => x.GeneratePasswordResetTokenAsync(user.Id), Times.Once);
            mockEmailClient.Verify(x => x.SendAsync(It.IsAny<EmailMessage>(), user.Id), Times.Once);
            mockUrlGenerator.Verify(x => x.GenerateAbsoluteUrl<AccountController>(It.IsAny<Expression<Func<AccountController, ActionResult>>>()), Times.Once);
        }
    }
}
