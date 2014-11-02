﻿using System;
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
                AzureName = "Azure",
                LogoFilename = "Logo",
                SupportedCritters = new string[] { "Critter1", "Critter2" }
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
                SupportedCritters = new string[] { "Critter1", "Critter2" }
            };

            OrganizationContext context = OrganizationContext.FromOrganization(organization);

            context.OrganizationID.Should().Be(organization.ID);
            context.FullName.Should().Be(organization.FullName);
            context.ShortName.Should().Be(organization.ShortName);
            context.AzureName.Should().Be(organization.AzureName);
            context.LogoFilename.Should().Be(organization.LogoFilename);
            context.SupportedCritters.Should().Equal(organization.SupportedCritters);
        }
    }
}
