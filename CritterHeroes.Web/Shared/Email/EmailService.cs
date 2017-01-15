using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Email;
using CritterHeroes.Web.Contracts.Events;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Features.Shared;
using CritterHeroes.Web.Models.LogEvents;
using CritterHeroes.Web.Shared.Commands;
using TOTD.Mailer.Core;

namespace CritterHeroes.Web.Shared.Email
{
    public class EmailService<TCommand> : IEmailService<TCommand> where TCommand : EmailCommandBase
    {
        private IEmailConfiguration _emailConfiguration;
        private IAzureService _azureService;
        private IAppEventPublisher _publisher;
        private IEmailBuilder<TCommand> _emailBuilder;

        private const string _blobPath = "email";
        private const string _queueName = "email";
        private const bool _isPrivate = true;

        public EmailService(IEmailConfiguration emailConfiguration, IAzureService azureService, IAppEventPublisher publisher, IEmailBuilder<TCommand> emailBuilder)
        {
            this._emailConfiguration = emailConfiguration;
            this._azureService = azureService;
            this._publisher = publisher;
            this._emailBuilder = emailBuilder;
        }

        public async Task<CommandResult> SendEmailAsync(TCommand command)
        {
            EmailMessage emailMessage = _emailBuilder.BuildEmail(command);

            string json = JavascriptConvert.SerializeObject(emailMessage).ToString();
            await _azureService.AddQueueMessageAsync(_queueName, json);

            Guid blobID = Guid.NewGuid();
            await _azureService.UploadBlobAsync($"{_blobPath}/{blobID}", _isPrivate, json);

            EmailLogEvent logEvent = EmailLogEvent.Create(blobID, emailMessage);
            _publisher.Publish(logEvent);

            return CommandResult.Success();
        }
    }
}
