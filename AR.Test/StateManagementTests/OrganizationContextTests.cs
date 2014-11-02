using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AR.Domain.Contracts;
using AR.Domain.Handlers;
using AR.Domain.Models;
using AR.Domain.StateManagement;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AR.Test.StateManagementTests
{
    [TestClass]
    public class OrganizationContextTests : BaseContextTest
    {
        [TestMethod]
        public void CanGetAndSaveOrganizationContext()
        {
            OrganizationContext context = new OrganizationContext()
            {
                OrganizationID = Guid.NewGuid(),
                FullName = "Full",
                ShortName = "Short",
                AzureTableName = "Azure",
                SupportedCritters = new string[] { "Critter1", "Critter2" }
            };

            OrganizationStateManager stateManager = new OrganizationStateManager(GetMockHttpContext().Object);
            stateManager.SaveContext(context);
            OrganizationContext result = stateManager.GetContext();
            result.OrganizationID.Should().Be(context.OrganizationID);
            result.FullName.Should().Be(context.FullName);
            result.ShortName.Should().Be(context.ShortName);
            result.AzureTableName.Should().Be(context.AzureTableName);
            result.SupportedCritters.Should().Equal(context.SupportedCritters);
        }

        [TestMethod]
        public void CanCreateItselfFromOrganization()
        {
            Organization organization = new Organization()
            {
                FullName = "Full",
                ShortName = "Short",
                AzureTableName = "Azure",
                SupportedCritters = new string[] { "Critter1", "Critter2" }
            };

            OrganizationContext context = OrganizationContext.FromOrganization(organization);

            context.OrganizationID.Should().Be(organization.ID);
            context.FullName.Should().Be(organization.FullName);
            context.ShortName.Should().Be(organization.ShortName);
            context.AzureTableName.Should().Be(organization.AzureTableName);
            context.SupportedCritters.Should().Equal(organization.SupportedCritters);
        }

        [TestMethod]
        public async Task CanCreateAndSaveContextFromStorage()
        {
            Organization organization = new Organization()
            {
                FullName = "Full",
                ShortName = "Short",
                AzureTableName = "Azure",
                SupportedCritters = new string[] { "Critter1", "Critter2" }
            };

            OrganizationContext context = OrganizationContext.FromOrganization(organization);

            Mock<IStateManager<OrganizationContext>> mockStateManager = new Mock<IStateManager<OrganizationContext>>();
            mockStateManager.Setup(x => x.SaveContext(It.IsAny<OrganizationContext>())).Callback<OrganizationContext>((callbackContext) =>
            {
                callbackContext.OrganizationID.Should().Be(context.OrganizationID);
                callbackContext.FullName.Should().Be(context.FullName);
                callbackContext.ShortName.Should().Be(context.ShortName);
                callbackContext.AzureTableName.Should().Be(context.AzureTableName);
                callbackContext.SupportedCritters.Should().Equal(context.SupportedCritters);
            });

            Mock<IStorageContext<Organization>> mockStorageContext = new Mock<IStorageContext<Organization>>();
            mockStorageContext.Setup(x => x.GetAsync(It.IsAny<string>())).Returns<string>((organizationID) =>
            {
                organizationID.Should().Be(organization.ID.ToString());
                return Task.FromResult(organization);
            });

            Mock<IAppConfiguration> mockConfiguration = new Mock<IAppConfiguration>();
            mockConfiguration.Setup(x => x.OrganizationID).Returns(organization.ID);

            OrganizationContext result = await new CreateOrganizationContext(mockConfiguration.Object, mockStorageContext.Object, mockStateManager.Object).ExecuteAsync();
            result.OrganizationID.Should().Be(context.OrganizationID);
            result.FullName.Should().Be(context.FullName);
            result.ShortName.Should().Be(context.ShortName);
            result.AzureTableName.Should().Be(context.AzureTableName);
            result.SupportedCritters.Should().Equal(context.SupportedCritters);

            mockStateManager.Verify(x => x.SaveContext(It.IsAny<OrganizationContext>()), Times.Once);
        }
    }
}
