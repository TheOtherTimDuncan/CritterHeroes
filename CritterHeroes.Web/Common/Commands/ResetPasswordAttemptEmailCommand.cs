using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Common.Commands
{
    public class ResetPasswordAttemptEmailCommand : EmailCommand
    {
        public ResetPasswordAttemptEmailCommand(string emailTo, string homeUrl, string logoUrl, string organizationFullName)
            : base(EmailCommand.GetEmailNameFromCommandName(nameof(ResetPasswordAttemptEmailCommand)), emailTo)
        {
            this.HomeUrl = homeUrl;
            this.LogoUrl = logoUrl;
            this.OrganizationFullName = organizationFullName;
        }

        public string OrganizationFullName
        {
            get;
            private set;
        }

        public string HomeUrl
        {
            get;
            private set;
        }

        public string LogoUrl
        {
            get;
            private set;
        }

        public override object EmailData
        {
            get
            {
                return new
                {
                    OrganizationFullName = OrganizationFullName,
                    UrlHome = HomeUrl,
                    UrlLogo = LogoUrl
                };
            }
        }
    }
}
