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
using Newtonsoft.Json.Linq;

namespace CH.Test.RescueGroups
{
    [TestClass]
    public class PersonSourceTests
    {
        [TestMethod]
        public void ObjectTypeIsCorrect()
        {
            new PersonSourceStorage(new RescueGroupsConfiguration(), null, null).ObjectType.Should().Be("contacts");
        }

        [TestMethod]
        public async Task ConvertsJsonToModel()
        {
            PersonSource source1 = new PersonSource()
            {
                ID = "1",
                FirstName = "First1",
                LastName = "Last1",
                Email = "email1@email1.com",
                Address = "Address1",
                City = "City1",
                State = "OH",
                Zip = "Zip1"
            };

            PersonSource source2 = new PersonSource()
            {
                ID = "2",
                FirstName = "First2",
                LastName = "Last2",
                Email = "email2@email2.com",
                Address = "Address2",
                City = "City2",
                State = "IO",
                Zip = "Zip2"
            };

            JProperty element1 = new JProperty(source1.ID, new JObject(
                new JProperty("contactID", source1.ID),
                new JProperty("contactFirstname", source1.FirstName),
                new JProperty("contactLastname", source1.LastName),
                new JProperty("contactEmail", source1.Email),
                new JProperty("contactAddress", source1.Address),
                new JProperty("contactCity", source1.City),
                new JProperty("contactState", source1.State),
                new JProperty("contactPostalcode", source1.Zip),
                new JProperty("contactPlus4", ""),
                new JProperty("contactGroups", "")
            ));

            JProperty element2 = new JProperty(source2.ID, new JObject(
                new JProperty("contactID", source2.ID),
                new JProperty("contactFirstname", source2.FirstName),
                new JProperty("contactLastname", source2.LastName),
                new JProperty("contactEmail", source2.Email),
                new JProperty("contactAddress", source2.Address),
                new JProperty("contactCity", source2.City),
                new JProperty("contactState", source2.State),
                new JProperty("contactPostalcode", source2.Zip),
                new JProperty("contactPlus4", "1234"),
                new JProperty("contactGroups", "Group1,Group2")
            ));

            MockHttpClient mockHttpClient = new MockHttpClient(element1, element2);

            Mock<IAppEventPublisher> mockPublisher = new Mock<IAppEventPublisher>();

            PersonSourceStorage storage = new PersonSourceStorage(new RescueGroupsConfiguration(), mockHttpClient.Object, mockPublisher.Object);
            IEnumerable<PersonSource> results = await storage.GetAllAsync();
            results.Should().HaveCount(2);

            PersonSource result1 = results.SingleOrDefault(x => x.ID == source1.ID);
            result1.Should().NotBeNull();

            result1.FirstName.Should().Be(source1.FirstName);
            result1.LastName.Should().Be(source1.LastName);
            result1.Email.Should().Be(source1.Email);
            result1.Address.Should().Be(source1.Address);
            result1.City.Should().Be(source1.City);
            result1.State.Should().Be(source1.State);
            result1.Zip.Should().Be(source1.Zip);
            result1.GroupNames.Should().BeNullOrEmpty();

            PersonSource result2 = results.SingleOrDefault(x => x.ID == source2.ID);
            result2.Should().NotBeNull();

            result2.FirstName.Should().Be(source2.FirstName);
            result2.LastName.Should().Be(source2.LastName);
            result2.Email.Should().Be(source2.Email);
            result2.Address.Should().Be(source2.Address);
            result2.City.Should().Be(source2.City);
            result2.State.Should().Be(source2.State);
            result2.Zip.Should().Be(source2.Zip + "1234");
            result2.GroupNames.Should().Equal(new[] { "Group1", "Group2" });
        }
    }
}
