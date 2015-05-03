using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.Email;
using CritterHeroes.Web.Models;

namespace CritterHeroes.Web.Common.Email
{
    public class ConfirmEmailCommandHandler : BaseEmailCommandHandler<ConfirmEmailCommand>
    {
        private OrganizationContext _organizationContext;

        public ConfirmEmailCommandHandler(IEmailClient emailClient, OrganizationContext organizationContext)
            : base(emailClient)
        {
            this._organizationContext = organizationContext;
        }

        protected override EmailMessage CreateEmail(ConfirmEmailCommand emailCommand)
        {
            EmailMessage emailMessage = new EmailMessage()
            {
                Subject = "Email Confirmation - " + _organizationContext.FullName,
                From = _organizationContext.EmailAddress
            };

            EmailBuilder
                .Begin(emailMessage)
                .AddParagraph("Please confirm your email address by clicking the link below or visiting <a href=\"" + emailCommand.HomeUrl + "\">" + _organizationContext.FullName + "</a> and copying the code into the provided form. The code will be valid for " + emailCommand.TokenLifespanDisplay + ".")
                .AddParagraph("Confirmation code: " + emailCommand.Token)
                .AddParagraph("<a href=\"" + emailCommand.Url + "\">Confirm Email</a>")
                .End();

            return emailMessage;
        }
    }
}