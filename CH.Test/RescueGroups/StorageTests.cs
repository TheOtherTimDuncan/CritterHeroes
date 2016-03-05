using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Test.Mocks;
using CritterHeroes.Web.Contracts.Events;
using CritterHeroes.Web.DataProviders.RescueGroups;
using CritterHeroes.Web.DataProviders.RescueGroups.Configuration;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using CritterHeroes.Web.DataProviders.RescueGroups.Storage;
using CritterHeroes.Web.Models.LogEvents;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test.RescueGroups
{
    [TestClass]
    public class StorageTests
    {
        [TestMethod]
        public void CanReadErrorResponse()
        {
            MockHttpClient mockHttpClient = new MockHttpClient("{\"status\":\"error\",\"messages\":{\"generalMessages\":[{\"messageID\":\"1001\",\"messageCriticality\":\"error\",\"messageText\":\"The action you specified was not found.\"}],\"recordMessages\":[]},\"foundRows\":0,\"data\":[]}");

            Mock<IAppEventPublisher> mockPublisher = new Mock<IAppEventPublisher>();

            CritterStatusSourceStorage storage = new CritterStatusSourceStorage(new RescueGroupsConfiguration(), mockHttpClient.Object, mockPublisher.Object);
            Action action = () =>
            {
                var result = storage.GetAllAsync().Result;
            };

            action.ShouldThrow<RescueGroupsException>().WithMessage("The action you specified was not found.");

            mockPublisher.Verify(x => x.Publish(It.IsAny<RescueGroupsLogEvent>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task CanReadResponseWithNoData()
        {
            MockHttpClient mockHttpClient = new MockHttpClient("{\"status\":\"ok\",\"messages\":{\"generalMessages\":[],\"recordMessages\":[]},\"foundRows\":0,\"data\":[]}");

            Mock<IAppEventPublisher> mockPublisher = new Mock<IAppEventPublisher>();

            CritterStatusSourceStorage storage = new CritterStatusSourceStorage(new RescueGroupsConfiguration(), mockHttpClient.Object, mockPublisher.Object);
            IEnumerable<CritterStatusSource> result = await storage.GetAllAsync();
            result.Should().BeEmpty();

            mockPublisher.Verify(x => x.Publish(It.IsAny<RescueGroupsLogEvent>()), Times.Exactly(2));
        }
    }
}
