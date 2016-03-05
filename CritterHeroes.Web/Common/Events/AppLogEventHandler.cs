using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts.Events;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Models.LogEvents;
using Serilog;
using CritterHeroes.Web.DataProviders.Azure.Logging;

namespace CritterHeroes.Web.Common.Events
{
    public class AppLogEventHandler<TAppLogEvent> : IAppEventHandler<TAppLogEvent> where TAppLogEvent : AppLogEvent
    {
        private ILogger _logger;
        private IAppLogEventEnricherFactory _enricherFactory;

        public AppLogEventHandler(ILogger logger, IAzureService azureService, IAppLogEventEnricherFactory enricherFactory)
        {
            this._enricherFactory = enricherFactory;

            this._logger = new LoggerConfiguration()
                .WriteTo.Logger(logger)
                .WriteTo.AzureTableStorage(azureService, "logs")
                .CreateLogger();
        }

        public void Handle(TAppLogEvent appEvent)
        {
            ILogger logger = _logger.ForContext("Category", appEvent.Category);

            IAppLogEventEnricher<TAppLogEvent> enricher = _enricherFactory.GetEnricher(appEvent);
            if (enricher != null)
            {
                logger = enricher.Enrich(logger, appEvent);
            }

            if (appEvent.Context != null)
            {
                logger = logger.ForContext("Context", appEvent.Context, destructureObjects: true);
            }

            logger.Write(appEvent.Level, appEvent.MessageTemplate, appEvent.MessageValues);
        }
    }
}
