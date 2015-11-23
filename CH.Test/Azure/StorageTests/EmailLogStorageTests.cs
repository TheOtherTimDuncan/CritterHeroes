using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.Proxies.Configuration;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.DataProviders.Azure;
using CritterHeroes.Web.DataProviders.Azure.Storage;
using CritterHeroes.Web.DataProviders.Azure.Storage.Logging;
using CritterHeroes.Web.Models;
using CritterHeroes.Web.Models.Logging;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;

namespace CH.Test.Azure.StorageTests
{
    [TestClass]
    public class EmailLogStorageTests : BaseAzureTest
    {
        [TestMethod]
        public async Task CanSaveAndRetrieveEmailLog()
        {
            TestEmailData testData = new TestEmailData()
            {
                Message = "message"
            };

            EmailLog emailLog = new EmailLog(testData);

            string emailData = null;

            Mock<IEmailStorageService> mockEmailStorage = new Mock<IEmailStorageService>();
            mockEmailStorage.Setup(x => x.SaveEmailAsync(emailLog.ID, It.IsAny<string>())).Returns((Guid emailID, string saveEmailData) =>
            {
                emailData = saveEmailData;
                return Task.FromResult(0);
            });
            mockEmailStorage.Setup(x => x.GetEmailAsync(emailLog.ID)).Returns(() => Task.FromResult(emailData));

            AzureEmailLogger logger = new AzureEmailLogger(new AzureConfiguration(), mockEmailStorage.Object);
            await logger.LogEmailAsync(emailLog);

            IEnumerable<EmailLog> results = await logger.GetEmailLogAsync(DateTime.UtcNow.AddHours(-1), DateTime.UtcNow.AddHours(1));

            EmailLog result = results.FirstOrDefault(x => x.ID == emailLog.ID);
            result.Should().NotBeNull();
            result.WhenCreatedUtc.Should().Be(emailLog.WhenCreatedUtc);

            TestEmailData resultData = result.ConvertEmailData<TestEmailData>();
            resultData.Message.Should().Be(testData.Message);
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

            TestEmailData testData = new TestEmailData()
            {
                Message = "message"
            };

            Guid logID = Guid.NewGuid();

            Mock<IStateManager<OrganizationContext>> mockOrgStateManager = new Mock<IStateManager<OrganizationContext>>();
            mockOrgStateManager.Setup(x => x.GetContext()).Returns(orgContext);

            EmailStorageService service = new EmailStorageService(mockOrgStateManager.Object, new AppConfiguration(), new AzureConfiguration());
            await service.SaveEmailAsync(logID, JsonConvert.SerializeObject(testData));
            string emailData = await service.GetEmailAsync(logID);
            TestEmailData result = JsonConvert.DeserializeObject<TestEmailData>(emailData);

            result.Should().NotBeNull();
            result.Message.Should().Be(testData.Message);
        }

        public class TestEmailData
        {
            public string Message
            {
                get;
                set;
            }
        }
    }
}
