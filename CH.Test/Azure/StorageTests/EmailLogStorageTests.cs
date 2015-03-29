using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.DataProviders.Azure;
using CritterHeroes.Web.DataProviders.Azure.Storage.Logging;
using CritterHeroes.Web.Models;
using CritterHeroes.Web.Models.Logging;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CH.Test.Azure.StorageTests
{
    [TestClass]
    public class EmailLogStorageTests : BaseAzureTest
    {
        [TestMethod]
        public void SuccessfullyMapsEntityToAndFromStorage()
        {
            EmailMessage message = new EmailMessage()
            {
                From = "from@from.com",
                HtmlBody = "html",
                TextBody = "text",
                Subject = "subject"
            };
            message.To.Add("to@to.com");

            EmailLog emailLog = new EmailLog(DateTime.UtcNow, message);
            emailLog.EmailTo.Should().Be(message.To.Single());

            AzureEmailLogger source = new AzureEmailLogger(new AzureConfiguration());
            AzureEmailLogger target = new AzureEmailLogger(new AzureConfiguration());
            EmailLog result = target.FromStorage(source.ToStorage(emailLog));

            result.ID.Should().Be(emailLog.ID);
            result.EmailTo.Should().Be(emailLog.EmailTo);
            result.WhenSentUtc.Should().Be(emailLog.WhenSentUtc);
            result.WhenSentUtc.Kind.Should().Be(DateTimeKind.Utc);

            result.Message.Should().NotBeNull();
            result.Message.From.Should().Be(emailLog.Message.From);
            result.Message.HtmlBody.Should().Be(emailLog.Message.HtmlBody);
            result.Message.TextBody.Should().Be(emailLog.Message.TextBody);
            result.Message.Subject.Should().Be(emailLog.Message.Subject);
            result.Message.To.Should().Equal(emailLog.Message.To);
        }
    }
}
