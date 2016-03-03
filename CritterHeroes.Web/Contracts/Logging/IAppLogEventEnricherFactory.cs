using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Models.LogEvents;

namespace CritterHeroes.Web.Contracts.Logging
{
    public interface IAppLogEventEnricherFactory
    {
        IAppLogEventEnricher<LogEventType> GetEnricher<LogEventType>(LogEventType logEvent) where LogEventType : AppLogEvent;
    }
}
