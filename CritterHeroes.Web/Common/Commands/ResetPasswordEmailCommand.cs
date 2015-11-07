using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Common.Commands
{
    public class ResetPasswordEmailCommand : BaseTokenEmailCommand
    {
        public ResetPasswordEmailCommand(string emailTo)
            : base(EmailCommand.GetEmailNameFromCommandName(nameof(ResetPasswordEmailCommand)), emailTo)
        {
        }

        public string OrganizationFullName
        {
            get;
            set;
        }

        public string ResetUrl
        {
            get;
            set;
        }

        public string HomeUrl
        {
            get;
            set;
        }

        public string LogoUrl
        {
            get;
            set;
        }

        public override object EmailData
        {
            get
            {
                return new
                {
                    OrganizationFullName = OrganizationFullName,
                    UrlHome = HomeUrl,
                    UrlLogo = LogoUrl,
                    UrlReset = ResetUrl,
                    TokenLifespan = TokenLifespanDisplay,
                    Token = Token
                };
            }
        }
    }
}
