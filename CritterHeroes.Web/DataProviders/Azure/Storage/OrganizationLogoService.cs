using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Extensions;
using CritterHeroes.Web.Data.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using TOTD.Utility.StringHelpers;

namespace CritterHeroes.Web.DataProviders.Azure.Storage
{
    public class OrganizationLogoService : IOrganizationLogoService
    {
        private IAppConfiguration _appConfiguration;
        private IStateManager<OrganizationContext> _orgStateManager;
        private IAzureConfiguration _configuration;
        private ISqlStorageContext<Organization> _storageContext;

        private OrganizationContext _orgContext;
        private CloudBlobContainer _container;

        public OrganizationLogoService(IStateManager<OrganizationContext> orgStateManager, IAppConfiguration appConfiguration, IAzureConfiguration azureConfiguration, ISqlStorageContext<Organization> storageContext)
        {
            this._appConfiguration = appConfiguration;
            this._orgStateManager = orgStateManager;
            this._configuration = azureConfiguration;
            this._storageContext = storageContext;
        }

        private OrganizationContext OrganizationContext
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

        public string GetLogoUrl()
        {
            return GetBlobUrl(OrganizationContext.LogoFilename);
        }

        public async Task SaveLogo(Stream source, string filename, string contentType)
        {
            Organization org = await _storageContext.Entities.FindByIDAsync(OrganizationContext.OrganizationID);

            CloudBlobContainer container = await GetContainer();

            // Let's delete the original logo first if there is one
            if (!org.LogoFilename.IsNullOrEmpty())
            {
                CloudBlockBlob oldBlob = container.GetBlockBlobReference(org.LogoFilename);
                if (oldBlob != null)
                {
                    await oldBlob.DeleteIfExistsAsync();
                }
            }

            // Update the organization with the filename
            org.LogoFilename = filename;
            await _storageContext.SaveChangesAsync();

            // Upload the new logo
            CloudBlockBlob blob = container.GetBlockBlobReference(filename.ToLower());
            blob.Properties.ContentType = contentType;
            await blob.UploadFromStreamAsync(source);
        }

        private string GetBlobUrl(string filename)
        {
            // Blob urls are case sensitive and convention is they should always be lowercase
            return string.Format("{0}/{1}/{2}", _appConfiguration.BlobBaseUrl, OrganizationContext.AzureName.ToLower(), filename.ToLower());
        }

        private async Task<CloudBlobContainer> GetContainer()
        {
            if (_container != null)
            {
                return _container;
            }

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_configuration.ConnectionString);
            CloudBlobClient client = storageAccount.CreateCloudBlobClient();

            _container = client.GetContainerReference(OrganizationContext.AzureName.ToLower());
            await _container.CreateIfNotExistsAsync();
            await _container.SetPermissionsAsync(new BlobContainerPermissions()
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });

            return _container;
        }
    }
}