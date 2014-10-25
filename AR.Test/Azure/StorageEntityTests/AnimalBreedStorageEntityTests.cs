using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AR.Azure;
using AR.Azure.Storage;
using AR.Domain.Models.Data;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AR.Test.Azure.StorageEntityTests
{
    [TestClass]
    public class AnimalBreedStorageEntityTests
    {
        [TestMethod]
        public void SuccessfullyMapsEntityToStorage()
        {
            AnimalBreed animalBreed = new AnimalBreed("0", "species", "breed");

            StorageEntity<AnimalBreed> storageEntity = StorageEntityFactory.GetStorageEntity<AnimalBreed>();
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
            AnimalBreed animalBreed = new AnimalBreed("0", "species", "breed");

            StorageEntity<AnimalBreed> storageEntity1 = StorageEntityFactory.GetStorageEntity<AnimalBreed>();
            storageEntity1.Should().NotBeNull();
            storageEntity1.Entity = animalBreed;

            StorageEntity<AnimalBreed> storageEntity2 = StorageEntityFactory.GetStorageEntity<AnimalBreed>();
            storageEntity2.Should().NotBeNull();
            storageEntity2.TableEntity = storageEntity1.TableEntity;

            storageEntity2.Entity.ID.Should().Be(animalBreed.ID);
            storageEntity2.Entity.Species.Should().Be(animalBreed.Species);
            storageEntity2.Entity.BreedName.Should().Be(animalBreed.BreedName);
        }

        [TestMethod]
        public async Task TestCRUDSingle()
        {
            AnimalBreed animalStatus = new AnimalBreed("0", "Species", "Breed");
            AzureStorage storage = new AzureStorage("fflah");
            await storage.SaveAsync<AnimalBreed>(animalStatus);

            AnimalBreed result = await storage.GetAsync<AnimalBreed>(animalStatus.ID);
            result.Should().NotBeNull();
            result.Species.Should().Be(animalStatus.Species);
            result.BreedName.Should().Be(animalStatus.BreedName);

            await storage.DeleteAsync<AnimalBreed>(animalStatus);
            AnimalStatus deleted = await storage.GetAsync<AnimalStatus>(animalStatus.ID);
            deleted.Should().BeNull();
        }

        [TestMethod]
        public async Task TestCRUDMultiple()
        {
            AzureStorage storage = new AzureStorage("fflah");

            AnimalBreed animalBreed1 = new AnimalBreed("1", "Species1", "Breed1");
            AnimalBreed animalBreed2 = new AnimalBreed("2", "Species2", "Breed2");

            await storage.SaveAsync<AnimalBreed>(new AnimalBreed[] { animalBreed1, animalBreed2 });

            IEnumerable<AnimalBreed> results = await storage.GetAllAsync<AnimalBreed>();
            results.Count().Should().BeGreaterOrEqualTo(2);

            AnimalBreed result1 = results.FirstOrDefault(x => x.ID == animalBreed1.ID);
            result1.Should().NotBeNull();
            result1.Species.Should().Be(animalBreed1.Species);
            result1.BreedName.Should().Be(animalBreed1.BreedName);

            AnimalBreed result2 = results.FirstOrDefault(x => x.ID == animalBreed2.ID);
            result2.Should().NotBeNull();
            result2.Species.Should().Be(animalBreed2.Species);
            result2.BreedName.Should().Be(animalBreed2.BreedName);

            await storage.DeleteAllAsync<AnimalBreed>();

            IEnumerable<AnimalBreed> deleted = await storage.GetAllAsync<AnimalBreed>();
            deleted.Should().BeNullOrEmpty();
        }
    }
}
