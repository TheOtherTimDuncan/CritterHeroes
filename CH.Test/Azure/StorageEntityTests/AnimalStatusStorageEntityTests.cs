using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Azure;
using CH.Azure.Storage;
using CH.Domain.Models.Data;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage.Table;

namespace CH.Test.Azure.StorageEntityTests
{
    [TestClass]
    public class AnimalStatusStorageEntityTests : BaseStorageEntityTest
    {
        [TestMethod]
        public void SuccessfullyMapsEntityToStorage()
        {
            AnimalStatus animalStatus = new AnimalStatus("0", "name", "desccription");

            StorageEntity<AnimalStatus> storageEntity = StorageEntityFactory.GetStorageEntity<AnimalStatus>();
            storageEntity.Should().NotBeNull();

            storageEntity.Entity = animalStatus;
            storageEntity.TableEntity.Properties.Count.Should().Be(3);
            storageEntity.TableEntity["ID"].StringValue.Should().Be(animalStatus.ID);
            storageEntity.TableEntity["Name"].StringValue.Should().Be(animalStatus.Name);
            storageEntity.TableEntity["Description"].StringValue.Should().Be(animalStatus.Description);
        }

        [TestMethod]
        public void SuccessfullyMapsStorageToEntity()
        {
            AnimalStatus animalStatus = new AnimalStatus("0", "name", "description");

            StorageEntity<AnimalStatus> storageEntity1 = StorageEntityFactory.GetStorageEntity<AnimalStatus>();
            storageEntity1.Should().NotBeNull();
            storageEntity1.Entity = animalStatus;

            StorageEntity<AnimalStatus> storageEntity2 = StorageEntityFactory.GetStorageEntity<AnimalStatus>();
            storageEntity2.Should().NotBeNull();
            storageEntity2.TableEntity = storageEntity1.TableEntity;

            storageEntity2.Entity.ID.Should().Be(animalStatus.ID);
            storageEntity2.Entity.Name.Should().Be(animalStatus.Name);
            storageEntity2.Entity.Description.Should().Be(animalStatus.Description);
        }

        [TestMethod]
        public async Task TestCRUDSingle()
        {
            AnimalStatus animalStatus = new AnimalStatus("0", "Name", "Description");
            AzureStorage storage = GetEntityAzureStorage();
            await storage.SaveAsync<AnimalStatus>(animalStatus);

            AnimalStatus result = await storage.GetAsync<AnimalStatus>(animalStatus.ID);
            result.Should().NotBeNull();
            result.Name.Should().Be(animalStatus.Name);
            result.Description.Should().Be(animalStatus.Description);

            await storage.DeleteAsync<AnimalStatus>(animalStatus);
            AnimalStatus deleted = await storage.GetAsync<AnimalStatus>(animalStatus.ID);
            deleted.Should().BeNull();
        }

        [TestMethod]
        public async Task TestCRUDMultiple()
        {
            AzureStorage storage = GetEntityAzureStorage();

            AnimalStatus animalStatus1 = new AnimalStatus("1", "Name1", "Description1");
            AnimalStatus animalStatus2 = new AnimalStatus("2", "Name2", "Description2");

            await storage.SaveAsync<AnimalStatus>(new AnimalStatus[] { animalStatus1, animalStatus2 });

            IEnumerable<AnimalStatus> results = await storage.GetAllAsync<AnimalStatus>();
            results.Count().Should().BeGreaterOrEqualTo(2);

            AnimalStatus result1 = results.FirstOrDefault(x => x.ID == animalStatus1.ID);
            result1.Should().NotBeNull();
            result1.Name.Should().Be(animalStatus1.Name);
            result1.Description.Should().Be(animalStatus1.Description);

            AnimalStatus result2 = results.FirstOrDefault(x => x.ID == animalStatus2.ID);
            result2.Should().NotBeNull();
            result2.Name.Should().Be(animalStatus2.Name);
            result2.Description.Should().Be(animalStatus2.Description);

            await storage.DeleteAllAsync<AnimalStatus>();

            IEnumerable<AnimalStatus> deleted = await storage.GetAllAsync<AnimalStatus>();
            deleted.Should().BeNullOrEmpty();
        }
    }
}
