using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CH.Test.Mocks;
using CritterHeroes.Web.Common.Proxies.Configuration;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.DataProviders.Azure;
using CritterHeroes.Web.DataProviders.Azure.Storage;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TOTD.Utility.Misc;

namespace CH.Test.Azure.StorageTests
{
    [TestClass]
    public class OrganizationLogoServiceTests
    {
        [TestMethod]
        public async Task CanSaveLogo()
        {
            Organization org = new Organization();

            OrganizationContext orgContext = new OrganizationContext()
            {
                OrganizationID = org.ID,
                AzureName = "fflah"
            };

            string filename = "logo.svg";
            string filepath = Path.Combine(UnitTestHelper.GetSolutionRoot(), "CH.Test", "TestFiles", filename);

            AppConfiguration appConfiguration = new AppConfiguration();
            AzureConfiguration azureConfiguration = new AzureConfiguration();

            Mock<IStateManager<OrganizationContext>> mockOrgStateManager = new Mock<IStateManager<OrganizationContext>>();
            mockOrgStateManager.Setup(x => x.GetContext()).Returns(orgContext);

            MockSqlStorageContext<Organization> mockOrgStorageContext = new MockSqlStorageContext<Organization>(org);

            OrganizationLogoService logoService = new OrganizationLogoService(mockOrgStateManager.Object, appConfiguration, azureConfiguration, mockOrgStorageContext.Object);

            using (FileStream stream = new FileStream(filepath, FileMode.Open))
            {
                await logoService.SaveLogo(stream, filename, "image/svg+xml");
            }

            org.LogoFilename.Should().Be(filename);

            mockOrgStorageContext.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
    }
}
