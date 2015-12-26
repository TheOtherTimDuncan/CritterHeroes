using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Contracts.Storage;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.DataProviders.Azure
{
    public class AzureService : IAzureService
    {
        private IAzureConfiguration _configuration;
        private IStateManager<OrganizationContext> _orgStateManager;
        private IAppConfiguration _appConfiguration;

        private CloudTable _cloudTable = null;
        private CloudBlobContainer _container;
        private string _containerName;

        private const int batchSize = 100;

        public AzureService(IAzureConfiguration azureConfiguration, IStateManager<OrganizationContext> orgStateManager, IAppConfiguration appConfiguration)
        {
            ThrowIf.Argument.IsNull(azureConfiguration, nameof(azureConfiguration));
            ThrowIf.Argument.IsNull(orgStateManager, nameof(orgStateManager));
            ThrowIf.Argument.IsNull(appConfiguration, nameof(appConfiguration));

            this._configuration = azureConfiguration;
            this._orgStateManager = orgStateManager;
            this._appConfiguration = appConfiguration;
        }

        public string CreateBlobUrl(string path)
        {
            return FixCaseForBlobPath($"{_appConfiguration.BlobBaseUrl}/{GetContainerName()}/{path}");
        }

        public async Task<bool> DeleteBlobAsync(string path, bool isPrivate)
        {
            CloudBlockBlob blob = await GetBlockBlobAsync(path, isPrivate);
            if (blob != null)
            {
                return await blob.DeleteIfExistsAsync();
            }
            return true;
        }

        public async Task<CloudBlockBlob> UploadBlobAsync(string path, bool isPrivate, string contentType, Stream source)
        {
            // Ensure stream is at the beginning
            source.Position = 0;

            CloudBlockBlob blob = await GetBlockBlobAsync(path, isPrivate);
            blob.Properties.ContentType = contentType;
            await blob.UploadFromStreamAsync(source);

            return blob;
        }

        public async Task<CloudBlockBlob> UploadBlobAsync(string path, bool isPrivate, string content)
        {
            CloudBlockBlob blob = await GetBlockBlobAsync(path, isPrivate);
            await blob.UploadTextAsync(content);
            return blob;
        }

        public async Task<string> DownloadBlobAsync(string path, bool isPrivate)
        {
            CloudBlockBlob blob = await GetBlockBlobAsync(path, isPrivate);
            string data = await blob.DownloadTextAsync();
            return data;
        }

        public async Task DownloadBlobAsync(string path, bool isPrivate, Stream target)
        {
            CloudBlockBlob blob = await GetBlockBlobAsync(path, isPrivate);
            await blob.DownloadToStreamAsync(target);

            // Ensure stream is at the beginning
            target.Position = 0;
        }

        public string GetLoggingKey()
        {
            return GetLoggingKey(DateTime.UtcNow);
        }

        public string GetLoggingKey(DateTime logDateUtc)
        {
            return new DateTime(logDateUtc.Year, logDateUtc.Month, logDateUtc.Day, logDateUtc.Hour, logDateUtc.Minute, 0, DateTimeKind.Utc).Ticks.ToString("d19");
        }

        public async Task<TableResult> ExecuteTableOperationAsync(string tableName, TableOperation operation)
        {
            CloudTable table = await GetCloudTable(tableName);
            TableResult tableResult = await table.ExecuteAsync(operation);
            return tableResult;
        }

        public async Task ExecuteTableBatchOperationAsync(string tableName, IEnumerable<ITableEntity> tableEntities, Func<ITableEntity, TableOperation> operationFactory)
        {
            CloudTable table = await GetCloudTable(tableName);

            TableBatchOperation batchOperation = new TableBatchOperation();
            int batchCount = 0;
            foreach (ITableEntity tableEntity in tableEntities)
            {
                batchOperation.Add(operationFactory(tableEntity));
                batchCount++;

                if (batchCount >= batchSize)
                {
                    IEnumerable<TableResult> results = await table.ExecuteBatchAsync(batchOperation);
                    batchOperation.Clear();
                    batchCount = 0;
                }
            }

            // Don't forget the stragglers
            if (batchCount > 0)
            {
                await table.ExecuteBatchAsync(batchOperation);
            }
        }

        public async Task<IQueryable<TElement>> CreateTableQuery<TElement>(string tableName) where TElement : ITableEntity, new()
        {
            CloudTable table = await GetCloudTable(tableName);
            return table.CreateQuery<TElement>();
        }

        /// <summary>
        /// Blob urls are case sensitive and convention is they should always be lowercase
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string FixCaseForBlobPath(string path)
        {
            return path.ToLower();
        }

        private string GetContainerName()
        {
            if (_containerName == null)
            {
                OrganizationContext _orgContext = _orgStateManager.GetContext();
                _containerName = _orgContext.AzureName.ToLower();

            }

            return _containerName;
        }

        private async Task<CloudBlobContainer> GetBlobContainer(bool isPrivate)
        {
            if (_container != null)
            {
                return _container;
            }

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_configuration.ConnectionString);
            CloudBlobClient client = storageAccount.CreateCloudBlobClient();

            OrganizationContext _orgContext = _orgStateManager.GetContext();
            string containerName = GetContainerName();
            if (isPrivate)
            {
                containerName += "-private";
            }

            _container = client.GetContainerReference(containerName);
            await _container.CreateIfNotExistsAsync();

            if (!isPrivate)
            {
                await _container.SetPermissionsAsync(new BlobContainerPermissions()
                {
                    PublicAccess = BlobContainerPublicAccessType.Blob
                });
            }

            return _container;
        }

        private async Task<CloudBlockBlob> GetBlockBlobAsync(string path, bool isPrivate)
        {
            CloudBlobContainer container = await GetBlobContainer(isPrivate);
            CloudBlockBlob blob = container.GetBlockBlobReference(FixCaseForBlobPath(path));
            return blob;
        }

        private async Task<CloudTable> GetCloudTable(string tableName)
        {
            if (_cloudTable != null)
            {
                return _cloudTable;
            }

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_configuration.ConnectionString);
            CloudTableClient client = storageAccount.CreateCloudTableClient();
            _cloudTable = client.GetTableReference(tableName);
            await _cloudTable.CreateIfNotExistsAsync();
            return _cloudTable;
        }
    }
}
