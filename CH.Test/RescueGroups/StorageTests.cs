using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.DataProviders.RescueGroups;
using CritterHeroes.Web.DataProviders.RescueGroups.Configuration;
using CritterHeroes.Web.DataProviders.RescueGroups.Storage;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace CH.Test.RescueGroups
{
    [TestClass]
    public class StorageTests
    {
        [TestMethod]
        public void CanReadErrorResponse()
        {
            CritterStatusSourceStorage storage = new CritterStatusSourceStorage(new RescueGroupsConfiguration(), null);
            JObject json = JObject.Parse("{\"status\":\"error\",\"messages\":{\"generalMessages\":[{\"messageID\":\"1001\",\"messageCriticality\":\"error\",\"messageText\":\"The action you specified was not found.\"}],\"recordMessages\":[]},\"foundRows\":0,\"data\":[]}");
            Action action = () => storage.ValidateResponse(json);
            action.ShouldThrow<RescueGroupsException>().WithMessage("The action you specified was not found.");
        }
    }
}
