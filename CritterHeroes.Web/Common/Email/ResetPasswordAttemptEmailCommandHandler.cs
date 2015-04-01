using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.Email;
using CritterHeroes.Web.Models;

namespace CritterHeroes.Web.Common.Email
{
    public class ResetPasswordAttemptEmailCommandHandler : BaseEmailCommandHandler<ResetPasswordAttemptEmailCommand>
    {
        private OrganizationContext _organizationContext;

        public ResetPasswordAttemptEmailCommandHandler(IEmailClient emailClient, OrganizationContext organizationContext)
            : base(emailClient)
        {
            this._organizationContext = organizationContext;
        }

        protected override EmailMessage CreateEmail(ResetPasswordAttemptEmailCommand emailCommand)
        {
            EmailMessage emailMessage = new EmailMessage()
            {
                Subject = "Password Reset Attempted - " + _organizationContext.FullName,
                From = _organizationContext.EmailAddress
            };

            EmailBuilder
                .Begin(emailMessage)
                .AddParagraph("You (or someone else) entered this email address when trying to change the password for an account at <a href=\"" +emailCommand.HomeUrl + "\">" + _organizationContext.FullName + "</a>.")
                .AddParagraph("However, this email address did not match an existing account and therefore the attempted password reset has failed.")
                .AddParagraph("If you have an account at <a href=\"" + emailCommand.HomeUrl + "\">" + _organizationContext.FullName + "</a> and were expecting this email, please try again using the email address you used when you created your account.")
                .AddParagraph("If you do not have an account at <a href=\"" + emailCommand.HomeUrl + "\">" + _organizationContext.FullName + "</a>, please ignore this email.")
                .End();

            return emailMessage;
        }
    }
}