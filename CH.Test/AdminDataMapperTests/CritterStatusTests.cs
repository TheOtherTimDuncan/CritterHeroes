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
    public class CritterStatusTests : BaseDataMapperTest
    {
        [TestMethod]
        public async Task ReturnsCollectionOfUniqueItems()
        {
            CritterStatusSource source1 = new CritterStatusSource("1", "name1", "description1");
            CritterStatusSource source2 = new CritterStatusSource("2", "name2", "description2");

            CritterStatus master2 = new CritterStatus(source2.Name, source2.Description, source2.ID);
            CritterStatus master3 = new CritterStatus("name3", "description3");

            MockRescueGroupsStorageContext<CritterStatusSource> mockSourceStorage = new MockRescueGroupsStorageContext<CritterStatusSource>(source1, source2);

            MockSqlStorageContext<CritterStatus> mockSqlStorage = new MockSqlStorageContext<CritterStatus>(master2, master3);

            Mock<IStateManager<OrganizationContext>> mockStateManager = new Mock<IStateManager<OrganizationContext>>();

            CritterStatusMapper mapper = new CritterStatusMapper(mockSqlStorage.Object, mockSourceStorage.Object, mockStateManager.Object);
            DashboardItemStatus status = await mapper.GetDashboardItemStatusAsync();

            status.TargetItem.Items.Should().HaveCount(3);
            status.TargetItem.InvalidCount.Should().Be(1);
            status.TargetItem.ValidCount.Should().Be(2);

            ValidateDataItem(status.TargetItem.Items.ElementAt(0), expectedValue: null, isValid: false);
            ValidateDataItem(status.TargetItem.Items.ElementAt(1), expectedValue: master2.Name, isValid: true);
            ValidateDataItem(status.TargetItem.Items.ElementAt(2), expectedValue: master3.Name, isValid: true);

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
            CritterStatusSource source1 = new CritterStatusSource("1", "name1", "description1");
            CritterStatusSource source2 = new CritterStatusSource("2", "name2", "description2");

            CritterStatus master2 = new CritterStatus(source2.Name, source2.Description, source2.ID);
            CritterStatus master3 = new CritterStatus("name3", "description3");

            MockRescueGroupsStorageContext<CritterStatusSource> mockSourceStorage = new MockRescueGroupsStorageContext<CritterStatusSource>(source1, source2);

            MockSqlStorageContext<CritterStatus> mockSqlStorage = new MockSqlStorageContext<CritterStatus>(master2, master3);

            Mock<IStateManager<OrganizationContext>> mockStateManager = new Mock<IStateManager<OrganizationContext>>();

            CritterStatusMapper mapper = new CritterStatusMapper(mockSqlStorage.Object, mockSourceStorage.Object, mockStateManager.Object);
            CommandResult commandResult = await mapper.CopySourceToTarget();

            mockSqlStorage.Object.Entities.Should().HaveCount(2);

            CritterStatus result1 = mockSqlStorage.Object.Entities.Single(x => x.RescueGroupsID == source1.ID);
            result1.Name.Should().Be(source1.Name);
            result1.Description.Should().Be(source1.Description);

            CritterStatus result2 = mockSqlStorage.Object.Entities.Single(x => x.RescueGroupsID == source2.ID);
            result2.Name.Should().Be(source2.Name);
            result2.Description.Should().Be(source2.Description);

            mockSqlStorage.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}
