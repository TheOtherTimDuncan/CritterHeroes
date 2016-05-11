using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.DataProviders.Azure;
using CritterHeroes.Web.Shared.StateManagement;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using Moq;

namespace CH.Test.Azure
{
    [TestClass]
    public class AzureServiceTests
    {
        private Mock<IStateManager<OrganizationContext>> mockOrganizationStateManger;
        private OrganizationContext orgContext;

        private Mock<IAppConfiguration> mockAppConfiguration;

        private const string tableName = "unittest";

        [TestInitialize]
        public void InitializeTest()
        {
            orgContext = new OrganizationContext()
            {
                AzureName = "unittest"
            };

            mockOrganizationStateManger = new Mock<IStateManager<OrganizationContext>>();
            mockOrganizationStateManger.Setup(x => x.GetContext()).Returns(orgContext);

            mockAppConfiguration = new Mock<IAppConfiguration>();
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
            string blobPath = "stream";

            AzureService azureService = new AzureService(new AzureConfiguration(), mockOrganizationStateManger.Object, mockAppConfiguration.Object);

            bool blobExists = await azureService.BlobExistsAsync(blobPath, isPrivate);
            blobExists.Should().BeFalse();

            CloudBlockBlob blob = await azureService.UploadBlobAsync(blobPath, isPrivate, contentType, memStream);
            blob.Properties.ContentType.Should().Be(contentType);

            blobExists = await azureService.BlobExistsAsync(blobPath, isPrivate);
            blobExists.Should().BeTrue();

            BlobContainerPermissions permissions = await blob.Container.GetPermissionsAsync();
            permissions.PublicAccess.Should().Be(BlobContainerPublicAccessType.Off);

            Stream result = new MemoryStream();
            await azureService.DownloadBlobAsync(blobPath, isPrivate, result);

            StreamReader reader = new StreamReader(result);
            string resultData = reader.ReadLine();
            resultData.Should().Be(data);

            await azureService.DeleteBlobAsync(blobPath, isPrivate);

            blobExists = await azureService.BlobExistsAsync(blobPath, isPrivate);
            blobExists.Should().BeFalse();
        }

        [TestMethod]
        public async Task CanUploadAndDownloadPrivateBlobText()
        {
            string data = Faker.Lorem.Paragraph();
            bool isPrivate = true;

            AzureService azureService = new AzureService(new AzureConfiguration(), mockOrganizationStateManger.Object, mockAppConfiguration.Object);
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

            // Upload with uppercase and download with lowercase to make sure case is forced to lower
            string path = "stream";

            AzureService azureService = new AzureService(new AzureConfiguration(), mockOrganizationStateManger.Object, mockAppConfiguration.Object);
            CloudBlockBlob blob = await azureService.UploadBlobAsync(path.ToUpper(), isPrivate, contentType, memStream);
            blob.Properties.ContentType.Should().Be(contentType);

            BlobContainerPermissions permissions = await blob.Container.GetPermissionsAsync();
            permissions.PublicAccess.Should().Be(BlobContainerPublicAccessType.Blob);

            Stream result = new MemoryStream();
            await azureService.DownloadBlobAsync(path, isPrivate, result);

            StreamReader reader = new StreamReader(result);
            string resultData = reader.ReadLine();
            resultData.Should().Be(data);
        }

        [TestMethod]
        public async Task CanUploadAndDownloadPublicBlobText()
        {
            string data = Faker.Lorem.Paragraph();
            bool isPrivate = false;

            // Upload with uppercase and download with lowercase to make sure case is forced to lower
            string path = "txt";

            AzureService azureService = new AzureService(new AzureConfiguration(), mockOrganizationStateManger.Object, mockAppConfiguration.Object);
            CloudBlockBlob blob = await azureService.UploadBlobAsync(path.ToUpper(), isPrivate, data);

            BlobContainerPermissions permissions = await blob.Container.GetPermissionsAsync();
            permissions.PublicAccess.Should().Be(BlobContainerPublicAccessType.Blob);

            string result = await azureService.DownloadBlobAsync(path, isPrivate);
            result.Should().Be(data);
        }

        [TestMethod]
        public void CreateBlobUrlReturnsUrlForBlob()
        {
            orgContext.AzureName = orgContext.AzureName.ToUpper();

            string path = "PATH";
            string basePath = "BASE";

            mockAppConfiguration.Setup(x => x.BlobBaseUrl).Returns(basePath);

            AzureService azureService = new AzureService(new AzureConfiguration(), mockOrganizationStateManger.Object, mockAppConfiguration.Object);
            string url = azureService.CreateBlobUrl(path);
            url.Should().Be($"{basePath}/{orgContext.AzureName}/{path}".ToLower(), "azure blob urls should be forced to lower case");
        }

        [TestMethod]
        public async Task CanReadFromAndWriteToTableStorage()
        {
            string partitionKey = "partitionkey";
            string rowKey = "rowkey";

            DynamicTableEntity entity = new DynamicTableEntity(partitionKey, rowKey);
            entity.Properties["Test"] = new EntityProperty("value");

            AzureService azureService = new AzureService(new AzureConfiguration(), mockOrganizationStateManger.Object, mockAppConfiguration.Object);

            TableOperation insertOperation = TableOperation.InsertOrReplace(entity);
            TableResult insertResult = await azureService.ExecuteTableOperationAsync(tableName, insertOperation);
            insertResult.HttpStatusCode.Should().Be((int)HttpStatusCode.NoContent);

            TableOperation retrieveOperation = TableOperation.Retrieve<DynamicTableEntity>(partitionKey, rowKey);
            TableResult retrieveResult = await azureService.ExecuteTableOperationAsync(tableName, retrieveOperation);
            retrieveResult.HttpStatusCode.Should().Be((int)HttpStatusCode.OK);

            DynamicTableEntity resultEntity = retrieveResult.Result as DynamicTableEntity;
            resultEntity.Should().NotBeNull();
            resultEntity.Properties["Test"].StringValue.Should().Be(entity.Properties["Test"].StringValue);

            TableOperation deleteOperation = TableOperation.Delete(entity);
            TableResult deleteResult = await azureService.ExecuteTableOperationAsync(tableName, deleteOperation);
            deleteResult.HttpStatusCode.Should().Be((int)HttpStatusCode.NoContent);
        }

        [TestMethod]
        public async Task CanExecuteTableBatchOperationsAndTableQueries()
        {
            string partitionKey = "partitionkey";

            IEnumerable<DynamicTableEntity> entities = Enumerable.Range(1, 200).Select(x =>
            {
                DynamicTableEntity entity = new DynamicTableEntity(partitionKey, x.ToString());
                entity.Properties["Test"] = new EntityProperty(x);
                return entity;
            }).ToList();

            AzureService azureService = new AzureService(new AzureConfiguration(), mockOrganizationStateManger.Object, mockAppConfiguration.Object);

            await azureService.ExecuteTableBatchOperationAsync(tableName, entities, (entity) => TableOperation.InsertOrReplace(entity));

            var query =
                from x in await azureService.CreateTableQuery<DynamicTableEntity>(tableName)
                where x.PartitionKey == partitionKey
                select x;

            foreach (DynamicTableEntity resultEntity in query)
            {
                resultEntity.RowKey.Should().Be(resultEntity.Properties["Test"].Int32Value.Value.ToString());
            }

            await azureService.ExecuteTableBatchOperationAsync(tableName, entities, (entity) => TableOperation.Delete(entity));
        }
    }
}
