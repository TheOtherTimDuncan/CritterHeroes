using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts.Email;
using CritterHeroes.Web.Models.Emails;
using CritterHeroes.Web.Shared.Commands;
using TOTD.Mailer.Core;

namespace CritterHeroes.Web.Shared.Email
{
    public abstract class EmailBuilderBase<TCommand> : IEmailBuilder<TCommand> where TCommand : EmailCommand<BaseEmailData>
    {
        protected abstract EmailBuilder BuildEmail(EmailBuilder builder, TCommand command);

        public EmailMessage BuildEmail(TCommand command)
        {
            EmailMessage emailMessage = BuildEmail(EmailBuilder.Begin(), command)
                .To(command.EmailTo)
                .From(command.EmailFrom)
                .ToEmail();
            return emailMessage;
        }
    }
}
