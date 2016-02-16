using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.DataProviders.Azure.Logging;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage.Table;
using Moq;
using Newtonsoft.Json;
using Serilog.Events;

namespace CH.Test.Azure
{
    [TestClass]
    public class AzureHistoryLoggerTests : BaseTest
    {
        [TestMethod]
        public void LogsHistory()
        {
            Mock<IAzureService> mockAzureService = new Mock<IAzureService>();
            mockAzureService.Setup(x => x.GetLoggingKey()).Returns("partitionkey");

            DynamicTableEntity tableEntity = null;
            mockAzureService.Setup(x => x.ExecuteTableOperation(It.IsAny<string>(), It.IsAny<TableOperation>())).Returns((string tableName, TableOperation tableOperation) =>
            {
                tableEntity = GetNonPublicPropertyValue<DynamicTableEntity>(tableOperation, "Entity");
                return new TableResult();
            });

            int entityID = 99;
            string entityName = "entity";

            Dictionary<string, object> before = new Dictionary<string, object>();
            before["test1"] = 1;

            Dictionary<string, object> after = new Dictionary<string, object>();
            after["test2"] = 1;

            string jsonBefore = JsonConvert.SerializeObject(before);
            string jsonAfter = JsonConvert.SerializeObject(after);

            AzureHistoryLogger logger = new AzureHistoryLogger(mockAzureService.Object);
            logger.LogHistory(entityID, entityName, before, after);

            tableEntity.Properties[nameof(LogEvent.Level)].StringValue.Should().Be(LogEventLevel.Information.ToString());
            tableEntity.Properties["Category"].StringValue.Should().Be(LogCategory.History);
            tableEntity.Properties["Before"].StringValue.Should().Be(jsonBefore);
            tableEntity.Properties["After"].StringValue.Should().Be(jsonAfter);
            tableEntity.Properties["Message"].StringValue.Should().Be($"Changed entity {entityID} - \"{entityName}\"");

            mockAzureService.Verify(x => x.ExecuteTableOperation(It.IsAny<string>(), It.IsAny<TableOperation>()), Times.Once);
        }
    }
}
