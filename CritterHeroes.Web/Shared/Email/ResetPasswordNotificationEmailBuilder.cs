using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Models.Emails;
using CritterHeroes.Web.Shared.Commands;
using TOTD.Mailer.Core;

namespace CritterHeroes.Web.Shared.Email
{
    public class ResetPasswordNotificationEmailBuilder : EmailBuilderBase<ResetPasswordNotificationEmailCommand, BaseEmailData>
    {
        protected override EmailBuilder BuildEmail(EmailBuilder builder, ResetPasswordNotificationEmailCommand command)
        {
            return builder
                .AddParagraph("This is a notification that your password has been successfuly reset.")
                .WithSubject($"Admin Notification - {command.EmailData.OrganizationFullName}");
        }
    }
}
