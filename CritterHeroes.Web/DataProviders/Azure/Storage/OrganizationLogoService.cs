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
    public class OrganizationLogoService : BaseAzureBlobStorage, IOrganizationLogoService
    {
        private ISqlStorageContext<Organization> _storageContext;

        public OrganizationLogoService(IStateManager<OrganizationContext> orgStateManager, IAppConfiguration appConfiguration, IAzureConfiguration azureConfiguration, ISqlStorageContext<Organization> storageContext)
            : base(orgStateManager, appConfiguration, azureConfiguration)
        {
            this._storageContext = storageContext;
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
                CloudBlockBlob oldBlob = container.GetBlockBlobReference(org.LogoFilename.ToLower());
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
            return GetBlobUrl(filename);
        }
    }
}
