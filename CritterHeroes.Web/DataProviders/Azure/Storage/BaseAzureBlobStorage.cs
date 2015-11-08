using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Contracts.StateManagement;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace CritterHeroes.Web.DataProviders.Azure.Storage
{
    public abstract class BaseAzureBlobStorage
    {
        private IAzureConfiguration _configuration;
        private IStateManager<OrganizationContext> _orgStateManager;
        private IAppConfiguration _appConfiguration;
        private bool _isPrivate;

        private CloudBlobContainer _container;
        private OrganizationContext _orgContext;

        protected BaseAzureBlobStorage(IStateManager<OrganizationContext> orgStateManager, IAppConfiguration appConfiguration, IAzureConfiguration azureConfiguration, bool isPrivate = false)
        {
            this._appConfiguration = appConfiguration;
            this._orgStateManager = orgStateManager;
            this._configuration = azureConfiguration;
            this._isPrivate = isPrivate;
        }

        protected OrganizationContext OrganizationContext
        {
            get
            {
                if (_orgContext == null)
                {
                    _orgContext = _orgStateManager.GetContext();
                }
                return _orgContext;
            }
        }

        protected string CreateBlobUrl(string path)
        {
            // Blob urls are case sensitive and convention is they should always be lowercase
            return $"{_appConfiguration.BlobBaseUrl}/{OrganizationContext.AzureName}/{path}".ToLower();
        }

        protected async Task<CloudBlobContainer> GetContainer()
        {
            if (_container != null)
            {
                return _container;
            }

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_configuration.ConnectionString);
            CloudBlobClient client = storageAccount.CreateCloudBlobClient();

            string containerName = OrganizationContext.AzureName.ToLower();
            if (_isPrivate)
            {
                containerName += "-private";
            }

            _container = client.GetContainerReference(containerName);
            await _container.CreateIfNotExistsAsync();
            await _container.SetPermissionsAsync(new BlobContainerPermissions()
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });

            return _container;
        }
    }
}
