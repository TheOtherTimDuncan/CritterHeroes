using System;
using System.Collections.Generic;
using System.Linq;
using CH.Test.Mocks;
using CritterHeroes.Web.DataProviders.RescueGroups;
using CritterHeroes.Web.DataProviders.RescueGroups.Configuration;
using CritterHeroes.Web.DataProviders.RescueGroups.Storage;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CH.Test.RescueGroups
{
    [TestClass]
    public class StorageTests
    {
        [TestMethod]
        public void CanReadErrorResponse()
        {
            MockHttpClient mockHttpClient = new MockHttpClient("{\"status\":\"error\",\"messages\":{\"generalMessages\":[{\"messageID\":\"1001\",\"messageCriticality\":\"error\",\"messageText\":\"The action you specified was not found.\"}],\"recordMessages\":[]},\"foundRows\":0,\"data\":[]}");
            CritterStatusSourceStorage storage = new CritterStatusSourceStorage(new RescueGroupsConfiguration(), mockHttpClient.Object);
            Action action = () =>
            {
                var result = storage.GetAllAsync().Result;
            };
            action.ShouldThrow<RescueGroupsException>().WithMessage("The action you specified was not found.");
        }
    }
}
