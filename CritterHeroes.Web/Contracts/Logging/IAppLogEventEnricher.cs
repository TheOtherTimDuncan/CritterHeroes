using System;
using System.Collections.Generic;
using CritterHeroes.Web.Models.LogEvents;
using Serilog;

namespace CritterHeroes.Web.Contracts.Logging
{
    public interface IAppLogEventEnricher<LogEventType> where LogEventType : AppLogEvent
    {
        ILogger Enrich(ILogger logger, LogEventType logEvent);
    }
}
