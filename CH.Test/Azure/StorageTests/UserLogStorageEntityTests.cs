using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using CritterHeroes.Web.DataProviders.Azure;
using CritterHeroes.Web.DataProviders.Azure.Storage.Logging;
using CritterHeroes.Web.Models.Logging;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CH.Test.Azure.StorageEntityTests
{
    [TestClass]
    public class UserLogStorageEntityTests : BaseAzureTest
    {
        [TestMethod]
        public void SuccessfullyMapsEntityToAndFromStorage()
        {
            UserLog userLog = new UserLog(UserActions.PasswordLoginSuccess, "username", DateTime.UtcNow);
            userLog.IPAddress = "1.1.1.1";
            userLog.ThreadID = Thread.CurrentThread.ManagedThreadId;
            userLog.AdditionalData = "data";
            
            AzureUserLogger source = new AzureUserLogger(new AzureConfiguration(), null);
            AzureUserLogger target = new AzureUserLogger(new AzureConfiguration(), null);
            UserLog result = target.FromStorage(source.ToStorage(userLog));

            result.ID.Should().Be(userLog.ID);
            result.Action.Should().Be(userLog.Action);
            result.Username.Should().Be(userLog.Username);
            result.WhenOccurredUtc.Should().Be(userLog.WhenOccurredUtc);
            result.WhenOccurredUtc.Kind.Should().Be(DateTimeKind.Utc);
            result.IPAddress.Should().Be(userLog.IPAddress);
            result.ThreadID.Should().HaveValue();
            result.AdditionalData.Should().Be(userLog.AdditionalData);
        }
    }
}
