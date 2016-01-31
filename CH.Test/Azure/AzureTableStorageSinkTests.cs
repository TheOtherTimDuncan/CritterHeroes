using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.DataProviders.Azure.Storage;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage.Table;
using Moq;
using Serilog;
using Serilog.Events;

namespace CH.Test.Azure
{
    [TestClass]
    public class AzureTableStorageSinkTests : BaseTest
    {
        private Mock<IAzureService> mockAzureService;

        [TestInitialize]
        public void InitializeTest()
        {
            mockAzureService = new Mock<IAzureService>();
            mockAzureService.Setup(x => x.GetLoggingKey()).Returns("partitionkey");
            mockAzureService.Setup(x => x.GetLoggingKey(It.IsAny<DateTime>())).Returns("partitionkey");
        }

        [TestMethod]
        public void MapsLogEventToTableEntity()
        {
            DynamicTableEntity tableEntity = null;
            mockAzureService.Setup(x => x.ExecuteTableOperation(It.IsAny<string>(), It.IsAny<TableOperation>())).Returns((string tableName, TableOperation tableOperation) =>
            {
                tableEntity = GetNonPublicPropertyValue<DynamicTableEntity>(tableOperation, "Entity");
                return new TableResult();
            });

            Log.Logger = new LoggerConfiguration()
                .WriteTo
                .AzureTableStorage(mockAzureService.Object, "Log")
                .MinimumLevel.Debug()
                .CreateLogger();

            Log.Debug("This is a {Test}", "test");

            tableEntity.Should().NotBeNull();
            tableEntity.Timestamp.Should().BeCloseTo(DateTimeOffset.UtcNow, precision: 100);
            tableEntity.Properties[nameof(LogEvent.Level)].StringValue.Should().Be("Debug");
            tableEntity.Properties["Message"].StringValue.Should().Be("This is a \"test\"");
            tableEntity.Properties["Test"].StringValue.Should().Be("\"test\"");

            mockAzureService.Verify(x => x.ExecuteTableOperation(It.IsAny<string>(), It.IsAny<TableOperation>()), Times.Once);
        }
    }
}
