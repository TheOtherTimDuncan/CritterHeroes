using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Email;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Shared.Commands;
using CritterHeroes.Web.Shared.StateManagement;
using TOTD.Mailer.Core;

namespace CritterHeroes.Web.Shared.Email
{
    public class ConfirmEmailEmailBuilder : EmailBuilderBase<ConfirmEmailEmailCommand>
    {
        public ConfirmEmailEmailBuilder(IUrlGenerator urlGenerator, IStateManager<OrganizationContext> stateManager, IOrganizationLogoService logoService, IEmailConfiguration emailConfiguration)
            : base(urlGenerator, stateManager, logoService, emailConfiguration)
        {
        }

        protected override EmailBuilder BuildEmail(EmailBuilder builder, ConfirmEmailEmailCommand command)
        {
            return builder
                .AddParagraph($"Please click the link below to confirm your email address. This link will be valid for {command.TokenLifespanDisplay}.")
                .AddButton(command.UrlConfirm, "Confirm Email")
                .WithSubject($"Email Confirmation - {command.OrganizationFullName}");
        }
    }
}
