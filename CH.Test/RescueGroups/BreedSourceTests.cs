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

namespace CH.Test.RescueGroups.MappingTests
{
    [TestClass]
    public class BreedSourceTests : BaseTest
    {
        [TestMethod]
        public void ObjectTypeIsCorrect()
        {
            new BreedSourceStorage(new RescueGroupsConfiguration(), null, null).ObjectType.Should().Be("animalBreeds");
        }

        [TestMethod]
        public async Task ConvertsJsonResultToModel()
        {
            BreedSource breedSource1 = new BreedSource("1", "Species 1", "Breed 1");
            BreedSource breedSource2 = new BreedSource("2", "Species 2", "Breed 2");

            string json = @"
{
    ""1"": {
        ""breedID"": ""1"",
        ""breedName"": ""Breed 1"",
        ""breedSpecies"": ""Species 1""
    },
    ""2"": {
        ""breedID"": ""2"",
        ""breedName"": ""Breed 2"",
        ""breedSpecies"": ""Species 2""
    }}
";
            MockHttpClient mockClient = new MockHttpClient()
                .SetupLoginResponse()
                .SetupSearchResponse(json);

            Mock<IAppEventPublisher> mockPublisher = new Mock<IAppEventPublisher>();

            BreedSourceStorage storage = new BreedSourceStorage(new RescueGroupsConfiguration(), mockClient.Object, mockPublisher.Object);

            IEnumerable<BreedSource> animalBreeds = await storage.GetAllAsync();
            animalBreeds.Should().HaveCount(2);

            BreedSource result1 = animalBreeds.FirstOrDefault(x => x.ID == breedSource1.ID);
            result1.Should().NotBeNull();
            result1.Species.Should().Be(breedSource1.Species);
            result1.BreedName.Should().Be(breedSource1.BreedName);

            BreedSource result2 = animalBreeds.FirstOrDefault(x => x.ID == breedSource2.ID);
            result2.Should().NotBeNull();
            result2.Species.Should().Be(breedSource2.Species);
            result2.BreedName.Should().Be(breedSource2.BreedName);
        }
    }
}
