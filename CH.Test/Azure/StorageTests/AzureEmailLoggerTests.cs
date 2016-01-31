using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.DataProviders.Azure.Storage.Logging;
using CritterHeroes.Web.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using Moq;
using Newtonsoft.Json;
using Serilog.Events;

namespace CH.Test.Azure.StorageTests
{
    [TestClass]
    public class AzureEmailLoggerTests : BaseTest
    {
        [TestMethod]
        public async Task CanSaveAndRetrieveEmailLog()
        {
            TestEmailData testData = new TestEmailData()
            {
                Message = "message"
            };

            bool isPrivate = true;

            string email1 = "to1@to1.com";
            string email2 = "to2@to2.com";

            EmailModel email = new EmailModel()
            {
                From = "from@from.com",
                To = new[] { email1, email2 },
                EmailData = testData
            };

            string emailData = null;
            DynamicTableEntity tableEntity = null;

            Mock<IAzureService> mockAzureService = new Mock<IAzureService>();
            mockAzureService.Setup(x => x.GetLoggingKey()).Returns("partitionkey");
            mockAzureService.Setup(x => x.ExecuteTableOperation(It.IsAny<string>(), It.IsAny<TableOperation>())).Returns((string tableName, TableOperation tableOperation) =>
            {
                tableEntity = GetNonPublicPropertyValue<DynamicTableEntity>(tableOperation, "Entity");
                return new TableResult();
            });

            mockAzureService.Setup(x => x.UploadBlobAsync(It.IsAny<string>(), isPrivate, It.IsAny<string>())).Returns((string path, bool callbackIsPrivate, string content) =>
            {
                emailData = content;
                CloudBlockBlob blob = new CloudBlockBlob(new Uri("http://localhost/container"));
                return Task.FromResult(blob);
            });
            mockAzureService.Setup(x => x.DownloadBlobAsync(It.IsAny<string>(), isPrivate)).Returns(() => Task.FromResult(emailData));

            AzureEmailLogger logger = new AzureEmailLogger(mockAzureService.Object);
            await logger.LogEmailAsync(email);

            tableEntity.Properties[nameof(LogEvent.Level)].StringValue.Should().Be(LogEventLevel.Information.ToString());
            tableEntity.Properties["From"].StringValue.Should().Be(email.From);
            tableEntity.Properties["To"].StringValue.Should().Be("[\"" + email1 + "\", \"" + email2 + "\"]");
            tableEntity.Properties["Message"].StringValue.Should().Be("Sent email from \"" + email.From + "\" to [\"" + email1 + "\", \"" + email2 + "\"]");
            tableEntity.Properties["Category"].StringValue.Should().Be(LogCategory.Email);

            TestEmailData resultData = JsonConvert.DeserializeObject<TestEmailData>(emailData);
            resultData.Message.Should().Be(testData.Message);

            mockAzureService.Verify(x => x.ExecuteTableOperation(It.IsAny<string>(), It.IsAny<TableOperation>()), Times.Once);
            mockAzureService.Verify(x => x.UploadBlobAsync(It.IsAny<string>(), isPrivate, It.IsAny<string>()), Times.Once);
        }

        public class TestEmailData : BaseEmailData
        {
            public string Message
            {
                get;
                set;
            }
        }
    }
}
