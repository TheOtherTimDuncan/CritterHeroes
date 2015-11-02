using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Common;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Contracts.Email;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CritterHeroes.Web.Common.Email
{
    public class EmailService : IEmailService
    {
        private IFileSystem _fileSystem;
        private IHttpContext _httpContext;
        private IEmailConfiguration _emailConfiguration;
        private IAzureConfiguration _azureConfiguration;

        private CloudQueue _cloudQueue = null;

        public EmailService(IFileSystem fileSystem, IHttpContext httpContext, IEmailConfiguration emailConfiguration, IAzureConfiguration azureConfiguration)
        {
            this._fileSystem = fileSystem;
            this._httpContext = httpContext;
            this._emailConfiguration = emailConfiguration;
            this._azureConfiguration = azureConfiguration;
        }

        public async Task<CommandResult> SendEmailAsync<TParameter>(TParameter command) where TParameter : EmailCommand
        {
            string folder = _httpContext.Server.MapPath($"~/Areas/Emails/{command.EmailName}");

            string filenameSubject = _fileSystem.CombinePath(folder, "Subject.txt");
            string filenameHtmlBody = _fileSystem.CombinePath(folder, "Body.html");
            string filenameTxtBody = _fileSystem.CombinePath(folder, "Body.txt");

            var email = new
            {
                From = command.EmailFrom ?? _emailConfiguration.DefaultFrom,
                To = command.EmailTo,
                SubjectTemplate = _fileSystem.ReadAllText(filenameSubject),
                HtmlTemplate = _fileSystem.ReadAllText(filenameHtmlBody),
                TextTemplate = _fileSystem.ReadAllText(filenameTxtBody),
                Data = command.EmailData

            };

            string json = JavascriptConvert.SerializeObject(email).ToString();
            CloudQueue queue = await GetCloudQueue();
            CloudQueueMessage queueMessage = new CloudQueueMessage(json);
            await queue.AddMessageAsync(queueMessage);

            return CommandResult.Success();
        }

        protected async Task<CloudQueue> GetCloudQueue()
        {
            if (_cloudQueue != null)
            {
                return _cloudQueue;
            }

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_azureConfiguration.ConnectionString);
            CloudQueueClient client = storageAccount.CreateCloudQueueClient();
            _cloudQueue = client.GetQueueReference("email");
            await _cloudQueue.CreateIfNotExistsAsync();
            return _cloudQueue;
        }
    }
}
