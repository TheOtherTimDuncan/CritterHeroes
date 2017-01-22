using System;
using System.Collections.Generic;
using System.Linq;
using Serilog.Events;
using TOTD.Mailer.Core;

namespace CritterHeroes.Web.Models.LogEvents
{
    public class EmailLogEvent : AppLogEvent
    {
        public static EmailLogEvent Create(EmailMessage emailMessage)
        {
            return new EmailLogEvent("Sent email from {From} to {To}", emailMessage.From, emailMessage.To);
        }

        private EmailLogEvent(string messageTemplate, params object[] messageValues)
            : base(LogEventCategory.Email, LogEventLevel.Information, messageTemplate, messageValues)
        {
        }
    }
}
