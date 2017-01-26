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
    public class ResetPasswordAttemptEmailBuilder : EmailBuilderBase<ResetPasswordAttemptEmailCommand>
    {
        public ResetPasswordAttemptEmailBuilder(IUrlGenerator urlGenerator, IStateManager<OrganizationContext> stateManager, IOrganizationLogoService logoService, IEmailConfiguration emailConfiguration)
            : base(urlGenerator, stateManager, logoService, emailConfiguration)
        {
        }

        protected override EmailBuilder BuildEmail(EmailBuilder builder, ResetPasswordAttemptEmailCommand command)
        {
            return builder
                .AddParagraph($"You (or someone else) entered this email address when trying to change the password for an account at {command.OrganizationFullName}.")
                .AddParagraph("However, this email address did not match an existing account and therefore the attempted password reset has failed.")
                .AddParagraph($"If you have an account at {command.OrganizationFullName} and were expecting this email, please try again using the email address you used when you created your account.")
                .AddParagraph($"If you do not have an account at {command.OrganizationFullName}, please ignore this email.")
                .WithSubject($"Password Reset Attempted - {command.OrganizationFullName}");
        }
    }
}
