using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AR.Azure;
using AR.Domain.Models.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TOTD.Utility.EnumerableHelpers;
using TOTD.Utility.StringHelpers;

namespace AR.Test.Azure
{
    [TestClass]
    public class StorageTests
    {
        [TestMethod]
        public void UsesConnectionStringFromConfigByDefault()
        {
            AzureStorage storage = new AzureStorage("fflah");
            Assert.IsFalse(storage.ConnectionString.IsNullOrEmpty());
        }

        [TestMethod]
        public async Task TestCRUDSingle()
        {
            AnimalStatus animalStatus = new AnimalStatus("Name", "Description");
            AzureStorage storage = new AzureStorage("fflah");
            await storage.SaveAsync<AnimalStatus>(animalStatus);

            AnimalStatus result = await storage.GetAsync<AnimalStatus>(animalStatus.Name);
            Assert.IsNotNull(result);
            Assert.AreEqual(animalStatus.Description, result.Description);

            await storage.DeleteAsync<AnimalStatus>(animalStatus);
            AnimalStatus deleted = await storage.GetAsync<AnimalStatus>(animalStatus.Name);
            Assert.IsNull(deleted);
        }

        [TestMethod]
        public async Task TestCRUDMultiple()
        {
            AzureStorage storage = new AzureStorage("fflah");

            AnimalStatus animalStatus1 = new AnimalStatus("Name1", "Description1");
            AnimalStatus animalStatus2 = new AnimalStatus("Name2", "Description2");

            await storage.SaveAsync<AnimalStatus>(new AnimalStatus[]{animalStatus1,animalStatus2});

            IEnumerable<AnimalStatus> results = await storage.GetAllAsync<AnimalStatus>();
            Assert.IsTrue(results.Count() >= 2);

            AnimalStatus result1 = results.FirstOrDefault(x => x.Name == animalStatus1.Name);
            Assert.IsNotNull(result1);
            Assert.AreEqual(animalStatus1.Description, result1.Description);

            AnimalStatus result2 = results.FirstOrDefault(x => x.Name == animalStatus2.Name);
            Assert.IsNotNull(result2);
            Assert.AreEqual(animalStatus2.Description, result2.Description);

            await storage.DeleteAllAsync<AnimalStatus>();

            IEnumerable<AnimalStatus> deleted = await storage.GetAllAsync<AnimalStatus>();
            Assert.IsTrue(deleted.IsNullOrEmpty());
        }

        [TestMethod]
        public async Task TestGet()
        {
            AzureStorage storage = new AzureStorage("fflah");
            IEnumerable<AnimalStatus> result = await storage.GetAllAsync<AnimalStatus>();
        }
    }
}
