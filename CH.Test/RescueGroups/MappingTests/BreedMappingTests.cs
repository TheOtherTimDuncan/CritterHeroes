using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CH.Domain.Models.Data;
using CH.RescueGroups;
using CH.RescueGroups.Mappings;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace CH.Test.RescueGroups.MappingTests
{
    [TestClass]
    public class BreedMappingTests
    {
        public IRescueGroupsMapping<Breed> Mapping
        {
            get
            {
                return RescueGroupsMappingFactory.GetMapping<Breed>();
            }
        }

        [TestMethod]
        public void ObjectTypeIsCorrect()
        {
            Mapping.ObjectType.Should().Be("animalBreeds");
        }

        [TestMethod]
        public void ConvertsJsonResultToModel()
        {
            Breed animalStatus1 = new Breed("1", "Species 1", "Breed 1");
            Breed animalStatus2 = new Breed("2", "Species 2", "Breed 2");

            JProperty element1 = new JProperty("1", new JObject(new JProperty("species", animalStatus1.Species), new JProperty("name", animalStatus1.BreedName)));
            JProperty element2 = new JProperty("2", new JObject(new JProperty("species", animalStatus2.Species), new JProperty("name", animalStatus2.BreedName)));

            JObject data = new JObject();
            data.Add(element1);
            data.Add(element2);

            IEnumerable<Breed> animalBreeds = Mapping.ToModel(data.Properties());
            animalBreeds.Should().HaveCount(2);

            Breed result1 = animalBreeds.FirstOrDefault(x => x.ID == animalStatus1.ID);
            result1.Should().NotBeNull();
            result1.Species.Should().Be(animalStatus1.Species);
            result1.BreedName.Should().Be(animalStatus1.BreedName);

            Breed result2 = animalBreeds.FirstOrDefault(x => x.ID == animalStatus2.ID);
            result2.Should().NotBeNull();
            result2.Species.Should().Be(animalStatus2.Species);
            result2.BreedName.Should().Be(animalStatus2.BreedName);
        }

        [TestMethod]
        public async Task TestGetAnimalBreed()
        {
            RescueGroupsStorage storage = new RescueGroupsStorage();
            (await storage.GetAllAsync<Breed>()).ToList().Should().NotBeEmpty();
        }
    }
}
