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
    public class ConfirmEmailCommandHandler : BaseEmailCommandHandler<ConfirmEmailCommand>
    {
        private IStateManager<OrganizationContext> _orgStateManager;

        public ConfirmEmailCommandHandler(IEmailClient emailClient, IStateManager<OrganizationContext> orgStateManager)
            : base(emailClient)
        {
            this._orgStateManager = orgStateManager;
        }

        protected override EmailMessage CreateEmail(ConfirmEmailCommand emailCommand)
        {
            OrganizationContext orgContext = _orgStateManager.GetContext();

            EmailMessage emailMessage = new EmailMessage()
            {
                Subject = "Email Confirmation - " + orgContext.FullName,
                From = orgContext.EmailAddress
            };

            EmailBuilder
                .Begin(emailMessage)
                .AddParagraph("Please confirm your email address by clicking the link below or visiting <a href=\"" + emailCommand.HomeUrl + "\">" + orgContext.FullName + "</a> and copying the code into the provided form. The code will be valid for " + emailCommand.TokenLifespanDisplay + ".")
                .AddParagraph("Confirmation code: " + emailCommand.Token)
                .AddParagraph("<a href=\"" + emailCommand.Url + "\">Confirm Email</a>")
                .End();

            return emailMessage;
        }
    }
}