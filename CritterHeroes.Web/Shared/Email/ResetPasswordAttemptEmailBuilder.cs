using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Shared.Commands;
using TOTD.Mailer.Core;

namespace CritterHeroes.Web.Shared.Email
{
    public class ResetPasswordAttemptEmailBuilder : EmailBuilderBase<ResetPasswordAttemptEmailCommand>
    {
        protected override EmailBuilder BuildEmail(EmailBuilder builder, ResetPasswordAttemptEmailCommand command)
        {
            return builder
                .AddParagraph($"You (or someone else) entered this email address when trying to change the password for an account at {command.EmailData.OrganizationFullName}.")
                .AddParagraph("However, this email address did not match an existing account and therefore the attempted password reset has failed.")
                .AddParagraph($"If you have an account at {command.EmailData.OrganizationFullName} and were expecting this email, please try again using the email address you used when you created your account.")
                .AddParagraph($"If you do not have an account at {command.EmailData.OrganizationFullName}, please ignore this email.")
                .BeginParagraph()
                    .AddText("Thanks,")
                    .AddLineBreak()
                    .AddText(command.EmailData.OrganizationFullName)
                    .AddLineBreak()
                    .AddLink(command.EmailData.UrlHome, command.EmailData.UrlHome)
                .EndParagraph()
                .AddImage(command.EmailData.UrlLogo, "Logo")
                .WithSubject($"Password Reset Attempted - {command.EmailData.OrganizationFullName}");
        }
    }
}
