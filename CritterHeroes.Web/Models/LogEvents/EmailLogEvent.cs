using System;
using System.Collections.Generic;
using System.Linq;
using Serilog.Events;
using TOTD.Mailer.Core;

namespace CritterHeroes.Web.Models.LogEvents
{
    public class EmailLogEvent : AppLogEvent
    {
        public static EmailLogEvent Create(Guid emailID, EmailMessage emailMessage)
        {
            EmailContext context = new EmailContext(emailID);
            return new EmailLogEvent(context, "Sent email from {From} to {To}", emailMessage.From, emailMessage.To);
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
