using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Test.Mocks;
using CritterHeroes.Web.Areas.Admin.Lists.DataMappers;
using CritterHeroes.Web.Areas.Admin.Lists.Models;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test.AdminDataMapperTests
{
    [TestClass]
    public class BreedDataMapperTests : BaseDataMapperTest
    {
        [TestMethod]
        public async Task ReturnsCollectionOfUniqueItems()
        {
            BreedSource source1 = new BreedSource("1", "species1", "breed1");
            BreedSource source2 = new BreedSource("2", "species2", "breed2");

            Breed master1 = new Breed(2, "species2", "breed2");
            Breed master2 = new Breed(3, "species3", "breed3");

            MockRescueGroupsStorageContext<BreedSource> mockSourceStorage = new MockRescueGroupsStorageContext<BreedSource>(source1, source2);

            MockSqlStorageContext<Breed> mockSqlStorage = new MockSqlStorageContext<Breed>(master1, master2);

            Mock<IStateManager<OrganizationContext>> mockStateManager = new Mock<IStateManager<OrganizationContext>>();

            BreedDataMapper mapper = new BreedDataMapper(mockSqlStorage.Object, mockSourceStorage.Object, mockStateManager.Object);
            DashboardItemStatus status = await mapper.GetDashboardItemStatusAsync();

            status.TargetItem.Items.Should().HaveCount(3);
            status.TargetItem.InvalidCount.Should().Be(1);
            status.TargetItem.ValidCount.Should().Be(2);

            ValidateDataItem(status.TargetItem.Items.ElementAt(0), expectedValue: null, isValid: false);
            ValidateDataItem(status.TargetItem.Items.ElementAt(1), expectedValue: master1.Species + " - " + master1.BreedName, isValid: true);
            ValidateDataItem(status.TargetItem.Items.ElementAt(2), expectedValue: master2.Species + " - " + master2.BreedName, isValid: true);

            status.SourceItem.Items.Should().HaveCount(3);
            status.SourceItem.InvalidCount.Should().Be(1);
            status.SourceItem.ValidCount.Should().Be(2);

            ValidateDataItem(status.SourceItem.Items.ElementAt(0), expectedValue: source1.Species + " - " + source1.BreedName, isValid: true);
            ValidateDataItem(status.SourceItem.Items.ElementAt(1), expectedValue: source2.Species + " - " + source2.BreedName, isValid: true);
            ValidateDataItem(status.SourceItem.Items.ElementAt(2), expectedValue: null, isValid: false);
        }

        [TestMethod]
        public async Task CopiesSourceToTarget()
        {
            BreedSource source1 = new BreedSource("1", "species1", "breed1");
            BreedSource source2 = new BreedSource("2", "species2", "breed2");

            Breed master1 = new Breed(2, "species2", "breed2");
            Breed master2 = new Breed(3, "species3", "breed3");


            List<Breed> entities = new List<Breed>();

            MockRescueGroupsStorageContext<BreedSource> mockSourceStorage = new MockRescueGroupsStorageContext<BreedSource>(source1, source2);

            MockSqlStorageContext<Breed> mockSqlStorage = new MockSqlStorageContext<Breed>(master1, master2);
            mockSqlStorage.Setup(x => x.Add(It.IsAny<Breed>())).Callback((Breed entity) =>
            {
                entities.Add(entity);
            });

            Mock<IStateManager<OrganizationContext>> mockStateManager = new Mock<IStateManager<OrganizationContext>>();

            BreedDataMapper mapper = new BreedDataMapper(mockSqlStorage.Object, mockSourceStorage.Object, mockStateManager.Object);
            CommandResult commandResult = await mapper.CopySourceToTarget();

            entities.Should().HaveCount(2);

            Breed result1 = entities.First();
            result1.ID.Should().Be(1);
            result1.Species.Should().Be(source1.Species);
            result1.BreedName.Should().Be(source1.BreedName);

            Breed result2 = entities.Last();
            result2.ID.Should().Be(2);
            result2.Species.Should().Be(source2.Species);
            result2.BreedName.Should().Be(source2.BreedName);

            mockSqlStorage.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}
