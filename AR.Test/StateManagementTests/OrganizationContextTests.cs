using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AR.Domain.Contracts;
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
    }
}
