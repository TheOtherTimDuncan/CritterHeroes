using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Contracts.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace CritterHeroes.Web.DataProviders.Azure.Storage
{
    public class EmailStorageService : BaseAzureBlobStorage, IEmailStorageService
    {
        private const string _path = "email";

        public EmailStorageService(IStateManager<OrganizationContext> orgStateManager, IAppConfiguration appConfiguration, IAzureConfiguration azureConfiguration)
            : base(orgStateManager, appConfiguration, azureConfiguration, isPrivate: true)
        {
        }

        public async Task<string> GetEmailAsync(Guid emailID)
        {
            CloudBlobContainer container = await GetContainer();
            CloudBlockBlob blob = container.GetBlockBlobReference($"{_path}/{emailID}");
            string data = await blob.DownloadTextAsync();
            return data;
        }

        public async Task SaveEmailAsync(Guid emailID, string emailData)
        {
            CloudBlobContainer container = await GetContainer();
            CloudBlockBlob blob = container.GetBlockBlobReference($"{_path}/{emailID}");
            await blob.UploadTextAsync(emailData);
        }
    }
}
