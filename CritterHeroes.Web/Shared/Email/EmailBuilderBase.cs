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
        private const string _styles = @"
    .critters-list {
      width: 100%;
      border-collapse: collapse;
      margin-bottom: 15px;
    }
    .critters-list th {
      text-align: center;
    }
    .critters-list td,.critters-list th {
      padding: 5px 5px;
      text-align: left;
      border: darkgray 1px solid;
    }
";
        protected abstract EmailBuilder BuildEmail(EmailBuilder builder, TCommand command);

        public EmailMessage BuildEmail(TCommand command)
        {
            EmailMessage emailMessage = BuildEmail(EmailBuilder.Begin(), command)
                .AddStyles(_styles)
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
