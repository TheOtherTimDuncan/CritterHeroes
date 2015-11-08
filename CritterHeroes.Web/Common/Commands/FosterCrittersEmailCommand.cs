using System;
using System.Collections.Generic;
using System.Linq;

namespace CritterHeroes.Web.Common.Commands
{
    public class FosterCrittersEmailCommand : EmailCommand
    {
        public FosterCrittersEmailCommand(string emailTo)
            : base(EmailCommand.GetEmailNameFromCommandName(nameof(FosterCrittersEmailCommand)), emailTo)
        {
        }

        public string OrganizationFullName
        {
            get;
            set;
        }

        public string OrganizationShortName
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

        public IEnumerable<Object> Critters
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
                    OrganizationShortName = OrganizationShortName,
                    UrlHome = HomeUrl,
                    UrlLogo = LogoUrl,
                    Critters = Critters.ToArray()
                };
            }
        }
    }
}
