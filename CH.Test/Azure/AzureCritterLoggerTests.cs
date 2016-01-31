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
    public class AzureCritterLoggerTests : BaseTest
    {
        [TestMethod]
        public void LogsCritterAction()
        {
            Mock<IAzureService> mockAzureService = new Mock<IAzureService>();
            mockAzureService.Setup(x => x.GetLoggingKey()).Returns("partitionkey");

            DynamicTableEntity tableEntity = null;
            mockAzureService.Setup(x => x.ExecuteTableOperation(It.IsAny<string>(), It.IsAny<TableOperation>())).Returns((string tableName, TableOperation tableOperation) =>
            {
                tableEntity = GetNonPublicPropertyValue<DynamicTableEntity>(tableOperation, "Entity");
                return new TableResult();
            });

            AzureCritterLogger logger = new AzureCritterLogger(mockAzureService.Object);
            logger.LogAction("Changed critter name from {From} to {To}", "old", "new");

            tableEntity.Properties[nameof(LogEvent.Level)].StringValue.Should().Be(LogEventLevel.Information.ToString());
            tableEntity.Properties["Message"].StringValue.Should().Be("Changed critter name from \"old\" to \"new\"");
            tableEntity.Properties["Category"].StringValue.Should().Be(LogCategory.Critter);

            logger.Messages.Should().Contain("Changed critter name from \"old\" to \"new\"");

            mockAzureService.Verify(x => x.ExecuteTableOperation(It.IsAny<string>(), It.IsAny<TableOperation>()), Times.Once);
        }
    }
}
