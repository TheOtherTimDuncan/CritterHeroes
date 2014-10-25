using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AR.Domain.Models.Data;
using AR.RescueGroups;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AR.Test.RescueGroups
{
    [TestClass]
    public class StorageTests
    {
        [TestMethod]
        public void GetsConfigurationFromConfigurationFileByDefalt()
        {
            RescueGroupsStorage storage = new RescueGroupsStorage();
            storage.Url.Should().NotBeNullOrEmpty();
            storage.APIKey.Should().NotBeNullOrEmpty();
            storage.AccountNumber.Should().NotBeNullOrEmpty();
            storage.Username.Should().NotBeNullOrEmpty();
            storage.Password.Should().NotBeNullOrEmpty();
        }
    }
}
