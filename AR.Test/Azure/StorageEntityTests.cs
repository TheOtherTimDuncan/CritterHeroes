using System;
using AR.Azure.Storage;
using AR.Domain.Models.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage.Table;

namespace AR.Test.Azure
{
    [TestClass]
    public class StorageEntityTests
    {
        [TestClass]
        public class AnimalStatusStorageEntityTests
        {
            [TestMethod]
            public void SuccessfullyMapsEntityToStorage()
            {
                AnimalStatus animalStatus = new AnimalStatus("name", "desccription");

                StorageEntity<AnimalStatus> storageEntity = StorageEntityFactory.GetStorageEntity<AnimalStatus>();
                Assert.IsNotNull(storageEntity);

                storageEntity.Entity = animalStatus;
                Assert.AreEqual(2, storageEntity.TableEntity.Properties.Count);
                Assert.AreEqual(animalStatus.Name, storageEntity.TableEntity["Name"].StringValue);
                Assert.AreEqual(animalStatus.Description, storageEntity.TableEntity["Description"].StringValue);
            }

            [TestMethod]
            public void SuccessfullyMapsStorageToEntity()
            {
                AnimalStatus animalStatus = new AnimalStatus("name", "desccription");

                StorageEntity<AnimalStatus> storageEntity1 = StorageEntityFactory.GetStorageEntity<AnimalStatus>();
                Assert.IsNotNull(storageEntity1);
                storageEntity1.Entity = animalStatus;

                StorageEntity<AnimalStatus> storageEntity2 = StorageEntityFactory.GetStorageEntity<AnimalStatus>();
                Assert.IsNotNull(storageEntity2);
                storageEntity2.TableEntity = storageEntity1.TableEntity;

                Assert.AreEqual(animalStatus.Name, storageEntity2.Entity.Name);
                Assert.AreEqual(animalStatus.Description, storageEntity2.Entity.Description);
            }
        }
    }
}
