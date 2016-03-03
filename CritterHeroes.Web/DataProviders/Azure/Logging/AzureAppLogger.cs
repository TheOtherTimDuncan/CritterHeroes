using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Common;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Models.LogEvents;
using Serilog;

namespace CritterHeroes.Web.DataProviders.Azure.Logging
{
    public class LogCategory
    {
        public const string User = "User";
        public const string Email = "Email";
        public const string Critter = "Critter";
        public const string RescueGroups = "RescueGroups";
        public const string History = "History";
    }

    public class AzureAppLogger : IAppLogger
    {
        private ILogger _logger;
        private IAppLogEventEnricherFactory _enricherFactory;

        private List<string> _messages;

        public AzureAppLogger(IAzureService azureService, IAppLogEventEnricherFactory enricherFactory)
        {
            this._messages = new List<string>();

            this._enricherFactory = enricherFactory;

            this._logger = new LoggerConfiguration()
                .WriteTo.AzureTableStorage(azureService, "logs")
                .WriteTo.List(_messages)
                .MinimumLevel.Information()
                .CreateLogger();
        }

        public IEnumerable<string> Messages
        {
            get
            {
                return _messages;
            }
        }

        public void LogEvent<LogEventType>(LogEventType logEvent) where LogEventType : AppLogEvent
        {
            GetLogger(logEvent).Write(logEvent.Level, logEvent.MessageTemplate, logEvent.MessageValues);
        }

        private ILogger GetLogger<LogEventType>(LogEventType logEvent) where LogEventType : AppLogEvent
        {
            ILogger logger = _logger.ForContext("Category", logEvent.Category);

            IAppLogEventEnricher<LogEventType> enricher = _enricherFactory.GetEnricher(logEvent);
            if (enricher != null)
            {
                logger = enricher.Enrich(logger, logEvent);
            }

            if (logEvent.Context != null)
            {
                logger = logger.ForContext("Context", logEvent.Context, destructureObjects: true);
            }

            return logger;
        }
    }
}
