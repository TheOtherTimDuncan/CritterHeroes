using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Email;
using CritterHeroes.Web.Contracts.Events;
using CritterHeroes.Web.Models.LogEvents;
using CritterHeroes.Web.Shared.Commands;
using TOTD.Mailer.Core;

namespace CritterHeroes.Web.Shared.Email
{
    public class EmailService<TCommand> : TOTD.Mailer.Azure.EmailService, IEmailService<TCommand> where TCommand : EmailCommandBase
    {
        private IAppEventPublisher _publisher;
        private IEmailBuilder<TCommand> _emailBuilder;

        public EmailService(IAppEventPublisher publisher, IEmailBuilder<TCommand> emailBuilder)
        {
            this._publisher = publisher;
            this._emailBuilder = emailBuilder;
        }

        public async Task<CommandResult> SendEmailAsync(TCommand command)
        {
            EmailMessage emailMessage = _emailBuilder.BuildEmail(command);
            await SendEmailAsync(emailMessage);

            EmailLogEvent logEvent = EmailLogEvent.Create(emailMessage);
            _publisher.Publish(logEvent);

            return CommandResult.Success();
        }
    }
}
