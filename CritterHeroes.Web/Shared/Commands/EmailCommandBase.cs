using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Shared.Commands
{
    public abstract class EmailCommandBase
    {
        private List<string> _emailTos;

        public EmailCommandBase(string emailTo)
        {
            _emailTos = new List<string>();
            _emailTos.Add(emailTo);
        }

        public IEnumerable<string> EmailTo => _emailTos;

        public string EmailFrom
        {
            get;
            set;
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

        public string UrlHome
        {
            get;
            set;
        }

        public string UrlLogo
        {
            get;
            set;
        }

        public void AddTo(string email)
        {
            _emailTos.Add(email);
        }
    }
}
