using System;
using System.Collections.Generic;
using CritterHeroes.Web.Models.Emails;

namespace CritterHeroes.Web.Shared.Commands
{
    public class EmailCommand<TEmailDataType> where TEmailDataType : BaseEmailData, new()
    {
        private List<string> _emailTos;

        public EmailCommand(string emailName, string emailTo)
        {
            string suffix = nameof(EmailCommand<TEmailDataType>);
            if (emailName.EndsWith(suffix))
            {
                this.EmailName = emailName.Substring(0, emailName.Length - suffix.Length);
            }
            else
            {
                this.EmailName = emailName;
            }

            _emailTos = new List<string>();
            _emailTos.Add(emailTo);

            this.EmailData = new TEmailDataType();
        }

        public string EmailName
        {
            get;
            private set;
        }

        public IEnumerable<string> EmailTo => _emailTos;

        public string EmailFrom
        {
            get;
            set;
        }

        public TEmailDataType EmailData
        {
            get;
        }

        public void AddTo(string email)
        {
            _emailTos.Add(email);
        }
    }
}
