using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Common.Commands
{
    public class EmailCommand
    {
        public EmailCommand(string emailName, string emailTo)
        {
            this.EmailName = emailName;
            this.EmailTo = emailTo;
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

        public object EmailData
        {
            get;
            set;
        }

        protected static string GetEmailNameFromCommandName(string commandName)
        {
            return commandName.Substring(0, commandName.Length - nameof(EmailCommand).Length);
        }
    }
}
