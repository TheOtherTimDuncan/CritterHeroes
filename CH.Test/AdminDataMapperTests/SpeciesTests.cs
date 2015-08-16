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
    public class SpeciesTests : BaseDataMapperTest
    {
        [TestMethod]
        public async Task ReturnsCollectionOfUniqueItems()
        {
            SpeciesSource source1 = new SpeciesSource("name1", "singular1", "plural1", null, null);
            SpeciesSource source2 = new SpeciesSource("name2", "singular2", "plural2", null, null);

            Species master1 = new Species("name2", "singular1", "plural1", null, null);
            Species master2 = new Species("name3", "singular3", "plural3", null, null);

            MockRescueGroupsStorageContext<SpeciesSource> mockSourceStorage = new MockRescueGroupsStorageContext<SpeciesSource>(source1, source2);

            MockSqlStorageContext<Species> mockSqlStorage = new MockSqlStorageContext<Species>(master1, master2);

            Mock<IStateManager<OrganizationContext>> mockStateManager = new Mock<IStateManager<OrganizationContext>>();

            SpeciesMapper mapper = new SpeciesMapper(mockSqlStorage.Object, mockSourceStorage.Object, mockStateManager.Object);
            DashboardItemStatus status = await mapper.GetDashboardItemStatusAsync();

            status.TargetItem.Items.Should().HaveCount(3);
            status.TargetItem.InvalidCount.Should().Be(1);
            status.TargetItem.ValidCount.Should().Be(2);

            ValidateDataItem(status.TargetItem.Items.ElementAt(0), expectedValue: null, isValid: false);
            ValidateDataItem(status.TargetItem.Items.ElementAt(1), expectedValue: master1.Name, isValid: true);
            ValidateDataItem(status.TargetItem.Items.ElementAt(2), expectedValue: master2.Name, isValid: true);

            status.SourceItem.Items.Should().HaveCount(3);
            status.SourceItem.InvalidCount.Should().Be(1);
            status.SourceItem.ValidCount.Should().Be(2);

            ValidateDataItem(status.SourceItem.Items.ElementAt(0), expectedValue: source1.Name, isValid: true);
            ValidateDataItem(status.SourceItem.Items.ElementAt(1), expectedValue: source2.Name, isValid: true);
            ValidateDataItem(status.SourceItem.Items.ElementAt(2), expectedValue: null, isValid: false);
        }

        [TestMethod]
        public async Task CopiesSourceToTarget()
        {
            SpeciesSource source1 = new SpeciesSource("name1", "singular1", "plural1", null, null);
            SpeciesSource source2 = new SpeciesSource("name2", "singular2", "plural2", null, null);

            Species master1 = new Species("name2", "singular1", "plural1", null, null);
            Species master2 = new Species("name3", "singular3", "plural3", null, null);

            List<Species> entities = new List<Species>();

            MockRescueGroupsStorageContext<SpeciesSource> mockSourceStorage = new MockRescueGroupsStorageContext<SpeciesSource>(source1, source2);

            MockSqlStorageContext<Species> mockSqlStorage = new MockSqlStorageContext<Species>(master1, master2);
            mockSqlStorage.Setup(x => x.Add(It.IsAny<Species>())).Callback((Species entity) =>
            {
                entities.Add(entity);
            });

            Mock<IStateManager<OrganizationContext>> mockStateManager = new Mock<IStateManager<OrganizationContext>>();

            SpeciesMapper mapper = new SpeciesMapper(mockSqlStorage.Object, mockSourceStorage.Object, mockStateManager.Object);
            CommandResult commandResult = await mapper.CopySourceToTarget();

            entities.Should().HaveCount(2);

            Species result1 = entities.First();
            result1.Name.Should().Be(source1.Name);
            result1.Singular.Should().Be(source1.Singular);
            result1.YoungSingular.Should().Be(source1.YoungSingular);
            result1.Plural.Should().Be(source1.Plural);
            result1.YoungPlural.Should().Be(source1.YoungPlural);

            Species result2 = entities.Last();
            result2.Name.Should().Be(source2.Name);
            result2.Singular.Should().Be(source2.Singular);
            result2.YoungSingular.Should().Be(source2.YoungSingular);
            result2.Plural.Should().Be(source2.Plural);
            result2.YoungPlural.Should().Be(source2.YoungPlural);

            mockSqlStorage.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}
