using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CH.Test.Mocks;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.DataProviders.Azure.Services;
using CritterHeroes.Web.Domain.Contracts.StateManagement;
using CritterHeroes.Web.Domain.Contracts.Storage;
using CritterHeroes.Web.Shared.StateManagement;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage.Blob;
using Moq;

namespace CH.Test.Azure
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
                OrganizationID = org.ID
            };

            string filename = "logo.svg";
            string contentType = "image/svg+xml";
            MemoryStream stream = new MemoryStream();
            bool isPrivate = false;

            Mock<IStateManager<OrganizationContext>> mockOrgStateManager = new Mock<IStateManager<OrganizationContext>>();
            mockOrgStateManager.Setup(x => x.GetContext()).Returns(orgContext);

            MockSqlCommandStorageContext<Organization> mockOrgStorageContext = new MockSqlCommandStorageContext<Organization>(org);

            Mock<IAzureService> mockAzureService = new Mock<IAzureService>();
            mockAzureService.Setup(x => x.UploadBlobAsync(filename, isPrivate, contentType, stream)).Returns((string path, bool callbackIsPrivate, string callbackContentType, Stream callbackStream) =>
            {
                path.Should().Be(filename);

                CloudBlockBlob blob = new CloudBlockBlob(new Uri("http://localhost/container"));
                return Task.FromResult(blob);
            });

            OrganizationLogoService logoService = new OrganizationLogoService(mockOrgStateManager.Object, mockAzureService.Object, mockOrgStorageContext.Object);
            await logoService.SaveLogo(stream, filename, contentType);

            org.LogoFilename.Should().Be(filename);

            mockOrgStorageContext.Verify(x => x.SaveChangesAsync(), Times.Once);
            mockAzureService.Verify(x => x.UploadBlobAsync(filename, isPrivate, contentType, stream), Times.Once);
        }

        [TestMethod]
        public async Task DeletesOldLogoBeforeUploadingNewLogo()
        {
            Organization org = new Organization() ;

            string oldFilename = "old.svg";

            OrganizationContext orgContext = new OrganizationContext()
            {
                OrganizationID = org.ID,
                LogoFilename= oldFilename
            };

            string filename = "logo.svg";
            string contentType = "image/svg+xml";
            MemoryStream stream = new MemoryStream();
            bool isPrivate = false;

            Mock<IStateManager<OrganizationContext>> mockOrgStateManager = new Mock<IStateManager<OrganizationContext>>();
            mockOrgStateManager.Setup(x => x.GetContext()).Returns(orgContext);

            MockSqlCommandStorageContext<Organization> mockOrgStorageContext = new MockSqlCommandStorageContext<Organization>(org);

            Mock<IAzureService> mockAzureService = new Mock<IAzureService>();
            mockAzureService.Setup(x => x.UploadBlobAsync(filename, isPrivate, contentType, stream)).Returns((string path, bool callbackIsPrivate, string callbackContentType, Stream callbackStream) =>
            {
                path.Should().Be(filename);

                CloudBlockBlob blob = new CloudBlockBlob(new Uri("http://localhost/container"));
                return Task.FromResult(blob);
            });

            OrganizationLogoService logoService = new OrganizationLogoService(mockOrgStateManager.Object, mockAzureService.Object, mockOrgStorageContext.Object);
            await logoService.SaveLogo(stream, filename, contentType);

            org.LogoFilename.Should().Be(filename);

            mockOrgStorageContext.Verify(x => x.SaveChangesAsync(), Times.Once);
            mockAzureService.Verify(x => x.UploadBlobAsync(filename, isPrivate, contentType, stream), Times.Once);
            mockAzureService.Verify(x => x.DeleteBlobAsync(oldFilename, isPrivate), Times.Once);
        }
    }
}
