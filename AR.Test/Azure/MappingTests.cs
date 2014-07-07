using System;
using AR.Azure.Mapping;
using AR.Domain.Models.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage.Table;

namespace AR.Test.Azure
{
    [TestClass]
    public class MappingTests
    {
        [TestMethod]
        public void TestAnimalStatusMapping()
        {
            IAzureMapping<AnimalStatus> mapping = AzureMappingFactory.GetMapping<AnimalStatus>();
            Assert.IsNotNull(mapping);

            AnimalStatus animalStatus = new AnimalStatus("name", "desccription");

            DynamicTableEntity entity = (DynamicTableEntity)mapping.ToEntity(animalStatus);
            Assert.AreEqual(2, entity.Properties.Count);
            Assert.AreEqual(animalStatus.Name, entity["Name"].StringValue);
            Assert.AreEqual(animalStatus.Description, entity["Description"].StringValue);
        }
    }
}
