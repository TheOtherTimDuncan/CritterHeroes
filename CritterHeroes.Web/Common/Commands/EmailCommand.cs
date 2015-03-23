using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Common.Commands
{
    public class EmailCommand
    {
        public EmailCommand(string emailTo)
        {
            this.EmailTo = emailTo;
        }

        public string EmailTo
        {
            get;
            private set;
        }
    }
}