using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Domain.Contracts;
using CritterHeroes.Web.Domain.Contracts.Email;
using CritterHeroes.Web.Domain.Contracts.StateManagement;
using CritterHeroes.Web.Domain.Contracts.Storage;
using CritterHeroes.Web.Shared.Commands;
using CritterHeroes.Web.Shared.StateManagement;
using TOTD.Mailer.Core;

namespace CritterHeroes.Web.Shared.Email
{
    public class ResetPasswordNotificationEmailBuilder : EmailBuilderBase<ResetPasswordNotificationEmailCommand>
    {
        public ResetPasswordNotificationEmailBuilder(IUrlGenerator urlGenerator, IStateManager<OrganizationContext> stateManager, IOrganizationLogoService logoService, IEmailConfiguration emailConfiguration)
            : base(urlGenerator, stateManager, logoService, emailConfiguration)
        {
        }

        protected override EmailBuilder BuildEmail(EmailBuilder builder, ResetPasswordNotificationEmailCommand command)
        {
            return builder
                .AddParagraph("This is a notification that your password has been successfuly reset.")
                .WithSubject($"Admin Notification - {command.OrganizationFullName}");
        }
    }
}
