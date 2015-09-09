using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.Proxies.Configuration;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.StateManagement;
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
    public class CritterPictureServiceTests
    {
        [TestMethod]
        public async Task CanSaveImage()
        {
            Organization org = new Organization();

            OrganizationContext orgContext = new OrganizationContext()
            {
                OrganizationID = org.ID,
                AzureName = "fflah"
            };

            string filename = "3671153_448x336.jpg";
            string filepath = Path.Combine(UnitTestHelper.GetSolutionRoot(), ".vs", "Sample Data", "Pictures", filename);

            AppConfiguration appConfiguration = new AppConfiguration();
            AzureConfiguration azureConfiguration = new AzureConfiguration();

            Mock<IStateManager<OrganizationContext>> mockOrgStateManager = new Mock<IStateManager<OrganizationContext>>();
            mockOrgStateManager.Setup(x => x.GetContext()).Returns(orgContext);

            CritterPictureService service = new CritterPictureService(mockOrgStateManager.Object, appConfiguration, azureConfiguration);

            using (FileStream stream = new FileStream(filepath, FileMode.Open))
            {
                await service.SavePictureAsync(stream, 0, filename,"image/jpeg");
            }
        }
    }
}
