using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts.Email;
using CritterHeroes.Web.Models.Emails;
using CritterHeroes.Web.Shared.Commands;
using TOTD.Mailer.Core;

namespace CritterHeroes.Web.Shared.Email
{
    public abstract class EmailBuilderBase<TCommand, TEmailData> : IEmailBuilder<TCommand, TEmailData>
        where TCommand : EmailCommand<TEmailData>
        where TEmailData : BaseEmailData, new()
    {
        protected abstract EmailBuilder BuildEmail(EmailBuilder builder, TCommand command);

        public EmailMessage BuildEmail(TCommand command)
        {
            EmailMessage emailMessage = BuildEmail(EmailBuilder.Begin(), command)
                .BeginParagraph()
                    .AddText("Thanks,")
                    .AddLineBreak()
                    .AddText(command.EmailData.OrganizationFullName)
                    .AddLineBreak()
                    .AddLink(command.EmailData.UrlHome, command.EmailData.UrlHome)
                .EndParagraph()
                .AddImage(command.EmailData.UrlLogo, "Logo")
                .To(command.EmailTo)
                .From(command.EmailFrom)
                .ToEmail();

            return emailMessage;
        }
    }
}
