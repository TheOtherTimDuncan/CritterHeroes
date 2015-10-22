using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.Proxies.Configuration;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.DataProviders.Azure;
using CritterHeroes.Web.DataProviders.Azure.Storage;
using CritterHeroes.Web.DataProviders.Azure.Storage.Logging;
using CritterHeroes.Web.Models;
using CritterHeroes.Web.Models.Logging;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test.Azure
{
    [TestClass]
    public class EmailLogTests : BaseTest
    {
        //[TestMethod]
        public async Task CanSaveAndRetrieveEmailLog()
        {
            Organization org = new Organization();

            OrganizationContext orgContext = new OrganizationContext()
            {
                OrganizationID = org.ID,
                AzureName = "fflah"
            };

            EmailMessage message = new EmailMessage()
            {
                From = "from@from.com",
                HtmlBody = "html",
                TextBody = "text",
                Subject = "subject"
            };
            message.To.Add("to@to.com");

            EmailLog emailLog = new EmailLog(DateTimeOffset.UtcNow, message);

            Mock<IStateManager<OrganizationContext>> mockOrgStateManager = new Mock<IStateManager<OrganizationContext>>();
            mockOrgStateManager.Setup(x => x.GetContext()).Returns(orgContext);

            AzureEmailLogger logger = new AzureEmailLogger(new AzureConfiguration(), new EmailStorageService(mockOrgStateManager.Object, new AppConfiguration(), new AzureConfiguration()));
            await logger.LogEmailAsync(emailLog);

            IEnumerable<EmailLog> results = await logger.GetEmailLogAsync(DateTime.UtcNow.AddHours(-1), DateTime.UtcNow.AddHours(1));

            EmailLog result = results.FirstOrDefault(x => x.ID == emailLog.ID);
            result.Should().NotBeNull();
            result.EmailTo.Should().Be(emailLog.EmailTo);
            result.WhenSentUtc.Should().Be(emailLog.WhenSentUtc);

            //result.Message.Should().NotBeNull();
            //result.Message.From.Should().Be(emailLog.Message.From);
            //result.Message.HtmlBody.Should().Be(emailLog.Message.HtmlBody);
            //result.Message.TextBody.Should().Be(emailLog.Message.TextBody);
            //result.Message.Subject.Should().Be(emailLog.Message.Subject);
            //result.Message.To.Should().Equal(emailLog.Message.To);
        }
    }
}
