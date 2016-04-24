using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Test.Mocks;
using CritterHeroes.Web.Contracts.Events;
using CritterHeroes.Web.DataProviders.RescueGroups.Configuration;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using CritterHeroes.Web.DataProviders.RescueGroups.Storage;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test.RescueGroups
{
    [TestClass]
    public class CritterStatusSourceTests : BaseTest
    {
        [TestMethod]
        public void ObjectTypeIsCorrect()
        {
            new CritterStatusSourceStorage(new RescueGroupsConfiguration(), null, null).ObjectType.Should().Be("animalStatuses");
        }

        [TestMethod]
        public async Task ConvertsJsonResultToModel()
        {
            CritterStatusSource critterStatus1 = new CritterStatusSource(1, "Name 1", "Description 1");
            CritterStatusSource critterStatus2 = new CritterStatusSource(2, "Name 2", "Description 2");

            string json = @"
{
    ""1"": {
        ""statusID"": ""1"",
        ""statusName"": ""Name 1"",
        ""statusDescription"": ""Description 1""
    },
    ""2"": {
        ""statusID"": ""2"",
        ""statusName"": ""Name 2"",
        ""statusDescription"": ""Description 2""
    }}
";

            MockHttpClient mockClient = new MockHttpClient()
             .SetupLoginResponse()
             .SetupSearchResponse(json);

            Mock<IAppEventPublisher> mockPublisher = new Mock<IAppEventPublisher>();

            CritterStatusSourceStorage storage = new CritterStatusSourceStorage(new RescueGroupsConfiguration(), mockClient.Object, mockPublisher.Object);

            IEnumerable<CritterStatusSource> critterStatuses = await storage.GetAllAsync();
            critterStatuses.Should().HaveCount(2);

            CritterStatusSource result1 = critterStatuses.FirstOrDefault(x => x.ID == critterStatus1.ID);
            result1.Should().NotBeNull();
            result1.Name.Should().Be(critterStatus1.Name);
            result1.Description.Should().Be(critterStatus1.Description);

            CritterStatusSource result2 = critterStatuses.FirstOrDefault(x => x.ID == critterStatus2.ID);
            result2.Should().NotBeNull();
            result2.Name.Should().Be(critterStatus2.Name);
            result2.Description.Should().Be(critterStatus2.Description);
        }
    }
}
