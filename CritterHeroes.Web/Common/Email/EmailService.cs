using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Common;
using CritterHeroes.Web.Areas.Common.ActionExtensions;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Contracts.Email;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Models.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace CritterHeroes.Web.Common.Email
{
    public class EmailService : IEmailService
    {
        private IFileSystem _fileSystem;
        private IEmailConfiguration _emailConfiguration;
        private IUrlGenerator _urlGenerator;
        private IAzureConfiguration _azureConfiguration;
        private IOrganizationLogoService _logoService;
        private IStateManager<OrganizationContext> _stateManager;
        private IEmailLogger _emailLogger;

        private CloudQueue _cloudQueue = null;

        public EmailService(IFileSystem fileSystem, IEmailConfiguration emailConfiguration, IAzureConfiguration azureConfiguration, IUrlGenerator urlGenerator, IStateManager<OrganizationContext> stateManager, IOrganizationLogoService logoService, IEmailLogger emailLogger)
        {
            this._fileSystem = fileSystem;
            this._emailConfiguration = emailConfiguration;
            this._urlGenerator = urlGenerator;
            this._azureConfiguration = azureConfiguration;
            this._stateManager = stateManager;
            this._logoService = logoService;
            this._emailLogger = emailLogger;
        }

        public async Task<CommandResult> SendEmailAsync<EmailDataType>(EmailCommand<EmailDataType> command) where EmailDataType : BaseEmailData, new()
        {
            string folder = _fileSystem.MapServerPath($"dist/emails/{command.EmailName}");

            string filenameSubject = _fileSystem.CombinePath(folder, "Subject.txt");
            string filenameHtmlBody = _fileSystem.CombinePath(folder, "Body.html");
            string filenameTxtBody = _fileSystem.CombinePath(folder, "Body.txt");

            OrganizationContext organizationContext = _stateManager.GetContext();
            command.EmailData.OrganizationFullName = organizationContext.FullName;
            command.EmailData.OrganizationShortName = organizationContext.ShortName;

            command.EmailData.UrlLogo = _logoService.GetLogoUrl();
            command.EmailData.UrlHome = _urlGenerator.GenerateAbsoluteHomeUrl();

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

            await _emailLogger.LogEmailAsync (new EmailLog(email));

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
