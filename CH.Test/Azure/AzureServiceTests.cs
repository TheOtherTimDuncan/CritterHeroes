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
        public async Task CanUploadAndDownloadBlobStreams()
        {
            string data = Faker.Lorem.Paragraph();
            MemoryStream memStream = new MemoryStream();
            StreamWriter writer = new StreamWriter(memStream);
            writer.Write(data);
            writer.Flush();

            string contentType = "application/txt";

            AzureService azureService = new AzureService(new AzureConfiguration(), mockOrganizationStateManger.Object);
            CloudBlockBlob blob = await azureService.UploadBlobAsync("stream", contentType, memStream);
            blob.Properties.ContentType.Should().Be(contentType);

            Stream result = new MemoryStream();
            await azureService.DownloadBlobAsync("stream", result);

            StreamReader reader = new StreamReader(result);
            string resultData = reader.ReadLine();
            resultData.Should().Be(data);
        }

        [TestMethod]
        public async Task CanUploadAndDownloadText()
        {
            string data = Faker.Lorem.Paragraph();

            AzureService azureService = new AzureService(new AzureConfiguration(), mockOrganizationStateManger.Object);
            await azureService.UploadBlobAsync("txt", data);

            string result = await azureService.DownloadBlobAsync("txt");
            result.Should().Be(data);
        }
    }
}
