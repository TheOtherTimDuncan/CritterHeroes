using System;
using System.Collections.Generic;
using CritterHeroes.Web.Models.LogEvents;

namespace CritterHeroes.Web.Contracts.Logging
{
    public interface IAppLogger
    {
        IEnumerable<string> Messages
        {
            get;
        }

        void LogEvent<LogEventType>(LogEventType logEvent) where LogEventType : AppLogEvent;
    }
}
