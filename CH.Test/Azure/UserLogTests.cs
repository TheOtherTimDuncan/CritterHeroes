using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.DataProviders.Azure;
using CritterHeroes.Web.DataProviders.Azure.Storage.Logging;
using CritterHeroes.Web.Models;
using CritterHeroes.Web.Models.Logging;
using FluentAssertions;
using Microsoft.Owin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;

namespace CH.Test.Azure
{
    [TestClass]
    public class UserLogTests : BaseTest
    {
        [TestMethod]
        public async Task CanSaveAndRetrieveUserLog()
        {
            string testUsername = "test.user";
            UserActions userAction = UserActions.PasswordLoginSuccess;

            Mock<IOwinContext> mockOwinContext = new Mock<IOwinContext>();
            mockOwinContext.Setup(x => x.Request.RemoteIpAddress).Returns("1.1.1.1");

            AzureUserLogger userLogger = new AzureUserLogger(new AzureConfiguration(), mockOwinContext.Object);
            await userLogger.LogActionAsync(userAction, testUsername);

            IEnumerable<UserLog> userLogs = await userLogger.GetUserLogAsync(DateTime.UtcNow.AddHours(-1), DateTime.UtcNow.AddHours(1));
            UserLog log = userLogs.FirstOrDefault(x => x.Username == testUsername);
            log.Should().NotBeNull();
            log.Action.Should().Be(userAction);
        }

        [TestMethod]
        public async Task CanSaveAndRetrieveUserLogWithAdditionalData()
        {
            string testUsername = "test.user";
            UserActions userAction = UserActions.PasswordLoginSuccess;

            string data = "data";

            Mock<IOwinContext> mockOwinContext = new Mock<IOwinContext>();
            mockOwinContext.Setup(x => x.Request.RemoteIpAddress).Returns("1.1.1.1");

            AzureUserLogger userLogger = new AzureUserLogger(new AzureConfiguration(), mockOwinContext.Object);
            await userLogger.LogActionAsync(userAction, testUsername, data);

            IEnumerable<UserLog> userLogs = await userLogger.GetUserLogAsync(DateTime.UtcNow.AddHours(-1), DateTime.UtcNow.AddHours(1));
            UserLog log = userLogs.FirstOrDefault(x => x.Username == testUsername && x.AdditionalData != null);
            log.Should().NotBeNull();
            log.Action.Should().Be(userAction);
            log.AdditionalData = data;
        }
    }
}
