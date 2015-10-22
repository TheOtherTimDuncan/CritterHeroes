using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Models;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;

namespace CritterHeroes.Web.DataProviders.Azure.Storage
{
    public class EmailStorageService : BaseAzureBlobStorage, IEmailStorageService
    {
        private const string _path = "email";

        public EmailStorageService(IStateManager<OrganizationContext> orgStateManager, IAppConfiguration appConfiguration, IAzureConfiguration azureConfiguration)
            : base(orgStateManager, appConfiguration, azureConfiguration)
        {
        }

        public async Task<EmailMessage> GetEmailAsync(Guid emailID)
        {
            CloudBlobContainer container = await GetContainer();
            CloudBlockBlob blob = container.GetBlockBlobReference($"{_path}/{emailID}");
            string data = await blob.DownloadTextAsync();
            EmailMessage message = JsonConvert.DeserializeObject<EmailMessage>(data);
            return message;
        }

        public async Task SaveEmail(EmailMessage message, Guid emailID)
        {
            CloudBlobContainer container = await GetContainer();
            CloudBlockBlob blob = container.GetBlockBlobReference($"{_path}/{emailID}");
            string data = JsonConvert.SerializeObject(message);
            await blob.UploadTextAsync(data);
        }
    }
}
