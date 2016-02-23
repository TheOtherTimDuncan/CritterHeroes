﻿using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts.Storage;
using Serilog;

namespace CritterHeroes.Web.DataProviders.Azure.Logging
{
    public abstract class BaseAzureLogger
    {
        private readonly IAzureService _azureService;
        private readonly string _tableName;
        private readonly string _category;

        private ILogger _logger;

        protected BaseAzureLogger(IAzureService azureService, string category)
            : this(azureService, category, "logs")
        {
        }

        protected BaseAzureLogger(IAzureService azureService, string category, string tableName)
        {
            this._azureService = azureService;
            this._category = category;
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
            return loggerConfiguration
                .Enrich.WithProperty("Category", _category)
                .WriteTo.AzureTableStorage(_azureService, _tableName);
        }

        private ILogger CreateLogger()
        {
            LoggerConfiguration configuration = ConfigureLogger(new LoggerConfiguration());
            return configuration.MinimumLevel.Information().CreateLogger();
        }
    }
}