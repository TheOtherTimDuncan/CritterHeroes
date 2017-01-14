using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Shared.Commands;
using TOTD.Mailer.Core;

namespace CritterHeroes.Web.Shared.Email
{
    public class ResetPasswordEmailBuilder : EmailBuilderBase<ResetPasswordEmailCommand,ResetPasswordEmailCommand.ResetPasswordEmailData>
    {
        protected override EmailBuilder BuildEmail(EmailBuilder builder, ResetPasswordEmailCommand command)
        {
            return builder
                .AddParagraph($"Your password for your account at {command.EmailData.OrganizationFullName} has been reset. Please click the link below to complete resetting your password. This link will be valid for {command.EmailData.TokenLifespanDisplay}.")
                .AddButton(command.EmailData.UrlReset, "Reset Password")
                .WithSubject($"Password Reset - {command.EmailData.OrganizationFullName}");
        }
    }
}
