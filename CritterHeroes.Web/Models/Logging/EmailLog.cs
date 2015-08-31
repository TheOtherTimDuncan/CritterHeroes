using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Models.Logging
{
    public class EmailLog
    {
        public EmailLog(Guid logID, DateTimeOffset whenSentUtc, EmailMessage message)
        {
            this.ID = logID;
            this.WhenSentUtc = whenSentUtc;
            this.Message = message;
            this.EmailTo = string.Join(",", message.To);
        }

        public EmailLog(DateTimeOffset whenSentUtc, EmailMessage message)
            : this(Guid.NewGuid(), whenSentUtc, message)
        {
        }

        public Guid ID
        {
            get;
            private set;
        }

        public DateTimeOffset WhenSentUtc
        {
            get;
            private set;
        }

        public EmailMessage Message
        {
            get;
            private set;
        }

        public string EmailTo
        {
            get;
            set;
        }
    }
}
