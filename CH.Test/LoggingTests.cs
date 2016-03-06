using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CritterHeroes.Web.Common.Events;
using CritterHeroes.Web.Common.Logging;
using CritterHeroes.Web.Contracts.Events;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Models.Emails;
using CritterHeroes.Web.Models.LogEvents;
using FluentAssertions;
using Microsoft.Owin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage.Table;
using Moq;
using Newtonsoft.Json;
using Serilog;
using SimpleInjector;

namespace CH.Test
{
    [TestClass]
    public class LoggingTests : BaseTest
    {
        [TestMethod]
        public void AppLogEventHandlerLogsAllLogEvents()
        {
            using (Container container = new Container())
            {

                ILogger logger = new LoggerConfiguration()
                      .MinimumLevel.Information()
                      .CreateLogger();
                container.RegisterSingleton<ILogger>(logger);

                container.RegisterCollection(typeof(IAppEventHandler<>), new[] { typeof(AppLogEventHandler<>) });
                container.Register<IAppLogEventEnricherFactory>(() => new AppLogEventEnricherFactory(container));

                Mock<IAzureService> mockAzureService = new Mock<IAzureService>();
                mockAzureService.Setup(x => x.GetLoggingKey()).Returns("partitionkey");
                container.Register<IAzureService>(() => mockAzureService.Object);

                List<DynamicTableEntity> entities = new List<DynamicTableEntity>();
                mockAzureService.Setup(x => x.ExecuteTableOperation(It.IsAny<string>(), It.IsAny<TableOperation>())).Returns((string tableName, TableOperation tableOperation) =>
                {
                    entities.Add(GetNonPublicPropertyValue<DynamicTableEntity>(tableOperation, "Entity"));
                    return new TableResult();
                });

                AppEventPublisher publisher = new AppEventPublisher(container);

                CritterLogEvent critterEvent = CritterLogEvent.Action("critter");
                publisher.Publish(critterEvent);

                UserLogEvent userEvent = UserLogEvent.Action("user");
                publisher.Publish(userEvent);

                entities.Should().HaveCount(2);

                entities.Any(x => x.Properties["Category"].StringValue == critterEvent.Category).Should().BeTrue();
                entities.Any(x => x.Properties["Category"].StringValue == userEvent.Category).Should().BeTrue();
            }
        }

        [TestMethod]
        public void AppLogEventHandlerIncludesEventContextWhenLogging()
        {
            string category = "category";
            string test = "test";

            TestContext testContext = new TestContext()
            {
                TestValue = 99
            };

            using (Container container = new Container())
            {
                ILogger logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .CreateLogger();
                container.RegisterSingleton<ILogger>(logger);

                container.RegisterCollection(typeof(IAppEventHandler<>), new[] { typeof(AppLogEventHandler<>) });
                container.Register<IAppLogEventEnricherFactory>(() => new AppLogEventEnricherFactory(container));

                Mock<IAzureService> mockAzureService = new Mock<IAzureService>();
                mockAzureService.Setup(x => x.GetLoggingKey()).Returns("partitionkey");
                container.Register<IAzureService>(() => mockAzureService.Object);

                DynamicTableEntity entity = null;
                mockAzureService.Setup(x => x.ExecuteTableOperation(It.IsAny<string>(), It.IsAny<TableOperation>())).Returns((string tableName, TableOperation tableOperation) =>
                {
                    entity = GetNonPublicPropertyValue<DynamicTableEntity>(tableOperation, "Entity");
                    return new TableResult();
                });

                AppEventPublisher publisher = new AppEventPublisher(container);

                AppLogEvent logEvent = new AppLogEvent(testContext, category, Serilog.Events.LogEventLevel.Information, "This is a {Test}", test);
                publisher.Publish(logEvent);

                entity.Should().NotBeNull();
                entity.Properties[nameof(TestContext.TestValue)].Int32Value.Should().Be(testContext.TestValue);
            }
        }

