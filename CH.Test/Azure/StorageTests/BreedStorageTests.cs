using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.Proxies.Configuration;
using CritterHeroes.Web.DataProviders.Azure;
using CritterHeroes.Web.DataProviders.Azure.Storage.Data;
using CritterHeroes.Web.Models.Data;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CH.Test.Azure.StorageTests
{
    [TestClass]
    public class BreedStorageTests : BaseAzureTest
    {
        [TestMethod]
        public void SuccessfullyMapsEntityToAndFromStorage()
        {
            Breed animalBreed = new Breed("0", "species", "breed");

            BreedAzureStorageContext source = new BreedAzureStorageContext(new AzureConfiguration());
            BreedAzureStorageContext target = new BreedAzureStorageContext(new AzureConfiguration());
            Breed result = target.FromStorage(source.ToStorage(animalBreed));

            result.ID.Should().Be(animalBreed.ID);
            result.Species.Should().Be(animalBreed.Species);
            result.BreedName.Should().Be(animalBreed.BreedName);
        }

        [TestMethod]
        public async Task TestCRUDSingle()
        {
            Breed animalStatus = new Breed("0", "Species", "Breed");

            BreedAzureStorageContext storage = new BreedAzureStorageContext(new AzureConfiguration());
            await storage.SaveAsync(animalStatus);

            Breed result = await storage.GetAsync(animalStatus.ID);
            result.Should().NotBeNull();
            result.Species.Should().Be(animalStatus.Species);
            result.BreedName.Should().Be(animalStatus.BreedName);

            await storage.DeleteAsync(animalStatus);
            Breed deleted = await storage.GetAsync(animalStatus.ID);
            deleted.Should().BeNull();
        }

        [TestMethod]
        public async Task TestCRUDMultiple()
        {
            BreedAzureStorageContext storage = new BreedAzureStorageContext(new AzureConfiguration());

            Breed animalBreed1 = new Breed("1", "Species1", "Breed1");
            Breed animalBreed2 = new Breed("2", "Species2", "Breed2");

            await storage.SaveAsync(new Breed[] { animalBreed1, animalBreed2 });

            IEnumerable<Breed> results = await storage.GetAllAsync();
            results.Count().Should().BeGreaterOrEqualTo(2);

            Breed result1 = results.FirstOrDefault(x => x.ID == animalBreed1.ID);
            result1.Should().NotBeNull();
            result1.Species.Should().Be(animalBreed1.Species);
            result1.BreedName.Should().Be(animalBreed1.BreedName);

            Breed result2 = results.FirstOrDefault(x => x.ID == animalBreed2.ID);
            result2.Should().NotBeNull();
            result2.Species.Should().Be(animalBreed2.Species);
            result2.BreedName.Should().Be(animalBreed2.BreedName);

            await storage.DeleteAllAsync();

            IEnumerable<Breed> deleted = await storage.GetAllAsync();
            deleted.Should().BeNullOrEmpty();
        }
    }
}
