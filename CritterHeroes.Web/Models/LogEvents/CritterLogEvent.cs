using System;
using System.Collections.Generic;
using System.Linq;
using Serilog.Events;

namespace CritterHeroes.Web.Models.LogEvents
{
    public class CritterLogEvent : LogEvent
    {
        public static CritterLogEvent LogAction(string message, params object[] messageValues)
        {
            return new CritterLogEvent(LogEventLevel.Information, message, messageValues);
        }

        public static CritterLogEvent LogError(string message, params object[] messageValues)
        {
            return new CritterLogEvent(LogEventLevel.Error, message, messageValues);
        }

        private CritterLogEvent(LogEventLevel level, string messageTemplate, params object[] messageValues)
            : base(LogEventCategory.Critter, level, messageTemplate, messageValues)
        {
        }
    }
}
