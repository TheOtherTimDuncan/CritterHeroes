using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CritterHeroes.Web.Common.Commands
{
    public class ConfirmEmailEmailCommand : BaseTokenEmailCommand
    {
        public ConfirmEmailEmailCommand(string emailTo)
            : base(EmailCommand.GetEmailNameFromCommandName(nameof(ConfirmEmailEmailCommand)), emailTo)
        {
        }

        public string OrganizationFullName
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

        public string ConfirmUrl
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
                    UrlConfirm = ConfirmUrl,
                    TokenLifespan = TokenLifespanDisplay,
                    Token = Token
                };
            }
        }
    }
}
