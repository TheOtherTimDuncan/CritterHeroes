using System;
using System.Collections.Generic;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.Models.Logging
{
    public class EmailLog
    {
        public EmailLog(Guid logID, DateTimeOffset whenSentUtc, string emailTo)
            : this(logID, whenSentUtc)
        {
            this.WhenSentUtc = whenSentUtc;
            this.EmailTo = emailTo;
        }

        public EmailLog(DateTimeOffset whenSentUtc, EmailMessage message)
            : this(Guid.NewGuid(), whenSentUtc)
        {
            ThrowIf.Argument.IsNull(message, nameof(message));

            this.Message = message;
            this.EmailTo = string.Join(",", message.To);
        }

        private EmailLog(Guid logID, DateTimeOffset whenSentUtc)
        {
            this.ID = logID;
            this.WhenSentUtc = whenSentUtc;
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
            set;
        }

        public string EmailTo
        {
            get;
            set;
        }
    }
}
