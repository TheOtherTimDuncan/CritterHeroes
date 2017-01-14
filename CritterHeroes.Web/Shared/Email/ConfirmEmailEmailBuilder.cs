using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Shared.Commands;
using TOTD.Mailer.Core;

namespace CritterHeroes.Web.Shared.Email
{
    public class ConfirmEmailEmailBuilder : EmailBuilderBase<ConfirmEmailEmailCommand, ConfirmEmailEmailCommand.ConfirmEmailData>
    {
        protected override EmailBuilder BuildEmail(EmailBuilder builder, ConfirmEmailEmailCommand command)
        {
            return builder
                .AddParagraph($"Please click the link below to confirm your email address. This link will be valid for {command.EmailData.TokenLifespanDisplay}.")
                .AddButton(command.EmailData.UrlConfirm, "Confirm Email")
                .WithSubject($"Email Confirmation - {command.EmailData.OrganizationFullName}");
        }
    }
}
