using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AR.Domain.Models;
using AR.RescueGroups;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TOTD.Utility.StringHelpers;

namespace AR.Test.RescueGroups
{
    [TestClass]
    public class StorageTests
    {
        [TestMethod]
        public void GetsConfigurationFromConfigurationFileByDefalt()
        {
            RescueGroupsStorage storage = new RescueGroupsStorage();
            Assert.IsFalse(storage.Url.IsNullOrEmpty());
            Assert.IsFalse(storage.APIKey.IsNullOrEmpty());
            Assert.IsFalse(storage.AccountNumber.IsNullOrEmpty());
            Assert.IsFalse(storage.Username.IsNullOrEmpty());
            Assert.IsFalse(storage.Password.IsNullOrEmpty());
        }

        //[TestMethod]
        public async Task TestGet()
        {
            RescueGroupsStorage storage = new RescueGroupsStorage();
            IEnumerable<AnimalStatus> result = await storage.GetAllAsync<AnimalStatus>();
        }
    }
}
