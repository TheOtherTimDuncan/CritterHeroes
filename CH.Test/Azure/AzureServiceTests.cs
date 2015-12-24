using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.DataProviders.Azure;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage.Blob;
using Moq;

namespace CH.Test.Azure
{
    [TestClass]
    public class AzureServiceTests
    {
        private Mock<IStateManager<OrganizationContext>> mockOrganizationStateManger;
        private OrganizationContext orgContext;

        [TestInitialize]
        public void InitializeTest()
        {
            orgContext = new OrganizationContext()
            {
                AzureName = "unittest"
            };

            mockOrganizationStateManger = new Mock<IStateManager<OrganizationContext>>();
            mockOrganizationStateManger.Setup(x => x.GetContext()).Returns(orgContext);
        }

        [TestMethod]
        public async Task CanUploadAndDownloadPrivateBlobStreams()
        {
            string data = Faker.Lorem.Paragraph();
            MemoryStream memStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(memStream);
            writer.Write(data);
            writer.Flush();

            string contentType = "application/txt";
            bool isPrivate = true;

            AzureService azureService = new AzureService(new AzureConfiguration(), mockOrganizationStateManger.Object);
            CloudBlockBlob blob = await azureService.UploadBlobAsync("stream", isPrivate, contentType, memStream);
            blob.Properties.ContentType.Should().Be(contentType);

            BlobContainerPermissions permissions = await blob.Container.GetPermissionsAsync();
            permissions.PublicAccess.Should().Be(BlobContainerPublicAccessType.Off);

            Stream result = new MemoryStream();
            await azureService.DownloadBlobAsync("stream", isPrivate, result);

            StreamReader reader = new StreamReader(result);
            string resultData = reader.ReadLine();
            resultData.Should().Be(data);
        }

        [TestMethod]
        public async Task CanUploadAndDownloadPrivateBlobText()
        {
            string data = Faker.Lorem.Paragraph();
            bool isPrivate = true;

            AzureService azureService = new AzureService(new AzureConfiguration(), mockOrganizationStateManger.Object);
            CloudBlockBlob blob = await azureService.UploadBlobAsync("txt", isPrivate, data);

            BlobContainerPermissions permissions = await blob.Container.GetPermissionsAsync();
            permissions.PublicAccess.Should().Be(BlobContainerPublicAccessType.Off);

            string result = await azureService.DownloadBlobAsync("txt", isPrivate);
            result.Should().Be(data);
        }

        [TestMethod]
        public async Task CanUploadAndDownloadPublicBlobStreams()
        {
            string data = Faker.Lorem.Paragraph();
            MemoryStream memStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(memStream);
            writer.Write(data);
            writer.Flush();

            string contentType = "application/txt";
            bool isPrivate = false;

            AzureService azureService = new AzureService(new AzureConfiguration(), mockOrganizationStateManger.Object);
            CloudBlockBlob blob = await azureService.UploadBlobAsync("stream", isPrivate, contentType, memStream);
            blob.Properties.ContentType.Should().Be(contentType);

            BlobContainerPermissions permissions = await blob.Container.GetPermissionsAsync();
            permissions.PublicAccess.Should().Be(BlobContainerPublicAccessType.Blob);

            Stream result = new MemoryStream();
            await azureService.DownloadBlobAsync("stream", isPrivate, result);

            StreamReader reader = new StreamReader(result);
            string resultData = reader.ReadLine();
            resultData.Should().Be(data);
        }

        [TestMethod]
        public async Task CanUploadAndDownloadPublicBlobText()
        {
            string data = Faker.Lorem.Paragraph();
            bool isPrivate = false;

            AzureService azureService = new AzureService(new AzureConfiguration(), mockOrganizationStateManger.Object);
            CloudBlockBlob blob = await azureService.UploadBlobAsync("txt", isPrivate, data);

            BlobContainerPermissions permissions = await blob.Container.GetPermissionsAsync();
            permissions.PublicAccess.Should().Be(BlobContainerPublicAccessType.Blob);

            string result = await azureService.DownloadBlobAsync("txt", isPrivate);
            result.Should().Be(data);
        }
    }
}
