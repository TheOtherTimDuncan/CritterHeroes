using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.Proxies.Configuration;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.DataProviders.Azure;
using CritterHeroes.Web.DataProviders.Azure.Storage.Data;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CH.Test.Azure.StorageTests
{
    [TestClass]
    public class AnimalStatusStorageTests : BaseAzureTest
    {
        [TestMethod]
        public void SuccessfullyMapsEntityToAndFromStorage()
        {
            AnimalStatus animalStatus = new AnimalStatus("0", "name", "description");

            AnimalStatusAzureStorageContext source = new AnimalStatusAzureStorageContext(new AzureConfiguration());
            AnimalStatusAzureStorageContext target = new AnimalStatusAzureStorageContext(new AzureConfiguration());
            AnimalStatus result = target.FromStorage(source.ToStorage(animalStatus));

            result.ID.Should().Be(animalStatus.ID);
            result.Name.Should().Be(animalStatus.Name);
            result.Description.Should().Be(animalStatus.Description);
        }

        [TestMethod]
        public async Task TestCRUDSingle()
        {
            AnimalStatus animalStatus = new AnimalStatus("0", "Name", "Description");

            AnimalStatusAzureStorageContext storage = new AnimalStatusAzureStorageContext(new AzureConfiguration());
            await storage.SaveAsync(animalStatus);

            AnimalStatus result = await storage.GetAsync(animalStatus.ID.ToString());
            result.Should().NotBeNull();
            result.Name.Should().Be(animalStatus.Name);
            result.Description.Should().Be(animalStatus.Description);

            await storage.DeleteAsync(animalStatus);
            AnimalStatus deleted = await storage.GetAsync(animalStatus.ID.ToString());
            deleted.Should().BeNull();
        }

        [TestMethod]
        public async Task TestCRUDMultiple()
        {
            AnimalStatusAzureStorageContext storage = new AnimalStatusAzureStorageContext(new AzureConfiguration());

            AnimalStatus animalStatus1 = new AnimalStatus("1", "Name1", "Description1");
            AnimalStatus animalStatus2 = new AnimalStatus("2", "Name2", "Description2");

            await storage.SaveAsync(new AnimalStatus[] { animalStatus1, animalStatus2 });

            IEnumerable<AnimalStatus> results = await storage.GetAllAsync();
            results.Count().Should().BeGreaterOrEqualTo(2);

            AnimalStatus result1 = results.FirstOrDefault(x => x.ID == animalStatus1.ID);
            result1.Should().NotBeNull();
            result1.Name.Should().Be(animalStatus1.Name);
            result1.Description.Should().Be(animalStatus1.Description);

            AnimalStatus result2 = results.FirstOrDefault(x => x.ID == animalStatus2.ID);
            result2.Should().NotBeNull();
            result2.Name.Should().Be(animalStatus2.Name);
            result2.Description.Should().Be(animalStatus2.Description);

            await storage.DeleteAllAsync();

            IEnumerable<AnimalStatus> deleted = await storage.GetAllAsync();
            deleted.Should().BeNullOrEmpty();
        }
    }
}
