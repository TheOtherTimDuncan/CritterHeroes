using System;
using System.Collections.Generic;
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
        private CloudTable _cloudTable = null;
        private CloudBlobContainer _container;
        private IAzureConfiguration _configuration;
        private IStateManager<OrganizationContext> _orgStateManager;

        public AzureService(IAzureConfiguration azureConfiguration, IStateManager<OrganizationContext> orgStateManager)
        {
            ThrowIf.Argument.IsNull(azureConfiguration, nameof(azureConfiguration));
            ThrowIf.Argument.IsNull(orgStateManager, nameof(orgStateManager));

            this._configuration = azureConfiguration;
            this._orgStateManager = orgStateManager;
        }

        public async Task<CloudBlobContainer> GetBlobContainer()
        {
            if (_container != null)
            {
                return _container;
            }

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_configuration.ConnectionString);
            CloudBlobClient client = storageAccount.CreateCloudBlobClient();

            OrganizationContext _orgContext = _orgStateManager.GetContext();
            _container = client.GetContainerReference(_orgContext.AzureName.ToLower());
            await _container.CreateIfNotExistsAsync();

            return _container;
        }

        public async  Task<CloudTable> GetCloudTable(string tableName)
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

        public string GetLoggingKey()
        {
            return GetLoggingKeyForDate(DateTime.UtcNow);
        }

        public string GetLoggingKeyForDate(DateTime logDateUtc)
        {
            return new DateTime(logDateUtc.Year, logDateUtc.Month, logDateUtc.Day, logDateUtc.Hour, logDateUtc.Minute, 0, DateTimeKind.Utc).Ticks.ToString("d19");
        }
    }
}
