using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Azure.Logging;
using CH.Domain.Models;
using CH.Domain.Models.Logging;
using CH.Domain.Proxies.Configuration;
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
            EmailLog emailLog = new EmailLog(DateTime.UtcNow, new EmailMessage()
            {
                From = "from@from.com",
                HtmlBody = "html",
                TextBody = "text",
                Subject = "subject"
            })
            {
                ForUserID = "userid"
            };
            emailLog.Message.To.Add("to@to.com");

            AzureEmailLogger logger = new AzureEmailLogger(new AzureConfiguration());
            await logger.LogEmailAsync(emailLog);

            IEnumerable<EmailLog> results = await logger.GetEmailLogAsync(DateTime.UtcNow.AddHours(-1), DateTime.UtcNow.AddHours(1));

            EmailLog result = results.FirstOrDefault(x => x.ID == emailLog.ID);
            result.Should().NotBeNull();
            result.ForUserID.Should().Be(emailLog.ForUserID);
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
