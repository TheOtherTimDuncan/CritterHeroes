using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.DataProviders.Azure.Logging;
using CritterHeroes.Web.Models.LogEvents;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage.Table;
using Moq;

namespace CH.Test
{
    [TestClass]
    public class LoggingTests : BaseTest
    {
        [TestMethod]
        public void LogEventWritesEventToLogger()
        {
            Mock<IAzureService> mockAzureService = new Mock<IAzureService>();
            mockAzureService.Setup(x => x.GetLoggingKey()).Returns("partitionkey");

            DynamicTableEntity tableEntity = null;
            mockAzureService.Setup(x => x.ExecuteTableOperation(It.IsAny<string>(), It.IsAny<TableOperation>())).Returns((string tableName, TableOperation tableOperation) =>
            {
                tableEntity = GetNonPublicPropertyValue<DynamicTableEntity>(tableOperation, "Entity");
                return new TableResult();
            });

            string test = "test";
            string category = "category";

            LogEvent logEvent = new LogEvent(category, Serilog.Events.LogEventLevel.Information, "This is a {Test}", test);

            AzureAppLogger logger = new AzureAppLogger(mockAzureService.Object);
            logger.LogEvent(logEvent);

            tableEntity.Should().NotBeNull();
            tableEntity.Timestamp.Should().BeCloseTo(DateTimeOffset.UtcNow, precision: 100);
            tableEntity.Properties[nameof(LogEvent.Level)].StringValue.Should().Be("Information");
            tableEntity.Properties["Category"].StringValue.Should().Be(category);
            tableEntity.Properties["Message"].StringValue.Should().Be("This is a \"test\"");
            tableEntity.Properties["Test"].StringValue.Should().Be(test);

            logger.Messages.Should().Equal(new[] { "This is a \"test\"" });

            mockAzureService.Verify(x => x.ExecuteTableOperation(It.IsAny<string>(), It.IsAny<TableOperation>()), Times.Once);
        }

        [TestMethod]
        public void RescueGroupsLogEventLogsRescueGroupsRequest()
        {
            string url = "url";
            string request = "request";
            string response = "response";
            HttpStatusCode statusCode = HttpStatusCode.OK;

            RescueGroupsLogEvent logEvent = RescueGroupsLogEvent.LogRequest(url, request, response, statusCode);

            logEvent.Level.Should().Be(Serilog.Events.LogEventLevel.Information);
            logEvent.Category.Should().Be(LogEventCategory.RescueGroups);
            logEvent.MessageValues.Should().Contain(url);
            logEvent.MessageValues.Should().Contain(request);
            logEvent.MessageValues.Should().Contain(response);
            logEvent.MessageValues.Should().Contain(statusCode);
        }

        [TestMethod]
        public void CritterLogEventLogsCritterAction()
        {
            string oldValue = "old";
            string newValue = "new";

            CritterLogEvent logEvent = CritterLogEvent.LogAction("Changed critter name from {From} to {To}", oldValue, newValue);

            logEvent.Level.Should().Be(Serilog.Events.LogEventLevel.Information);
            logEvent.Category.Should().Be(LogEventCategory.Critter);
            logEvent.MessageValues.Should().Contain(oldValue);
            logEvent.MessageValues.Should().Contain(newValue);
        }

        [TestMethod]
        public void CritterLogEventLogsCritterError()
        {
            string oldValue = "old";
            string newValue = "new";

            CritterLogEvent logEvent = CritterLogEvent.LogError("Changed critter name from {From} to {To}", oldValue, newValue);

            logEvent.Level.Should().Be(Serilog.Events.LogEventLevel.Error);
            logEvent.Category.Should().Be(LogEventCategory.Critter);
            logEvent.MessageValues.Should().Contain(oldValue);
            logEvent.MessageValues.Should().Contain(newValue);
        }
    }
}
