using System;
using System.Collections.Generic;

namespace CH.Domain.Models.Logging
{
    public class EmailLog
    {
        public EmailLog(Guid logID, DateTime whenSentUtc, EmailMessage message)
        {
            this.ID = logID;
            this.WhenSentUtc = whenSentUtc;
            this.Message = message;
        }

        public EmailLog(DateTime whenSentUtc, EmailMessage message)
            : this(Guid.NewGuid(), whenSentUtc, message)
        {
        }

        public Guid ID
        {
            get;
            private set;
        }

        public DateTime WhenSentUtc
        {
            get;
            private set;
        }

        public EmailMessage Message
        {
            get;
            private set;
        }

        public Guid? ForUserID
        {
            get;
            set;
        }
    }
}
