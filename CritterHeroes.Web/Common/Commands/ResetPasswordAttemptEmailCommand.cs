using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Common.Commands
{
    public class ResetPasswordAttemptEmailCommand : EmailCommand
    {
        public ResetPasswordAttemptEmailCommand(string emailTo, string homeUrl, string logoUrl, string organizationFullName)
            : base(EmailCommand.GetEmailNameFromCommandName(nameof(ResetPasswordAttemptEmailCommand)), emailTo)
        {
            this.EmailData = new
            {
                OrganizationFullName = organizationFullName,
                UrlHome = homeUrl,
                UrlLogo = logoUrl
            };
        }
    }
}
