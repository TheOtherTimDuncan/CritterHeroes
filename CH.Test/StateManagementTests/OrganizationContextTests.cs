using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CH.Domain.Contracts;
using CH.Domain.Handlers;
using CH.Domain.Models;
using CH.Domain.Models.Data;
using CH.Domain.StateManagement;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test.StateManagementTests
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
                AzureName = "Azure",
                LogoFilename = "Logo",
                SupportedCritters = GetTestSupportedSpecies()
            };

            OrganizationStateManager stateManager = new OrganizationStateManager(GetMockHttpContext().Object);
            stateManager.SaveContext(context);
            OrganizationContext result = stateManager.GetContext();
            result.OrganizationID.Should().Be(context.OrganizationID);
            result.FullName.Should().Be(context.FullName);
            result.ShortName.Should().Be(context.ShortName);
            result.AzureName.Should().Be(context.AzureName);
            result.LogoFilename.Should().Be(context.LogoFilename);
            result.SupportedCritters.Should().Equal(context.SupportedCritters);
        }

        [TestMethod]
        public void CanCreateItselfFromOrganization()
        {
            Organization organization = new Organization()
            {
                FullName = "Full",
                ShortName = "Short",
                AzureName = "Azure",
                LogoFilename = "Logo",
                SupportedCritters = GetTestSupportedSpecies()
            };

            OrganizationContext context = OrganizationContext.FromOrganization(organization);

            context.OrganizationID.Should().Be(organization.ID);
            context.FullName.Should().Be(organization.FullName);
            context.ShortName.Should().Be(organization.ShortName);
            context.AzureName.Should().Be(organization.AzureName);
            context.LogoFilename.Should().Be(organization.LogoFilename);
            context.SupportedCritters.Should().Equal(organization.SupportedCritters);
        }

        [TestMethod]
        public void StateManagerReturnsNullIfFullNameIsMissing()
        {
            OrganizationContext context = new OrganizationContext()
            {
                FullName = null
            };

            Mock<IHttpContext> mockHttpContext = GetMockHttpContext();
            mockHttpContext.Object.Request.Cookies.Should().HaveCount(0);

            OrganizationStateManager stateManager = new OrganizationStateManager(mockHttpContext.Object);
            stateManager.SaveContext(context);
            mockHttpContext.Object.Request.Cookies.Should().HaveCount(1);
            stateManager.GetContext().Should().BeNull();
        }

        [TestMethod]
        public void StateManagerReturnsNullIfShortNameIsMissing()
        {
            OrganizationContext context = new OrganizationContext()
            {
                ShortName = null
            };

            Mock<IHttpContext> mockHttpContext = GetMockHttpContext();
            mockHttpContext.Object.Request.Cookies.Should().HaveCount(0);

            OrganizationStateManager stateManager = new OrganizationStateManager(mockHttpContext.Object);
            stateManager.SaveContext(context);
            mockHttpContext.Object.Request.Cookies.Should().HaveCount(1);
            stateManager.GetContext().Should().BeNull();
        }

        [TestMethod]
        public void StateManagerReturnsNullIfAzureNameIsMissing()
        {
            OrganizationContext context = new OrganizationContext()
            {
                AzureName = null
            };

            Mock<IHttpContext> mockHttpContext = GetMockHttpContext();
            mockHttpContext.Object.Request.Cookies.Should().HaveCount(0);

            OrganizationStateManager stateManager = new OrganizationStateManager(mockHttpContext.Object);
            stateManager.SaveContext(context);
            mockHttpContext.Object.Request.Cookies.Should().HaveCount(1);
            stateManager.GetContext().Should().BeNull();
        }

        [TestMethod]
        public void StateManagerReturnsNullIfSupportedCrittersNameIsMissing()
        {
            OrganizationContext context = new OrganizationContext()
            {
                SupportedCritters = new Species[] { }
            };

            Mock<IHttpContext> mockHttpContext = GetMockHttpContext();
            mockHttpContext.Object.Request.Cookies.Should().HaveCount(0);

            OrganizationStateManager stateManager = new OrganizationStateManager(mockHttpContext.Object);
            stateManager.SaveContext(context);
            mockHttpContext.Object.Request.Cookies.Should().HaveCount(1);
            stateManager.GetContext().Should().BeNull();
        }
    }
}
