using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.DataProviders.RescueGroups.Configuration;
using CritterHeroes.Web.DataProviders.RescueGroups.Storage;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace CH.Test.RescueGroups.MappingTests
{
    [TestClass]
    public class AnimalStatusMappingTests : BaseTest
    {
        [TestMethod]
        public void ObjectTypeIsCorrect()
        {
            new AnimalStatusRescueGroupsStorage(new RescueGroupsConfiguration()).ObjectType.Should().Be("animalStatuses");
        }

        [TestMethod]
        public void ConvertsJsonResultToModel()
        {
            AnimalStatus animalStatus1 = new AnimalStatus("1", "Name 1", "Description 1");
            AnimalStatus animalStatus2 = new AnimalStatus("2", "Name 2", "Description 2");

            JProperty element1 = new JProperty("1", new JObject(new JProperty("name", animalStatus1.Name), new JProperty("description", animalStatus1.Description)));
            JProperty element2 = new JProperty("2", new JObject(new JProperty("name", animalStatus2.Name), new JProperty("description", animalStatus2.Description)));

            JObject data = new JObject();
            data.Add(element1);
            data.Add(element2);

            IEnumerable<AnimalStatus> animalStatuses = new AnimalStatusRescueGroupsStorage(new RescueGroupsConfiguration()).FromStorage(data.Properties());
            animalStatuses.Should().HaveCount(2);

            AnimalStatus result1 = animalStatuses.FirstOrDefault(x => x.ID == animalStatus1.ID);
            result1.Should().NotBeNull();
            result1.Name.Should().Be(animalStatus1.Name);
            result1.Description.Should().Be(animalStatus1.Description);

            AnimalStatus result2 = animalStatuses.FirstOrDefault(x => x.ID == animalStatus2.ID);
            result2.Should().NotBeNull();
            result2.Name.Should().Be(animalStatus2.Name);
            result2.Description.Should().Be(animalStatus2.Description);
        }

        [TestMethod]
        public async Task TestGetAnimalStatus()
        {
            AnimalStatusRescueGroupsStorage storage = new AnimalStatusRescueGroupsStorage(new RescueGroupsConfiguration());
            (await storage.GetAllAsync()).ToList().Should().NotBeEmpty();
        }
    }
}
