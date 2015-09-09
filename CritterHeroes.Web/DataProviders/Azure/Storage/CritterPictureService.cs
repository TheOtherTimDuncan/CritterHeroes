using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Models;
using Microsoft.WindowsAzure.Storage.Blob;

namespace CritterHeroes.Web.DataProviders.Azure.Storage
{
    public class CritterPictureService : BaseAzureBlobStorage, ICritterPictureService
    {
        private const string _blobName = "critters";

        public CritterPictureService(IStateManager<OrganizationContext> orgStateManager, IAppConfiguration appConfiguration, IAzureConfiguration azureConfiguration)
            : base(orgStateManager, appConfiguration, azureConfiguration)
        {
        }

        public async Task SavePictureAsync(Stream source, int critterID, string filename, string contentType)
        {
            CloudBlobContainer container = await GetContainer();
            CloudBlockBlob blob = container.GetBlockBlobReference(GetBlobPath(critterID, filename));
            blob.Properties.ContentType = contentType;
            await blob.UploadFromStreamAsync(source);
        }

        private string GetBlobPath(int critterID, string filename)
        {
            return $"{_blobName}/{critterID}/{filename.ToLower()}";
        }
    }
}
