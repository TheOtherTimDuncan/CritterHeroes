using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Domain.Contracts;
using CH.Domain.Handlers.DataStatus;
using CH.Domain.Models.Data;
using CH.Domain.Models.Status;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test.Models
{
    [TestClass]
    public class DataStatusTests
    {
        [TestMethod]
        public async Task TestAnimalStatusBehavior()
        {
            AnimalStatus[] entities1 = new AnimalStatus[] { new AnimalStatus("1", "Name1", "Status1"), new AnimalStatus("2", "Name2", "Status2") };
            IStorageContext context1 = Mock.Of<IStorageContext>(x => x.GetAllAsync<AnimalStatus>() == Task.FromResult<IEnumerable<AnimalStatus>>(entities1));
            Mock<IStorageSource> mockSource1 = new Mock<IStorageSource>();
            mockSource1.Setup(x => x.ID).Returns(1);
            mockSource1.Setup(x => x.StorageContext).Returns(context1);

            AnimalStatus[] entities2 = new AnimalStatus[] { new AnimalStatus("2", "Name2", "Status2"), new AnimalStatus("3", "Name3", "Status3") };
            IStorageContext context2 = Mock.Of<IStorageContext>(x => x.GetAllAsync<AnimalStatus>() == Task.FromResult<IEnumerable<AnimalStatus>>(entities2));
            Mock<IStorageSource> mockSource2 = new Mock<IStorageSource>();
            mockSource2.Setup(x => x.ID).Returns(2);
            mockSource2.Setup(x => x.StorageContext).Returns(context2);

            IDataStatusHandler handler = new AnimalStatusStatusHandler();
            DataStatusModel model = await handler.GetModelStatusAsync(null, mockSource1.Object, mockSource2.Object);

            model.Items.Count().Should().Be(2);
            model.DataItemCount.Should().Be(3);

            StorageItem storageItem1 = model.Items.FirstOrDefault(x => x.StorageID == mockSource1.Object.ID);
            storageItem1.Should().NotBeNull();
            storageItem1.InvalidCount.Should().Be(1);
            storageItem1.ValidCount.Should().Be(2);

            ValidateDataItem(storageItem1.Items.ElementAt(0), entities1[0].Name, true);
            ValidateDataItem(storageItem1.Items.ElementAt(1), entities1[1].Name, true);
            ValidateDataItem(storageItem1.Items.ElementAt(2), null, false);

            StorageItem storageItem2 = model.Items.FirstOrDefault(x => x.StorageID == mockSource2.Object.ID);
            storageItem2.Should().NotBeNull();
            storageItem2.InvalidCount.Should().Be(1);
            storageItem2.ValidCount.Should().Be(2);

            ValidateDataItem(storageItem2.Items.ElementAt(1), entities2[0].Name, true);
            ValidateDataItem(storageItem2.Items.ElementAt(2), entities2[1].Name, true);
            ValidateDataItem(storageItem2.Items.ElementAt(0), null, false);
        }

        [TestMethod]
        public async Task TestBreedBehavior()
        {
            Breed[] entities1 = new Breed[] { new Breed("1", "Species1", "Breed1"), new Breed("2", "Species2", "Breed2") };
            IStorageContext context1 = Mock.Of<IStorageContext>(x => x.GetAllAsync<Breed>() == Task.FromResult<IEnumerable<Breed>>(entities1));
            Mock<IStorageSource> mockSource1 = new Mock<IStorageSource>();
            mockSource1.Setup(x => x.ID).Returns(1);
            mockSource1.Setup(x => x.StorageContext).Returns(context1);

            Breed[] entities2 = new Breed[] { new Breed("2", "Species2", "Breed2"), new Breed("3", "Species3", "Breed3") };
            IStorageContext context2 = Mock.Of<IStorageContext>(x => x.GetAllAsync<Breed>() == Task.FromResult<IEnumerable<Breed>>(entities2));
            Mock<IStorageSource> mockSource2 = new Mock<IStorageSource>();
            mockSource2.Setup(x => x.ID).Returns(2);
            mockSource2.Setup(x => x.StorageContext).Returns(context2);

            StatusContext statusContext = new StatusContext()
            {
                SupportedCritters = new Species[]
                {
                        new Species("Species1","1","2",null,null),
                        new Species("Species2","1","2",null,null),
                        new Species("Species3","1","2",null,null)
                }
            };

            IDataStatusHandler handler = new BreedStatusHandler();
            DataStatusModel model = await handler.GetModelStatusAsync(statusContext, mockSource1.Object, mockSource2.Object);

            model.Items.Count().Should().Be(2);
            model.DataItemCount.Should().Be(3);

            StorageItem storageItem1 = model.Items.FirstOrDefault(x => x.StorageID == mockSource1.Object.ID);
            storageItem1.Should().NotBeNull();
            storageItem1.InvalidCount.Should().Be(1);
            storageItem1.ValidCount.Should().Be(2);

            ValidateDataItem(storageItem1.Items.ElementAt(0), entities1[0].Species + " - " + entities1[0].BreedName, true);
            ValidateDataItem(storageItem1.Items.ElementAt(1), entities1[1].Species + " - " + entities1[1].BreedName, true);
            ValidateDataItem(storageItem1.Items.ElementAt(2), null, false);

            StorageItem storageItem2 = model.Items.FirstOrDefault(x => x.StorageID == mockSource2.Object.ID);
            storageItem2.Should().NotBeNull();
            storageItem2.InvalidCount.Should().Be(1);
            storageItem2.ValidCount.Should().Be(2);

            ValidateDataItem(storageItem2.Items.ElementAt(1), entities2[0].Species + " - " + entities2[0].BreedName, true);
            ValidateDataItem(storageItem2.Items.ElementAt(2), entities2[1].Species + " - " + entities2[1].BreedName, true);
            ValidateDataItem(storageItem2.Items.ElementAt(0), null, false);
        }

        private void ValidateDataItem(DataItem dataItem, string expectedValue, bool isValid)
        {
            dataItem.Value.Should().Be(expectedValue);
            dataItem.IsValid.Should().Be(isValid);
        }
    }
}
