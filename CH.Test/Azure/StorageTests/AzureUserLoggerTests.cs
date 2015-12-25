using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.DataProviders.Azure.Storage.Logging;
using CritterHeroes.Web.Models.Logging;
using FluentAssertions;
using Microsoft.Owin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage.Table;
using Moq;

namespace CH.Test.Azure.StorageEntityTests
{
    [TestClass]
    public class AzureUserLoggerTests : BaseTest
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
        public void SuccessfullyMapsEntityToAndFromStorage()
        {
            UserLog userLog = new UserLog(UserActions.PasswordLoginSuccess, "username", DateTimeOffset.UtcNow);
            userLog.IPAddress = "1.1.1.1";
            userLog.ThreadID = Thread.CurrentThread.ManagedThreadId;
            userLog.AdditionalData = "data";

            AzureUserLogger source = new AzureUserLogger(mockAzureService.Object, null);
            AzureUserLogger target = new AzureUserLogger(mockAzureService.Object, null);
            UserLog result = target.FromStorage(source.ToStorage(userLog));

            result.ID.Should().Be(userLog.ID);
            result.Action.Should().Be(userLog.Action);
            result.Username.Should().Be(userLog.Username);
            result.WhenOccurredUtc.Should().Be(userLog.WhenOccurredUtc);
            result.IPAddress.Should().Be(userLog.IPAddress);
            result.ThreadID.Should().HaveValue();
            result.AdditionalData.Should().Be(userLog.AdditionalData);
        }

        [TestMethod]
        public async Task CanSaveAndRetrieveUserLog()
        {
            string testUsername = "test.user";
            UserActions userAction = UserActions.PasswordLoginSuccess;

            Mock<IOwinContext> mockOwinContext = new Mock<IOwinContext>();
            mockOwinContext.Setup(x => x.Request.RemoteIpAddress).Returns("1.1.1.1");

            DynamicTableEntity tableEntity = null;
            mockAzureService.Setup(x => x.ExecuteTableOperationAsync(It.IsAny<string>(), It.IsAny<TableOperation>())).Returns((string tableName, TableOperation tableOperation) =>
            {
                tableEntity = GetNonPublicPropertyValue<DynamicTableEntity>(tableOperation, "Entity");
                return Task.FromResult(new TableResult());
            });
            mockAzureService.Setup(x => x.CreateTableQuery<DynamicTableEntity>(It.IsAny<string>())).Returns((string tableName) =>
            {
                return Task.FromResult(new[] { tableEntity }.AsQueryable());
            });

            AzureUserLogger userLogger = new AzureUserLogger(mockAzureService.Object, mockOwinContext.Object);
            await userLogger.LogActionAsync(userAction, testUsername);

            IEnumerable<UserLog> userLogs = await userLogger.GetUserLogAsync(DateTime.UtcNow.AddHours(-1), DateTime.UtcNow.AddHours(1));
            UserLog log = userLogs.FirstOrDefault(x => x.Username == testUsername);
            log.Should().NotBeNull();
            log.Action.Should().Be(userAction);

            mockAzureService.Verify(x => x.ExecuteTableOperationAsync(It.IsAny<string>(), It.IsAny<TableOperation>()), Times.Once);
        }

        [TestMethod]
        public async Task CanSaveAndRetrieveUserLogWithAdditionalData()
        {
            string testUsername = "test.user";
            UserActions userAction = UserActions.PasswordLoginSuccess;

            string data = "data";

            Mock<IOwinContext> mockOwinContext = new Mock<IOwinContext>();
            mockOwinContext.Setup(x => x.Request.RemoteIpAddress).Returns("1.1.1.1");

            DynamicTableEntity tableEntity = null;
            mockAzureService.Setup(x => x.ExecuteTableOperationAsync(It.IsAny<string>(), It.IsAny<TableOperation>())).Returns((string tableName, TableOperation tableOperation) =>
            {
                tableEntity = GetNonPublicPropertyValue<DynamicTableEntity>(tableOperation, "Entity");
                return Task.FromResult(new TableResult());
            });
            mockAzureService.Setup(x => x.CreateTableQuery<DynamicTableEntity>(It.IsAny<string>())).Returns((string tableName) =>
            {
                return Task.FromResult(new[] { tableEntity }.AsQueryable());
            });

            AzureUserLogger userLogger = new AzureUserLogger(mockAzureService.Object, mockOwinContext.Object);
            await userLogger.LogActionAsync(userAction, testUsername, data);

            IEnumerable<UserLog> userLogs = await userLogger.GetUserLogAsync(DateTime.UtcNow.AddHours(-1), DateTime.UtcNow.AddHours(1));
            UserLog log = userLogs.FirstOrDefault(x => x.Username == testUsername && x.AdditionalData != null);
            log.Should().NotBeNull();
            log.Action.Should().Be(userAction);
            log.AdditionalData = data;
        }
    }
}
