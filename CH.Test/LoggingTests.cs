using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CritterHeroes.Web.Common.Logging;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.DataProviders.Azure.Logging;
using CritterHeroes.Web.Models.Emails;
using CritterHeroes.Web.Models.LogEvents;
using FluentAssertions;
using Microsoft.Owin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage.Table;
using Moq;
using Newtonsoft.Json;
using Serilog;

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

            Mock<IAppLogEventEnricherFactory> mockEnricherFactory = new Mock<IAppLogEventEnricherFactory>();

            string test = "test";
            string category = "category";

            AppLogEvent logEvent = new AppLogEvent(category, Serilog.Events.LogEventLevel.Information, "This is a {Test}", test);

            AzureAppLogger logger = new AzureAppLogger(mockAzureService.Object, mockEnricherFactory.Object);
            logger.LogEvent(logEvent);

            tableEntity.Should().NotBeNull();
            tableEntity.Timestamp.Should().BeCloseTo(DateTimeOffset.UtcNow, precision: 100);
            tableEntity.Properties[nameof(AppLogEvent.Level)].StringValue.Should().Be("Information");
            tableEntity.Properties["Category"].StringValue.Should().Be(category);
            tableEntity.Properties["Message"].StringValue.Should().Be("This is a \"test\"");
            tableEntity.Properties["Test"].StringValue.Should().Be(test);

            logger.Messages.Should().Equal(new[] { "This is a \"test\"" });

            mockAzureService.Verify(x => x.ExecuteTableOperation(It.IsAny<string>(), It.IsAny<TableOperation>()), Times.Once);
        }

        [TestMethod]
        public void LogEventWritesEventAndEventContextToLogger()
        {
            Mock<IAzureService> mockAzureService = new Mock<IAzureService>();
            mockAzureService.Setup(x => x.GetLoggingKey()).Returns("partitionkey");

            DynamicTableEntity tableEntity = null;
            mockAzureService.Setup(x => x.ExecuteTableOperation(It.IsAny<string>(), It.IsAny<TableOperation>())).Returns((string tableName, TableOperation tableOperation) =>
            {
                tableEntity = GetNonPublicPropertyValue<DynamicTableEntity>(tableOperation, "Entity");
                return new TableResult();
            });

            Mock<IAppLogEventEnricherFactory> mockEnricherFactory = new Mock<IAppLogEventEnricherFactory>();

            string test = "test";
            string category = "category";

            TestContext testContext = new TestContext()
            {
                TestValue = 99
            };

            AppLogEvent logEvent = new AppLogEvent(testContext, category, Serilog.Events.LogEventLevel.Information, "This is a {Test}", test);

            AzureAppLogger logger = new AzureAppLogger(mockAzureService.Object, mockEnricherFactory.Object);
            logger.LogEvent(logEvent);

            tableEntity.Should().NotBeNull();
            tableEntity.Timestamp.Should().BeCloseTo(DateTimeOffset.UtcNow, precision: 100);
            tableEntity.Properties[nameof(AppLogEvent.Level)].StringValue.Should().Be("Information");
            tableEntity.Properties["Category"].StringValue.Should().Be(category);
            tableEntity.Properties["Message"].StringValue.Should().Be("This is a \"test\"");
            tableEntity.Properties["Test"].StringValue.Should().Be(test);
            tableEntity.Properties[nameof(TestContext.TestValue)].Int32Value.Should().Be(testContext.TestValue);

            logger.Messages.Should().Equal(new[] { "This is a \"test\"" });

            mockAzureService.Verify(x => x.ExecuteTableOperation(It.IsAny<string>(), It.IsAny<TableOperation>()), Times.Once);
        }

        [TestMethod]
        public void LogEventWritesEventAndEWithEnrichmentToLogger()
        {
            string test = "test";
            string category = "category";
            string enrichment = "enrichment";

            Mock<IAzureService> mockAzureService = new Mock<IAzureService>();
            mockAzureService.Setup(x => x.GetLoggingKey()).Returns("partitionkey");

            DynamicTableEntity tableEntity = null;
            mockAzureService.Setup(x => x.ExecuteTableOperation(It.IsAny<string>(), It.IsAny<TableOperation>())).Returns((string tableName, TableOperation tableOperation) =>
            {
                tableEntity = GetNonPublicPropertyValue<DynamicTableEntity>(tableOperation, "Entity");
                return new TableResult();
            });

            Mock<IAppLogEventEnricher<AppLogEvent>> mockEnricher = new Mock<IAppLogEventEnricher<AppLogEvent>>();
            mockEnricher.Setup(x => x.Enrich(It.IsAny<ILogger>(), It.IsAny<AppLogEvent>())).Returns((ILogger enrichLogger, AppLogEvent enrichLogEvent) =>
            {
                return enrichLogger.ForContext(nameof(enrichment), enrichment);
            });

            Mock<IAppLogEventEnricherFactory> mockEnricherFactory = new Mock<IAppLogEventEnricherFactory>();
            mockEnricherFactory.Setup(x => x.GetEnricher(It.IsAny<AppLogEvent>())).Returns(mockEnricher.Object);

            AppLogEvent logEvent = new AppLogEvent(category, Serilog.Events.LogEventLevel.Information, "This is a {Test}", test);

            AzureAppLogger logger = new AzureAppLogger(mockAzureService.Object, mockEnricherFactory.Object);
            logger.LogEvent(logEvent);

            tableEntity.Should().NotBeNull();
            tableEntity.Timestamp.Should().BeCloseTo(DateTimeOffset.UtcNow, precision: 100);
            tableEntity.Properties[nameof(AppLogEvent.Level)].StringValue.Should().Be("Information");
            tableEntity.Properties["Category"].StringValue.Should().Be(category);
            tableEntity.Properties["Message"].StringValue.Should().Be("This is a \"test\"");
            tableEntity.Properties["Test"].StringValue.Should().Be(test);
            tableEntity.Properties[nameof(enrichment)].StringValue.Should().Be(enrichment);

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
            logEvent.MessageValues.Should().Contain(statusCode);

            logEvent.Context.Should().BeOfType<RescueGroupsLogEvent.RescueGroupsContext>();
            RescueGroupsLogEvent.RescueGroupsContext context = logEvent.Context as RescueGroupsLogEvent.RescueGroupsContext;
            context.Url.Should().Be(url);
            context.Request.Should().Be(request);
            context.Response.Should().Be(response);
            context.StatusCode.Should().Be(statusCode);
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

        [TestMethod]
        public void UserLogEventLogsUserAction()
        {
            string username = "username";

            UserLogEvent logEvent = UserLogEvent.LogAction("{Username} logged in", username);

            logEvent.Level.Should().Be(Serilog.Events.LogEventLevel.Information);
            logEvent.Category.Should().Be(LogEventCategory.User);
            logEvent.MessageValues.Should().Contain(username);
        }

        [TestMethod]
        public void HistoryLogEventLogsEntityHistory()
        {
            int entityID = 99;
            string entityName = "entity";

            Dictionary<string, object> before = new Dictionary<string, object>();
            before["test1"] = 1;

            Dictionary<string, object> after = new Dictionary<string, object>();
            after["test2"] = 1;

            string jsonBefore = JsonConvert.SerializeObject(before);
            string jsonAfter = JsonConvert.SerializeObject(after);

            HistoryLogEvent logEvent = HistoryLogEvent.LogHistory(entityID, entityName, before, after);

            logEvent.Level.Should().Be(Serilog.Events.LogEventLevel.Information);
            logEvent.Category.Should().Be(LogEventCategory.History);
            logEvent.MessageValues.Should().Contain(entityID);
            logEvent.MessageValues.Should().Contain(entityName);

            logEvent.Context.Should().BeOfType<HistoryLogEvent.HistoryContext>();
            HistoryLogEvent.HistoryContext context = logEvent.Context as HistoryLogEvent.HistoryContext;
            context.Before.Should().Be(jsonBefore);
            context.After.Should().Be(jsonAfter);
        }

        [TestMethod]
        public void HistoryLogEventRemovesEntityNameAfterUnderscore()
        {
            int entityID = 99;
            string entityName = "entity_test";

            Dictionary<string, object> before = new Dictionary<string, object>();
            before["test1"] = 1;

            Dictionary<string, object> after = new Dictionary<string, object>();
            after["test2"] = 1;

            HistoryLogEvent logEvent = HistoryLogEvent.LogHistory(entityID, entityName, before, after);

            logEvent.Level.Should().Be(Serilog.Events.LogEventLevel.Information);
            logEvent.MessageValues.Should().Contain("entity");
        }

        [TestMethod]
        public void UserLogEventEnricherEnrichesUserLogEvent()
        {
            string ipAddress = "1.1.1.1";

            Mock<IOwinContext> mockOwinContext = new Mock<IOwinContext>();
            mockOwinContext.Setup(x => x.Request.RemoteIpAddress).Returns(ipAddress);

            Mock<ILogger> mockLogger = new Mock<ILogger>();

            UserLogEvent logEvent = UserLogEvent.LogAction("{Username} logged in", "username");

            UserLogEventEnricher enricher = new UserLogEventEnricher(mockOwinContext.Object);
            ILogger logger = enricher.Enrich(mockLogger.Object, logEvent);

            mockLogger.Verify(x => x.ForContext("IPAddress", ipAddress, false), Times.Once);
        }

        [TestMethod]
        public void EmailLogEventLogsEmail()
        {
            Guid emailID = Guid.NewGuid();

            string[] emailTo = new[] { "to@to.com" };

            EmailModel email = new EmailModel()
            {
                To = emailTo,
                From = "from@from.com"
            };

            EmailLogEvent logEvent = EmailLogEvent.LogEmail(emailID, email);

            logEvent.Level.Should().Be(Serilog.Events.LogEventLevel.Information);
            logEvent.Category.Should().Be(LogEventCategory.Email);
            logEvent.MessageValues.Any(x => emailTo.Equals(x)).Should().BeTrue();
            logEvent.MessageValues.Should().Contain(email.From);

            logEvent.Context.Should().BeOfType<EmailLogEvent.EmailContext>();
            EmailLogEvent.EmailContext context = logEvent.Context as EmailLogEvent.EmailContext;
            context.EmailID.Should().Be(emailID);
        }

        private class TestContext
        {
            public int TestValue
            {
                get;
                set;
            }
        }
    }
}
