using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Models.Emails;
using Serilog.Events;

namespace CritterHeroes.Web.Models.LogEvents
{
    public class EmailLogEvent : AppLogEvent
    {
        public static EmailLogEvent LogEmail(Guid emailID, EmailModel email)
        {
            EmailContext context = new EmailContext(emailID);
            return new EmailLogEvent(context, "Sent email from {From} to {To}", email.From, email.To);
        }

        private EmailLogEvent(EmailContext context, string messageTemplate, params object[] messageValues)
            : base(context, LogEventCategory.Email, LogEventLevel.Information, messageTemplate, messageValues)
        {
        }

        public class EmailContext
        {
            public EmailContext(Guid emailID)
            {
                this.EmailID = emailID;
            }

            public Guid EmailID
            {
                get;
            }
        }
    }
}
