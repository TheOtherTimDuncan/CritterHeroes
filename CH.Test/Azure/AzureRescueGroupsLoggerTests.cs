using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.DataProviders.Azure.Logging;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage.Table;
using Moq;
using Serilog.Events;

namespace CH.Test.Azure
{
    [TestClass]
    public class AzureRescueGroupsLoggerTests : BaseTest
    {
        [TestMethod]
        public void LogsServiceRequestAndResponse()
        {
            string url = "url";
            string request = "request";
            string response = "response";
            int statusCode = 200;

            Mock<IAzureService> mockAzureService = new Mock<IAzureService>();
            mockAzureService.Setup(x => x.GetLoggingKey()).Returns("partitionkey");

            DynamicTableEntity tableEntity = null;
            mockAzureService.Setup(x => x.ExecuteTableOperation(It.IsAny<string>(), It.IsAny<TableOperation>())).Returns((string tableName, TableOperation tableOperation) =>
            {
                tableEntity = GetNonPublicPropertyValue<DynamicTableEntity>(tableOperation, "Entity");
                return new TableResult();
            });

            AzureRescueGroupsLogger logger = new AzureRescueGroupsLogger(mockAzureService.Object);
            logger.LogRequest(url, request, response, statusCode);

            tableEntity.Properties[nameof(LogEvent.Level)].StringValue.Should().Be(LogEventLevel.Information.ToString());
            tableEntity.Properties["Category"].StringValue.Should().Be(LogCategory.RescueGroups);
            tableEntity.Properties["Message"].StringValue.Should().Be("Sent \"request\" to \"url\" and received status code 200 with \"response\"");
            tableEntity.Properties["Request"].StringValue.Should().Be(request);
            tableEntity.Properties["Response"].StringValue.Should().Be(response);
            tableEntity.Properties["Url"].StringValue.Should().Be(url);
            tableEntity.Properties["StatusCode"].Int32Value.Should().Be(statusCode);

            mockAzureService.Verify(x => x.ExecuteTableOperation(It.IsAny<string>(), It.IsAny<TableOperation>()), Times.Once);
        }
    }
}
