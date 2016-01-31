using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.DataProviders.Azure.Storage.Logging;
using FluentAssertions;
using Microsoft.Owin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage.Table;
using Moq;
using Serilog.Events;

namespace CH.Test.Azure.StorageEntityTests
{
    [TestClass]
    public class AzureUserLoggerTests : BaseTest
    {
        [TestMethod]
        public void LogsUserAction()
        {
            string ipAddress = "1.1.1.1";
            string username = "username";

            Mock<IAzureService> mockAzureService = new Mock<IAzureService>();
            mockAzureService.Setup(x => x.GetLoggingKey()).Returns("partitionkey");

            DynamicTableEntity tableEntity = null;
            mockAzureService.Setup(x => x.ExecuteTableOperation(It.IsAny<string>(), It.IsAny<TableOperation>())).Returns((string tableName, TableOperation tableOperation) =>
            {
                tableEntity = GetNonPublicPropertyValue<DynamicTableEntity>(tableOperation, "Entity");
                return new TableResult();
            });

            Mock<IOwinContext> mockOwinContext = new Mock<IOwinContext>();
            mockOwinContext.Setup(x => x.Request.RemoteIpAddress).Returns(ipAddress);

            AzureUserLogger logger = new AzureUserLogger(mockAzureService.Object, mockOwinContext.Object);
            logger.LogAction("{Username} logged in", username);

            tableEntity.Properties[nameof(LogEvent.Level)].StringValue.Should().Be(LogEventLevel.Information.ToString());
            tableEntity.Properties["IPAddress"].StringValue.Should().Be(ipAddress);
            tableEntity.Properties["Message"].StringValue.Should().Be("\"username\" logged in");
            tableEntity.Properties["Username"].StringValue.Should().Be(username);
            tableEntity.Properties["Category"].StringValue.Should().Be(LogCategory.User);

            mockAzureService.Verify(x => x.ExecuteTableOperation(It.IsAny<string>(), It.IsAny<TableOperation>()), Times.Once);
        }

        [TestMethod]
        public void IncludesErrorsInLog()
        {
            string ipAddress = "1.1.1.1";
            string username = "username";
            string error1 = "error1";
            string error2 = "error2";
            IEnumerable<string> errors = new[] { error1, error2 };

            Mock<IAzureService> mockAzureService = new Mock<IAzureService>();
            mockAzureService.Setup(x => x.GetLoggingKey()).Returns("partitionkey");

            DynamicTableEntity tableEntity = null;
            mockAzureService.Setup(x => x.ExecuteTableOperation(It.IsAny<string>(), It.IsAny<TableOperation>())).Returns((string tableName, TableOperation tableOperation) =>
            {
                tableEntity = GetNonPublicPropertyValue<DynamicTableEntity>(tableOperation, "Entity");
                return new TableResult();
            });

            Mock<IOwinContext> mockOwinContext = new Mock<IOwinContext>();
            mockOwinContext.Setup(x => x.Request.RemoteIpAddress).Returns(ipAddress);

            AzureUserLogger logger = new AzureUserLogger(mockAzureService.Object, mockOwinContext.Object);
            logger.LogError("{Username} logged in", errors, username);

            tableEntity.Properties[nameof(LogEvent.Level)].StringValue.Should().Be(LogEventLevel.Error.ToString());
            tableEntity.Properties["Errors"].StringValue.Should().Be("[\"" + error1 + "\", \"" + error2 + "\"]");

            mockAzureService.Verify(x => x.ExecuteTableOperation(It.IsAny<string>(), It.IsAny<TableOperation>()), Times.Once);
        }
    }
}
