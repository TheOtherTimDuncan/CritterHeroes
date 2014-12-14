using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Azure.Storage.Logging;
using CH.Domain.Models.Logging;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CH.Test.Azure.StorageEntityTests
{
    [TestClass]
    public class UserLogStorageEntityTests
    {
        [TestMethod]
        public void SuccessfullyMapsEntityToStorage()
        {
            UserLog userLog = new UserLog(UserActions.PasswordLoginSuccess, "username", DateTime.UtcNow);
            userLog.AdditionalData = "data";

            UserLogStorageEntity storageEntity = new UserLogStorageEntity();
            storageEntity.Should().NotBeNull();

            storageEntity.Entity = userLog;
            storageEntity.RowKey.Should().Be(userLog.ID.ToString());
            storageEntity.TableEntity["Action"].StringValue.Should().Be(UserActions.PasswordLoginSuccess.ToString());
            storageEntity.TableEntity["Username"].StringValue.Should().Be(userLog.Username);
            storageEntity.TableEntity["WhenOccurredUtc"].DateTime.Should().Be(userLog.WhenOccurredUtc);
            storageEntity.TableEntity["WhenOccurredUtc"].DateTime.Value.Kind.Should().Be(DateTimeKind.Utc);
            storageEntity.TableEntity["AdditionalData"].StringValue.Should().Be(userLog.AdditionalData);
        }

        [TestMethod]
        public void SuccessfullyMapsStorageToEntity()
        {
            UserLog userLog = new UserLog(UserActions.PasswordLoginSuccess, "username", DateTime.UtcNow);
            userLog.AdditionalData = "data";

            UserLogStorageEntity storageEntity1 = new UserLogStorageEntity();
            storageEntity1.Should().NotBeNull();
            storageEntity1.Entity = userLog;

            UserLogStorageEntity storageEntity2 = new UserLogStorageEntity();
            storageEntity2.Should().NotBeNull();
            storageEntity2.TableEntity = storageEntity1.TableEntity;

            storageEntity2.Entity.ID.Should().Be(userLog.ID);
            storageEntity2.Entity.Action.Should().Be(userLog.Action);
            storageEntity2.Entity.Username.Should().Be(userLog.Username);
            storageEntity2.Entity.WhenOccurredUtc.Should().Be(userLog.WhenOccurredUtc);
            storageEntity2.Entity.WhenOccurredUtc.Kind.Should().Be(DateTimeKind.Utc);
            storageEntity2.Entity.AdditionalData.Should().Be(userLog.AdditionalData);
        }
    }
}
