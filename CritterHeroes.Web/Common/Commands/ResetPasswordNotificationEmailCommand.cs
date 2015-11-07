using System;
using System.Collections.Generic;
using System.Linq;

namespace CritterHeroes.Web.Common.Commands
{
    public class ResetPasswordNotificationEmailCommand : EmailCommand
    {
        public ResetPasswordNotificationEmailCommand(string emailTo, string homeUrl, string logoUrl, string organizationFullName)
            : base(EmailCommand.GetEmailNameFromCommandName(nameof(ResetPasswordNotificationEmailCommand)), emailTo)
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
