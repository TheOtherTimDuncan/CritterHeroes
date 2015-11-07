using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CH.Test.Mocks;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.Email;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Email;
using CritterHeroes.Web.DataProviders.Azure;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TOTD.Utility.Misc;

namespace CH.Test.EmailTests
{
    [TestClass]
    public class EmailHandlerTests
    {
        public void VerifyEmailFolder(EmailCommand emailCommand)
        {
            string path = Path.Combine(UnitTestHelper.GetSolutionRoot(), "CritterHeroes.Web", "Areas", "Emails", emailCommand.EmailName);
            Directory.Exists(path).Should().BeTrue($"folder{path} should exist for email command {emailCommand.GetType().Name}");
        }

        [TestMethod]
        public void ResetPasswordAttemptEmailCommandHasExistingEmailFolder()
        {
            ResetPasswordAttemptEmailCommand command = new ResetPasswordAttemptEmailCommand("to", "url", "logo", "org");
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
            ResetPasswordNotificationEmailCommand command = new ResetPasswordNotificationEmailCommand("to", "url", "logo", "org");
            VerifyEmailFolder(command);
        }

        [TestMethod]
        public async Task EmailServiceAddsEmailToQueue()
        {
            EmailCommand emailCommand = new EmailCommand("emailname", "emailto");

            string pathRoot = "root";

            Mock<IEmailConfiguration> mockConfiguration = new Mock<IEmailConfiguration>();

            MockFileSystem mockFileSystem = new MockFileSystem();
            mockFileSystem.Setup(x => x.ReadAllText(It.IsAny<string>())).Returns((string path) => path);

            Mock<IHttpContext> mockHttpContext = new Mock<IHttpContext>();
            mockHttpContext.Setup(x => x.Server.MapPath(It.IsAny<string>())).Returns(pathRoot);

            EmailService emailService = new EmailService(mockFileSystem.Object, mockHttpContext.Object, mockConfiguration.Object, new AzureConfiguration());
            CommandResult commandResult = await emailService.SendEmailAsync(emailCommand);
            commandResult.Succeeded.Should().BeTrue();
        }
    }
}
