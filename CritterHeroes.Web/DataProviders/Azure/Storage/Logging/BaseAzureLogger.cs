using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts.Storage;
using Serilog;

namespace CritterHeroes.Web.DataProviders.Azure.Storage.Logging
{
    public abstract class BaseAzureLogger
    {
        private readonly IAzureService _azureService;
        private readonly string _tableName;

        private ILogger _logger;

        protected BaseAzureLogger(IAzureService azureService)
            : this(azureService, "logs")
        {
        }

        protected BaseAzureLogger(IAzureService azureService, string tableName)
        {
            this._azureService = azureService;
            this._tableName = tableName;
        }

        protected ILogger Logger
        {
            get
            {
                if (_logger == null)
                {
                    _logger = CreateLogger();
                }
                return _logger;
            }
        }

        protected virtual LoggerConfiguration ConfigureLogger(LoggerConfiguration loggerConfiguration)
        {
            return loggerConfiguration.WriteTo.AzureTableStorage(_azureService, _tableName);
        }

        private ILogger CreateLogger()
        {
            LoggerConfiguration configuration = ConfigureLogger(new LoggerConfiguration());
            return configuration.MinimumLevel.Information().CreateLogger();
        }
    }
}
