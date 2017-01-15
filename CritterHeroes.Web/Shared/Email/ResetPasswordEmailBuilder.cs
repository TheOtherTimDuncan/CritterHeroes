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
    public class ResetPasswordEmailBuilder : EmailBuilderBase<ResetPasswordEmailCommand>
    {
        public ResetPasswordEmailBuilder(IUrlGenerator urlGenerator, IStateManager<OrganizationContext> stateManager, IOrganizationLogoService logoService, IEmailConfiguration emailConfiguration)
            : base(urlGenerator, stateManager, logoService, emailConfiguration)
        {
        }

        protected override EmailBuilder BuildEmail(EmailBuilder builder, ResetPasswordEmailCommand command)
        {
            return builder
                .AddParagraph($"Your password for your account at {command.OrganizationFullName} has been reset. Please click the link below to complete resetting your password. This link will be valid for {command.TokenLifespanDisplay}.")
                .AddButton(command.UrlReset, "Reset Password")
                .WithSubject($"Password Reset - {command.OrganizationFullName}");
        }
    }
}
