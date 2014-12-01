using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CH.Domain.Contracts;
using CH.Domain.Models;
using CH.Domain.Models.Data;
using CH.Domain.Queries;
using CH.Domain.StateManagement;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test.QueryTests
{
    [TestClass]
    public class OrganizationContextQueryTests : BaseTest
    {
        [TestMethod]
        public async Task ReturnsExistingOrganizationContextIfContextExists()
        {
            OrganizationContext organizationContext = new OrganizationContext()
            {
                OrganizationID = Guid.NewGuid(),
                ShortName = "short",
                FullName = "full",
                AzureName = "azure",
                LogoFilename = "logo"
            };

            Mock<IStateManager<OrganizationContext>> mockStateManager = new Mock<IStateManager<OrganizationContext>>();
            mockStateManager.Setup(x => x.GetContext()).Returns(organizationContext);

            OrganizationContextQueryHandler queryHandler = new OrganizationContextQueryHandler(mockStateManager.Object, null);
            OrganizationContext resultContext = await queryHandler.Retrieve(new OrganizationQuery()
            {
                OrganizationID = organizationContext.OrganizationID
            });
            resultContext.Should().Be(organizationContext);

            mockStateManager.Verify(x => x.GetContext(), Times.Once);
        }

        [TestMethod]
        public async Task CreatesAndReturnsOrganizationContextIfContextDoesNotExist()
        {
            Organization organization = new Organization()
            {
                ShortName = "short",
                FullName = "full",
                AzureName = "azure",
                LogoFilename = "logo",
                SupportedCritters = GetTestSupportedSpecies()
            };

            Mock<IStateManager<OrganizationContext>> mockStateManager = new Mock<IStateManager<OrganizationContext>>();
            mockStateManager.Setup(x => x.GetContext()).Returns((OrganizationContext)null);

            Mock<IStorageContext<Organization>> mockStorageContext = new Mock<IStorageContext<Organization>>();
            mockStorageContext.Setup(x => x.GetAsync(organization.ID.ToString())).Returns(Task.FromResult(organization));

            OrganizationContextQueryHandler queryHandler = new OrganizationContextQueryHandler(mockStateManager.Object, mockStorageContext.Object);
            OrganizationContext organizationContext = await queryHandler.Retrieve(new OrganizationQuery()
            {
                OrganizationID = organization.ID
            });

            organizationContext.OrganizationID.Should().Be(organization.ID);

            mockStateManager.Verify(x => x.GetContext(), Times.Once);
            mockStorageContext.Verify(x => x.GetAsync(organization.ID.ToString()), Times.Once);
        }
    }
}
