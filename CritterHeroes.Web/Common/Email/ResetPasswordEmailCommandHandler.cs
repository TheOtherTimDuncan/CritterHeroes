using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.Email;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Models;

namespace CritterHeroes.Web.Common.Email
{
    public class ResetPasswordEmailCommandHandler : BaseEmailCommandHandler<ResetPasswordEmailCommand>
    {
        private IStateManager<OrganizationContext> _orgStateManager;

        public ResetPasswordEmailCommandHandler(IEmailClient emailClient, IStateManager<OrganizationContext> orgStateManager)
            : base(emailClient)
        {
            this._orgStateManager = orgStateManager;
        }

        protected override EmailMessage CreateEmail(ResetPasswordEmailCommand emailCommand)
        {
            OrganizationContext orgContext = _orgStateManager.GetContext();

            EmailMessage emailMessage = new EmailMessage()
            {
                Subject = "Password Reset - " + orgContext.FullName,
                From = orgContext.EmailAddress
            };

            EmailBuilder
                .Begin(emailMessage)
                .AddParagraph("Your password for your account at " + orgContext.FullName + " has been reset. To complete resetting your password, click the link below or visit " + orgContext.FullName + " and copy the code into the provided form. This code will be valid for " + emailCommand.TokenLifespanDisplay + ".")
                .AddParagraph("Confirmation Code: " + emailCommand.Token)
                .AddParagraph("<a href=\"" + emailCommand.Url + "\">Reset Password</a>")
                .End();

            return emailMessage;
        }
    }
}