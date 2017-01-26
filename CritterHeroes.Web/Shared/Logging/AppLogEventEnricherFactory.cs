using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Domain.Contracts.Logging;
using CritterHeroes.Web.Models.LogEvents;

namespace CritterHeroes.Web.Shared.Logging
{
    public class AppLogEventEnricherFactory : IAppLogEventEnricherFactory
    {
        private IServiceProvider _serviceProvider;

        public AppLogEventEnricherFactory(IServiceProvider serviceProvider)
        {
            this._serviceProvider = serviceProvider;
        }

        public IAppLogEventEnricher<LogEventType> GetEnricher<LogEventType>(LogEventType logEvent) where LogEventType : AppLogEvent
        {
            return (IAppLogEventEnricher<LogEventType>)_serviceProvider.GetService(typeof(IAppLogEventEnricher<LogEventType>));
        }
    }
}
