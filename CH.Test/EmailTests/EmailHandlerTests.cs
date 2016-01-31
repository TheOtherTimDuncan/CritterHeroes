using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CH.Test.Mocks;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.Email;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Email;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.DataProviders.Azure;
using CritterHeroes.Web.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TOTD.Utility.Misc;

namespace CH.Test.EmailTests
{
    [TestClass]
    public class EmailHandlerTests
    {
        public void VerifyEmailFolder<TEmailDataType>(EmailCommand<TEmailDataType> emailCommand) where TEmailDataType : BaseEmailData, new()
        {
            emailCommand.EmailName.Should().NotBeNullOrEmpty();
            string path = Path.Combine(UnitTestHelper.GetSolutionRoot(), "CritterHeroes.Web", "dist", "emails", emailCommand.EmailName);
            Directory.Exists(path).Should().BeTrue($"folder{path} should exist for email command {emailCommand.GetType().Name}");
        }

        [TestMethod]
        public void ResetPasswordAttemptEmailCommandHasExistingEmailFolder()
        {
            ResetPasswordAttemptEmailCommand command = new ResetPasswordAttemptEmailCommand("to");
            VerifyEmailFolder(command);
        }

        [TestMethod]
        public void ResetPasswordEmailCommandHasExistingEmailFolder()
        {
            ResetPasswordEmailCommand command = new ResetPasswordEmailCommand("to");
            VerifyEmailFolder(command);
        }

        [TestMethod]
        public void ConfirmEmailEmailCommandHasExistingEmailFolder()
        {
            ConfirmEmailEmailCommand command = new ConfirmEmailEmailCommand("to");
            VerifyEmailFolder(command);
        }

        [TestMethod]
        public void ResetPasswordNotificationEmailCommandHasExistingEmailFolder()
        {
            ResetPasswordNotificationEmailCommand command = new ResetPasswordNotificationEmailCommand("to");
            VerifyEmailFolder(command);
        }

        [TestMethod]
        public async Task EmailServiceAddsEmailToQueue()
        {
            string urlLogo = "logo";

            string pathRoot = "root";

            OrganizationContext organizationContext = new OrganizationContext()
            {
                FullName = "FullName",
                ShortName = "ShortName"
            };

            Mock<IEmailConfiguration> mockConfiguration = new Mock<IEmailConfiguration>();

            MockUrlGenerator mockUrlGenerator = new MockUrlGenerator();

            MockFileSystem mockFileSystem = new MockFileSystem();
            mockFileSystem.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns((string path) => path);
            mockFileSystem.Setup(x => x.MapServerPath(It.IsAny<string>())).Returns(pathRoot);

            Mock<IHttpContext> mockHttpContext = new Mock<IHttpContext>();

            Mock<IStateManager<OrganizationContext>> mockOrganizationStateManager = new Mock<IStateManager<OrganizationContext>>();
            mockOrganizationStateManager.Setup(x => x.GetContext()).Returns(organizationContext);

            Mock<IOrganizationLogoService> mockLogoService = new Mock<IOrganizationLogoService>();
            mockLogoService.Setup(x => x.GetLogoUrl()).Returns(urlLogo);

            Mock<IEmailLogger> mockEmailLogger = new Mock<IEmailLogger>();

            EmailCommand<EmailHandlerTests.TestEmailData> emailCommand = new EmailCommand<EmailHandlerTests.TestEmailData>("emailname", "emailto");

            EmailService emailService = new EmailService(mockFileSystem.Object, mockConfiguration.Object, new AzureConfiguration(), mockUrlGenerator.Object, mockOrganizationStateManager.Object, mockLogoService.Object, mockEmailLogger.Object);
            CommandResult commandResult = await emailService.SendEmailAsync(emailCommand);
            commandResult.Succeeded.Should().BeTrue();

            emailCommand.EmailData.OrganizationFullName.Should().Be(organizationContext.FullName);
            emailCommand.EmailData.OrganizationShortName.Should().Be(organizationContext.ShortName);
            emailCommand.EmailData.UrlLogo.Should().Be(urlLogo);
            emailCommand.EmailData.UrlHome.Should().NotBeNullOrEmpty();

            mockEmailLogger.Verify(x => x.LogEmailAsync(It.IsAny<EmailModel>()), Times.Once);
        }

        public class TestEmailData : BaseTokenEmailData
        {
            public string TestValue
            {
                get;
                set;
            }
        }
    }
}
