using System;
using System.Collections.Generic;
using System.Linq;
using Serilog.Events;

namespace CritterHeroes.Web.Models.LogEvents
{
    public class UserLogEvent : AppLogEvent
    {
        public static UserLogEvent LogAction(string message, params object[] messageValues)
        {
            return new UserLogEvent(LogEventLevel.Information, message, messageValues);
        }

        public static UserLogEvent LogError(string message, params object[] messageValues)
        {
            return new UserLogEvent(LogEventLevel.Error, message, messageValues);
        }

        private UserLogEvent(LogEventLevel level, string messageTemplate, params object[] messageValues)
            : base(LogEventCategory.User, level, messageTemplate, messageValues)
        {
        }
    }
}
