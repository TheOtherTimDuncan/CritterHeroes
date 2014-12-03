using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Domain.Contracts;
using CH.Domain.Models;
using CH.Domain.Queries;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test.QueryTests
{
    [TestClass]
    public class OrganizationQueryTests : BaseTest
    {
        [TestMethod]
        public async Task ReturnsOrganization()
        {
            Organization organization = new Organization()
            {
                ShortName = "short",
                FullName = "full",
                AzureName = "azure",
                LogoFilename = "logo",
                SupportedCritters = GetTestSupportedSpecies()
            };

            Mock<IStorageContext<Organization>> mockStorageContext = new Mock<IStorageContext<Organization>>();
            mockStorageContext.Setup(x => x.GetAsync(organization.ID.ToString())).Returns(Task.FromResult(organization));

            OrganizationQueryHandler queryHandler = new OrganizationQueryHandler(mockStorageContext.Object);
            Organization resultOrganization = await queryHandler.Retrieve(new OrganizationQuery()
            {
                OrganizationID = organization.ID
            });

            resultOrganization.Should().Equals(organization);

            mockStorageContext.Verify(x => x.GetAsync(organization.ID.ToString()), Times.Once);
        }
    }
}
