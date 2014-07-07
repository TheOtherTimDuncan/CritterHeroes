using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AR.Domain.Contracts;
using AR.Domain.Handlers.DataStatus;
using AR.Domain.Models.Data;
using AR.Domain.Models.Status;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AR.Test.Models
{
    [TestClass]
    public class DataStatusTests
    {
        [TestMethod]
        public async Task TestBehavior()
        {
            AnimalStatus[] entities1 = new AnimalStatus[] { new AnimalStatus("1", "Status1"), new AnimalStatus("2", "Status2") };
            IStorageContext context1 = Mock.Of<IStorageContext>(x => x.GetAllAsync<AnimalStatus>() == Task.FromResult<IEnumerable<AnimalStatus>>(entities1));
            context1.ID = 1;

            AnimalStatus[] entities2 = new AnimalStatus[] { new AnimalStatus("2", "Status2"), new AnimalStatus("3", "Status3") };
            IStorageContext context2 = Mock.Of<IStorageContext>(x => x.GetAllAsync<AnimalStatus>() == Task.FromResult<IEnumerable<AnimalStatus>>(entities2));
            context2.ID = 2;

            IDataStatusHandler handler = new AnimalStatusStatusHandler();
            DataStatusModel model = await handler.GetModelStatusAsync(context1, context2);

            Assert.AreEqual(2, model.Items.Count());

            StorageItem storageItem1 = model.Items.FirstOrDefault(x => x.StorageID == context1.ID);
            Assert.IsNotNull(storageItem1);
            Assert.AreEqual(1, storageItem1.InvalidCount);
            Assert.AreEqual(2, storageItem1.ValidCount);
            ValidateDataItem(storageItem1.Items.ElementAt(0), entities1[0].Name, true);
            ValidateDataItem(storageItem1.Items.ElementAt(1), entities1[1].Name, true);
            ValidateDataItem(storageItem1.Items.ElementAt(2), null, false);

            StorageItem storageItem2 = model.Items.FirstOrDefault(x => x.StorageID == context2.ID);
            Assert.IsNotNull(storageItem2);
            Assert.AreEqual(1, storageItem2.InvalidCount);
            Assert.AreEqual(2, storageItem2.ValidCount);
            ValidateDataItem(storageItem2.Items.ElementAt(1), entities2[0].Name, true);
            ValidateDataItem(storageItem2.Items.ElementAt(2), entities2[1].Name, true);
            ValidateDataItem(storageItem2.Items.ElementAt(0), null, false);
        }

        private void ValidateDataItem(DataItem dataItem, string expectedValue, bool isValid)
        {
            Assert.AreEqual(expectedValue, dataItem.Value);
            Assert.AreEqual(isValid, dataItem.IsValid);
        }
    }
}