        [TestMethod]
        public void AppLogEventHandlerIncludesEnrichmentWhenLogging()
        {
            string test = "test";
            string category = "category";
            string enrichment = "enrichment";


            using (Container container = new Container())
            {
                ILogger logger = new LoggerConfiguration()
                    .MinimumLevel.Information()
                    .CreateLogger();
                container.RegisterSingleton<ILogger>(logger);

                container.RegisterCollection(typeof(IAppEventHandler<>), new[] { typeof(AppLogEventHandler<>) });
                container.Register<IAppLogEventEnricherFactory>(() => new AppLogEventEnricherFactory(container));

                Mock<IAzureService> mockAzureService = new Mock<IAzureService>();
                mockAzureService.Setup(x => x.GetLoggingKey()).Returns("partitionkey");
                container.Register<IAzureService>(() => mockAzureService.Object);

                DynamicTableEntity entity = null;
                mockAzureService.Setup(x => x.ExecuteTableOperation(It.IsAny<string>(), It.IsAny<TableOperation>())).Returns((string tableName, TableOperation tableOperation) =>
                {
                    entity = GetNonPublicPropertyValue<DynamicTableEntity>(tableOperation, "Entity");
                    return new TableResult();
                });

                Mock<IAppLogEventEnricher<AppLogEvent>> mockEnricher = new Mock<IAppLogEventEnricher<AppLogEvent>>();
                mockEnricher.Setup(x => x.Enrich(It.IsAny<ILogger>(), It.IsAny<AppLogEvent>())).Returns((ILogger enrichLogger, AppLogEvent enrichLogEvent) =>
                {
                    return enrichLogger.ForContext(nameof(enrichment), enrichment);
                });
                container.Register(() => mockEnricher.Object);

                AppEventPublisher publisher = new AppEventPublisher(container);

                AppLogEvent logEvent = new AppLogEvent(category, Serilog.Events.LogEventLevel.Information, "This is a {Test}", test);
                publisher.Publish(logEvent);

                entity.Should().NotBeNull();
                entity.Properties[nameof(enrichment)].StringValue.Should().Be(enrichment);
            }
        }

        [TestMethod]
        public void CanSubscribeToPublishedEvents()
        {
            using (Container container = new Container())
            {
                Mock<IAppEventHandler<CritterLogEvent>> mockCritterLogEventHandler = new Mock<IAppEventHandler<CritterLogEvent>>();
                Registration registration1 = Lifestyle.Transient.CreateRegistration(typeof(IAppEventHandler<CritterLogEvent>), () => mockCritterLogEventHandler.Object, container);

                Mock<IAppEventHandler<UserLogEvent>> mockUserLogEventHandler = new Mock<IAppEventHandler<UserLogEvent>>();
                Registration registration2 = Lifestyle.Transient.CreateRegistration(typeof(IAppEventHandler<UserLogEvent>), () => mockUserLogEventHandler.Object, container);

                container.RegisterCollection(typeof(IAppEventHandler<>), new[] { registration1, registration2 });

                CritterLogEvent critterEvent = CritterLogEvent.Action("critter");

                UserLogEvent userEvent = UserLogEvent.Action("user");

                AppEventPublisher publisher = new AppEventPublisher(container);

                bool isCritterPublished = false;
                bool isUserPublished = true;

                publisher.Subscribe((CritterLogEvent logEvent) => isCritterPublished = true);
                publisher.Subscribe((UserLogEvent logEvent) => isUserPublished = true);

                publisher.Publish(userEvent);
                publisher.Publish(critterEvent);

                isCritterPublished.Should().BeTrue();
                isUserPublished.Should().BeTrue();
            }
        }

        [TestMethod]
        public void RescueGroupsLogEventLogsRescueGroupsRequest()
        {
            string url = "url";
            string request = "request";
            string response = "response";
            HttpStatusCode statusCode = HttpStatusCode.OK;

            RescueGroupsLogEvent logEvent = RescueGroupsLogEvent.Create(url, request, response, statusCode);

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

            CritterLogEvent logEvent = CritterLogEvent.Action("Changed critter name from {From} to {To}", oldValue, newValue);

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

            CritterLogEvent logEvent = CritterLogEvent.Error("Changed critter name from {From} to {To}", oldValue, newValue);

            logEvent.Level.Should().Be(Serilog.Events.LogEventLevel.Error);
            logEvent.Category.Should().Be(LogEventCategory.Critter);
            logEvent.MessageValues.Should().Contain(oldValue);
            logEvent.MessageValues.Should().Contain(newValue);
        }

        [TestMethod]
        public void UserLogEventLogsUserAction()
        {
            string username = "username";

            UserLogEvent logEvent = UserLogEvent.Action("{Username} logged in", username);

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

            HistoryLogEvent logEvent = HistoryLogEvent.Create(entityID, entityName, before, after);

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

            HistoryLogEvent logEvent = HistoryLogEvent.Create(entityID, entityName, before, after);

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

            UserLogEvent logEvent = UserLogEvent.Action("{Username} logged in", "username");

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

            EmailLogEvent logEvent = EmailLogEvent.Create(emailID, email);

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
