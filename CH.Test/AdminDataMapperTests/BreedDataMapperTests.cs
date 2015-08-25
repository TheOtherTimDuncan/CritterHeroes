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

            Species species2 = new Species("species2", "singular2", "plural2", null, null);
            Species species3 = new Species("species3", "singular3", "plural3", null, null);

            Breed master2 = new Breed(species2, source2.BreedName, source2.ID);
            Breed master3 = new Breed(species3, "breed3");

            MockRescueGroupsStorageContext<BreedSource> mockSourceStorage = new MockRescueGroupsStorageContext<BreedSource>(source1, source2);

            MockSqlStorageContext<Breed> mockBreedStorage = new MockSqlStorageContext<Breed>(master2, master3);

            Mock<IStateManager<OrganizationContext>> mockStateManager = new Mock<IStateManager<OrganizationContext>>();

            MockSqlStorageContext<Species> mockSpeciesStorage = new MockSqlStorageContext<Species>();

            BreedDataMapper mapper = new BreedDataMapper(mockBreedStorage.Object, mockSourceStorage.Object, mockStateManager.Object, mockSpeciesStorage.Object);
            DashboardItemStatus status = await mapper.GetDashboardItemStatusAsync();

            status.TargetItem.Items.Should().HaveCount(3);
            status.TargetItem.InvalidCount.Should().Be(1);
            status.TargetItem.ValidCount.Should().Be(2);

            ValidateDataItem(status.TargetItem.Items.ElementAt(0), expectedValue: null, isValid: false);
            ValidateDataItem(status.TargetItem.Items.ElementAt(1), expectedValue: master2.Species.Name + " - " + master2.BreedName, isValid: true);
            ValidateDataItem(status.TargetItem.Items.ElementAt(2), expectedValue: master3.Species.Name + " - " + master3.BreedName, isValid: true);

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
            Species species1 = new Species("species1", "singular1", "plural1", null, null, speciesID: 91);
            Species species2 = new Species("species2", "singular2", "plural2", null, null, speciesID: 92);
            Species species3 = new Species("species3", "singular3", "plural3", null, null, speciesID: 93);

            BreedSource source1 = new BreedSource("1", species1.Name, "breed1");
            BreedSource source2 = new BreedSource("2", species2.Name, "breed2");

            Breed master2 = new Breed(species2, source2.BreedName, source2.ID);
            Breed master3 = new Breed(species3, "breed3");

            MockRescueGroupsStorageContext<BreedSource> mockSourceStorage = new MockRescueGroupsStorageContext<BreedSource>(source1, source2);

            MockSqlStorageContext<Breed> mockBreedStorage = new MockSqlStorageContext<Breed>(master2, master3);

            MockSqlStorageContext<Species> mockSpeciesStorage = new MockSqlStorageContext<Species>(species1, species2, species3);

            Mock<IStateManager<OrganizationContext>> mockStateManager = new Mock<IStateManager<OrganizationContext>>();

            BreedDataMapper mapper = new BreedDataMapper(mockBreedStorage.Object, mockSourceStorage.Object, mockStateManager.Object, mockSpeciesStorage.Object);
            CommandResult commandResult = await mapper.CopySourceToTarget();

            mockBreedStorage.Object.Entities.Should().HaveCount(2);

            Breed result1 = mockBreedStorage.Object.Entities.Single(x => x.RescueGroupsID == source1.ID);
            result1.BreedName.Should().Be(source1.BreedName);
            result1.SpeciesID.Should().Be(species1.ID);

            Breed result2 = mockBreedStorage.Object.Entities.Single(x => x.RescueGroupsID == source2.ID);
            result2.BreedName.Should().Be(source2.BreedName);
            result2.SpeciesID.Should().Be(species2.ID);
            result2.RescueGroupsID.Should().Be(source2.ID);

            mockBreedStorage.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}
