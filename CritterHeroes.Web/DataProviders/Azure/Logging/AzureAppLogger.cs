using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Common;
using CritterHeroes.Web.Contracts;
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
        private List<string> _messages;

        public AzureAppLogger(IAzureService azureService)
        {
            _messages = new List<string>();

            _logger = new LoggerConfiguration()
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

        public void LogEvent(AppLogEvent logEvent)
        {
            GetLogger(logEvent).Write(logEvent.Level, logEvent.MessageTemplate, logEvent.MessageValues);
        }

        public void LogEvent<ContextType>(AppLogEvent<ContextType> logEvent) where ContextType : class, new()
        {
            GetLogger(logEvent)
                .ForContext("Context", logEvent.Context, destructureObjects: true)
                .Write(logEvent.Level, logEvent.MessageTemplate, logEvent.MessageValues);
        }

        private ILogger GetLogger(AppLogEvent logEvent)
        {
            return _logger.ForContext("Category", logEvent.Category);
        }
    }
}
