using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Common.Commands
{
    public class EmailCommand<TEmailDataType> where TEmailDataType : BaseEmailData, new()
    {
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

            this.EmailTo = emailTo;
            this.EmailData = new TEmailDataType();
        }

        public string EmailName
        {
            get;
            private set;
        }

        public string EmailTo
        {
            get;
            private set;
        }

        public string EmailFrom
        {
            get;
            set;
        }

        public TEmailDataType EmailData
        {
            get;
            protected set;
        }
    }
}
