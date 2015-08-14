using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.DataProviders.RescueGroups.Configuration;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using CritterHeroes.Web.DataProviders.RescueGroups.Storage;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace CH.Test.RescueGroups.MappingTests
{
    [TestClass]
    public class BreedSourceTests : BaseTest
    {
        [TestMethod]
        public void ObjectTypeIsCorrect()
        {
            new BreedSourceStorage(new RescueGroupsConfiguration()).ObjectType.Should().Be("animalBreeds");
        }

        [TestMethod]
        public void ConvertsJsonResultToModel()
        {
            BreedSource breedSource1 = new BreedSource("1", "Species 1", "Breed 1");
            BreedSource breedSource2 = new BreedSource("2", "Species 2", "Breed 2");

            JProperty element1 = new JProperty("1", new JObject(new JProperty("species", breedSource1.Species), new JProperty("name", breedSource1.BreedName)));
            JProperty element2 = new JProperty("2", new JObject(new JProperty("species", breedSource2.Species), new JProperty("name", breedSource2.BreedName)));

            JObject data = new JObject();
            data.Add(element1);
            data.Add(element2);

            IEnumerable<BreedSource> animalBreeds = new BreedSourceStorage(new RescueGroupsConfiguration()).FromStorage(data.Properties());
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
