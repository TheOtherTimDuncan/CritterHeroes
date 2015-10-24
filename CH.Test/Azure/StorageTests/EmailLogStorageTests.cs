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

namespace CH.Test.Azure.StorageTests
{
    [TestClass]
    public class EmailLogStorageTests : BaseAzureTest
    {
        [TestMethod]
        public void SuccessfullyMapsEntityToAndFromStorage()
        {
            EmailMessage message = new EmailMessage();
            message.To.Add("to@to.com");

            EmailLog emailLog = new EmailLog(DateTimeOffset.UtcNow, message);
            emailLog.EmailTo.Should().Be(message.To.Single());

            AzureEmailLogger source = new AzureEmailLogger(new AzureConfiguration(), null);
            AzureEmailLogger target = new AzureEmailLogger(new AzureConfiguration(), null);
            EmailLog result = target.FromStorage(source.ToStorage(emailLog));

            result.ID.Should().Be(emailLog.ID);
            result.WhenSentUtc.Should().Be(emailLog.WhenSentUtc);
        }

        [TestMethod]
        public async Task CanReadAndWriteEmailFromStorage()
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

            Guid logID = Guid.NewGuid();

            Mock<IStateManager<OrganizationContext>> mockOrgStateManager = new Mock<IStateManager<OrganizationContext>>();
            mockOrgStateManager.Setup(x => x.GetContext()).Returns(orgContext);

            EmailStorageService service = new EmailStorageService(mockOrgStateManager.Object, new AppConfiguration(), new AzureConfiguration());
            await service.SaveEmailAsync(message, logID);
            EmailMessage result = await service.GetEmailAsync(logID);

            result.Should().NotBeNull();
            result.From.Should().Be(message.From);
            result.HtmlBody.Should().Be(message.HtmlBody);
            result.TextBody.Should().Be(message.TextBody);
            result.Subject.Should().Be(message.Subject);
            result.To.Should().Equal(message.To);
        }
    }
}
