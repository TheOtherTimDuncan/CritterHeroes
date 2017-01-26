using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Domain.Contracts.Email;
using CritterHeroes.Web.Domain.Contracts.Events;
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
        public async Task SendsEmail()
        {
            EmailMessage emailMessage = new EmailMessage();

            TestEmailCommand emailCommand = new TestEmailCommand("emailto");

            Mock<IAppEventPublisher> mockPublisher = new Mock<IAppEventPublisher>();

            Mock<IEmailBuilder<TestEmailCommand>> mockEmailBuilder = new Mock<IEmailBuilder<TestEmailCommand>>();
            mockEmailBuilder.Setup(x => x.BuildEmail(emailCommand)).Returns(emailMessage);

            EmailService<TestEmailCommand> emailService = new EmailService<TestEmailCommand>(mockPublisher.Object, mockEmailBuilder.Object);

            CommandResult commandResult = await emailService.SendEmailAsync(emailCommand);
            commandResult.Succeeded.Should().BeTrue();

            mockEmailBuilder.Verify(x => x.BuildEmail(emailCommand), Times.Once);
            mockPublisher.Verify(x => x.Publish(It.IsAny<EmailLogEvent>()), Times.Once);
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
