using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Common;
using CritterHeroes.Web.Areas.Common.ActionExtensions;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Email;
using CritterHeroes.Web.Contracts.Events;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Models.Emails;
using CritterHeroes.Web.Models.LogEvents;

namespace CritterHeroes.Web.Common.Email
{
    public class EmailService : IEmailService
    {
        private IFileSystem _fileSystem;
        private IEmailConfiguration _emailConfiguration;
        private IUrlGenerator _urlGenerator;
        private IAzureService _azureService;
        private IOrganizationLogoService _logoService;
        private IStateManager<OrganizationContext> _stateManager;
        private IAppEventPublisher _publisher;

        private const string _blobPath = "email";
        private const string _queueName = "email";
        private const bool _isPrivate = true;

        public EmailService(IFileSystem fileSystem, IEmailConfiguration emailConfiguration, IAzureService azureService, IUrlGenerator urlGenerator, IStateManager<OrganizationContext> stateManager, IOrganizationLogoService logoService, IAppEventPublisher publisher)
        {
            this._fileSystem = fileSystem;
            this._emailConfiguration = emailConfiguration;
            this._urlGenerator = urlGenerator;
            this._azureService = azureService;
            this._stateManager = stateManager;
            this._logoService = logoService;
            this._publisher = publisher;
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

            EmailModel email = new EmailModel()
            {
                From = command.EmailFrom ?? _emailConfiguration.DefaultFrom,
                To = command.EmailTo,
                SubjectTemplate = _fileSystem.ReadAllText(filenameSubject),
                HtmlTemplate = _fileSystem.ReadAllText(filenameHtmlBody),
                TextTemplate = _fileSystem.ReadAllText(filenameTxtBody),
                EmailData = command.EmailData
            };

            string json = JavascriptConvert.SerializeObject(email).ToString();
            await _azureService.AddQueueMessageAsync(_queueName, json);

            Guid blobID = Guid.NewGuid();
            await _azureService.UploadBlobAsync($"{_blobPath}/{blobID}", _isPrivate, json);

            EmailLogEvent logEvent = EmailLogEvent.Create(blobID, email);
            _publisher.Publish(logEvent);

            return CommandResult.Success();
        }
    }
}
