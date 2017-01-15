using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Email;
using CritterHeroes.Web.Contracts.Events;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Models.LogEvents;
using CritterHeroes.Web.Shared.Commands;
using CritterHeroes.Web.Shared.Email;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TOTD.Mailer.Core;

namespace CH.Test.EmailTests
{
    [TestClass]
    public class EmailServiceTests
    {
        [TestMethod]
        public async Task AddsEmailToQueueAndSavesEmailToBlobStorage()
        {
            bool isPrivate = true;

            EmailMessage emailMessage = new EmailMessage();

            TestEmailCommand emailCommand = new TestEmailCommand("emailto");

            Mock<IEmailConfiguration> mockEmailConfiguration = new Mock<IEmailConfiguration>();

            Mock<IHttpContext> mockHttpContext = new Mock<IHttpContext>();

            Mock<IAppEventPublisher> mockPublisher = new Mock<IAppEventPublisher>();

            Mock<IAzureService> mockAzureService = new Mock<IAzureService>();

            Mock<IEmailBuilder<TestEmailCommand>> mockEmailBuilder = new Mock<IEmailBuilder<TestEmailCommand>>();
            mockEmailBuilder.Setup(x => x.BuildEmail(emailCommand)).Returns(emailMessage);

            EmailService<TestEmailCommand> emailService = new EmailService<TestEmailCommand>(mockEmailConfiguration.Object, mockAzureService.Object, mockPublisher.Object, mockEmailBuilder.Object);

            CommandResult commandResult = await emailService.SendEmailAsync(emailCommand);
            commandResult.Succeeded.Should().BeTrue();

            mockEmailBuilder.Verify(x => x.BuildEmail(emailCommand), Times.Once);
            mockPublisher.Verify(x => x.Publish(It.IsAny<EmailLogEvent>()), Times.Once);
            mockAzureService.Verify(x => x.AddQueueMessageAsync("email", It.IsAny<string>()), Times.Once);
            mockAzureService.Verify(x => x.UploadBlobAsync(It.IsAny<string>(), isPrivate, It.IsAny<string>()), Times.Once);
        }

        public class TestEmailCommand : EmailTokenCommandBase
        {
            public TestEmailCommand(string emailTo)
            : base(emailTo)
            {
            }

            public string TestValue
            {
                get;
                set;
            }
        }
    }
}
