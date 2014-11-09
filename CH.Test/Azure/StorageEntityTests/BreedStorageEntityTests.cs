using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CH.Azure;
using CH.Azure.Storage;
using CH.Domain.Models.Data;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CH.Test.Azure.StorageEntityTests
{
    [TestClass]
    public class BreedStorageEntityTests : BaseStorageEntityTest
    {
        [TestMethod]
        public void SuccessfullyMapsEntityToStorage()
        {
            Breed animalBreed = new Breed("0", "species", "breed");

            StorageEntity<Breed> storageEntity = StorageEntityFactory.GetStorageEntity<Breed>();
            storageEntity.Should().NotBeNull();

            storageEntity.Entity = animalBreed;
            storageEntity.TableEntity.Properties.Count.Should().Be(3);
            storageEntity.TableEntity["ID"].StringValue.Should().Be(animalBreed.ID);
            storageEntity.TableEntity["Species"].StringValue.Should().Be(animalBreed.Species);
            storageEntity.TableEntity["BreedName"].StringValue.Should().Be(animalBreed.BreedName);
        }

        [TestMethod]
        public void SuccessfullyMapsStorageToEntity()
        {
            Breed animalBreed = new Breed("0", "species", "breed");

            StorageEntity<Breed> storageEntity1 = StorageEntityFactory.GetStorageEntity<Breed>();
            storageEntity1.Should().NotBeNull();
            storageEntity1.Entity = animalBreed;

            StorageEntity<Breed> storageEntity2 = StorageEntityFactory.GetStorageEntity<Breed>();
            storageEntity2.Should().NotBeNull();
            storageEntity2.TableEntity = storageEntity1.TableEntity;

            storageEntity2.Entity.ID.Should().Be(animalBreed.ID);
            storageEntity2.Entity.Species.Should().Be(animalBreed.Species);
            storageEntity2.Entity.BreedName.Should().Be(animalBreed.BreedName);
        }

        [TestMethod]
        public async Task TestCRUDSingle()
        {
            Breed animalStatus = new Breed("0", "Species", "Breed");
            AzureStorage storage = GetEntityAzureStorage();
            await storage.SaveAsync<Breed>(animalStatus);

            Breed result = await storage.GetAsync<Breed>(animalStatus.ID);
            result.Should().NotBeNull();
            result.Species.Should().Be(animalStatus.Species);
            result.BreedName.Should().Be(animalStatus.BreedName);

            await storage.DeleteAsync<Breed>(animalStatus);
            AnimalStatus deleted = await storage.GetAsync<AnimalStatus>(animalStatus.ID);
            deleted.Should().BeNull();
        }

        [TestMethod]
        public async Task TestCRUDMultiple()
        {
            AzureStorage storage = GetEntityAzureStorage();

            Breed animalBreed1 = new Breed("1", "Species1", "Breed1");
            Breed animalBreed2 = new Breed("2", "Species2", "Breed2");

            await storage.SaveAsync<Breed>(new Breed[] { animalBreed1, animalBreed2 });

            IEnumerable<Breed> results = await storage.GetAllAsync<Breed>();
            results.Count().Should().BeGreaterOrEqualTo(2);

            Breed result1 = results.FirstOrDefault(x => x.ID == animalBreed1.ID);
            result1.Should().NotBeNull();
            result1.Species.Should().Be(animalBreed1.Species);
            result1.BreedName.Should().Be(animalBreed1.BreedName);

            Breed result2 = results.FirstOrDefault(x => x.ID == animalBreed2.ID);
            result2.Should().NotBeNull();
            result2.Species.Should().Be(animalBreed2.Species);
            result2.BreedName.Should().Be(animalBreed2.BreedName);

            await storage.DeleteAllAsync<Breed>();

            IEnumerable<Breed> deleted = await storage.GetAllAsync<Breed>();
            deleted.Should().BeNullOrEmpty();
        }
    }
}
