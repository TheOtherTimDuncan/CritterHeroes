using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.DataProviders.Azure;
using CritterHeroes.Web.DataProviders.Azure.Storage.Logging;
using CritterHeroes.Web.Models;
using CritterHeroes.Web.Models.Logging;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CH.Test.Azure
{
    [TestClass]
    public class EmailLogTests : BaseTest
    {
        [TestMethod]
        public async Task CanSaveAndRetrieveEmailLog()
        {
            EmailMessage message = new EmailMessage()
            {
                From = "from@from.com",
                HtmlBody = "html",
                TextBody = "text",
                Subject = "subject"
            };
            message.To.Add("to@to.com");

            EmailLog emailLog = new EmailLog(DateTimeOffset.UtcNow, message);

            AzureEmailLogger logger = new AzureEmailLogger(new AzureConfiguration());
            await logger.LogEmailAsync(emailLog);

            IEnumerable<EmailLog> results = await logger.GetEmailLogAsync(DateTime.UtcNow.AddHours(-1), DateTime.UtcNow.AddHours(1));

            EmailLog result = results.FirstOrDefault(x => x.ID == emailLog.ID);
            result.Should().NotBeNull();
            result.EmailTo.Should().Be(emailLog.EmailTo);
            result.WhenSentUtc.Should().Be(emailLog.WhenSentUtc);

            result.Message.Should().NotBeNull();
            result.Message.From.Should().Be(emailLog.Message.From);
            result.Message.HtmlBody.Should().Be(emailLog.Message.HtmlBody);
            result.Message.TextBody.Should().Be(emailLog.Message.TextBody);
            result.Message.Subject.Should().Be(emailLog.Message.Subject);
            result.Message.To.Should().Equal(emailLog.Message.To);
        }
    }
}
