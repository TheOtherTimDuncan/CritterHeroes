using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.Email;
using CritterHeroes.Web.Models;

namespace CritterHeroes.Web.Common.Email
{
    public class ResetPasswordEmailCommandHandler : BaseEmailCommandHandler<ResetPasswordEmailCommand>
    {
        private OrganizationContext _organizationContext;

        public ResetPasswordEmailCommandHandler(IEmailClient emailClient, OrganizationContext organizationContext)
            : base(emailClient)
        {
            this._organizationContext = organizationContext;
        }

        protected override EmailMessage CreateEmail(ResetPasswordEmailCommand emailCommand)
        {
            EmailMessage emailMessage = new EmailMessage()
            {
                Subject = "Password Reset - " + _organizationContext.FullName,
                From = _organizationContext.EmailAddress
            };

            EmailBuilder
                .Begin(emailMessage)
                .AddParagraph("Your password for your account at " + _organizationContext.FullName + " has been reset. To complete resetting your password, click the link below or visit " + _organizationContext.FullName + " and copy the code into the provided form. This code will be valid for " + emailCommand.TokenLifespanDisplay + ".")
                .AddParagraph("Confirmation Code: " + emailCommand.Token)
                .AddParagraph("<a href=\"" + emailCommand.Url + "\">Reset Password</a>")
                .End();

            return emailMessage;
        }
    }
}