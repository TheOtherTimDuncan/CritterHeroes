using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.DataProviders.Azure.Utility;
using CritterHeroes.Web.Models.Logging;
using Microsoft.Owin;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using Serilog;

namespace CritterHeroes.Web.DataProviders.Azure.Storage.Logging
{
    public class AzureUserLogger : IUserLogger
    {
        private IOwinContext _owinContext;
        private IAzureService _azureService;

        private ILogger _logger;

        public AzureUserLogger(IAzureService azureService, IOwinContext owinContext)
        {
            this._owinContext = owinContext;
            this._azureService = azureService;

            _logger = Log.Logger = new LoggerConfiguration()
                .WriteTo
                .AzureTableStorage(azureService, "userlog")
                .Enrich.WithProperty("IPAddress", _owinContext.Request.RemoteIpAddress)
                .MinimumLevel.Information()
                .CreateLogger();
        }

        public void LogAction(string message, params object[] messageValues)
        {
            _logger.Information(message, messageValues);
        }

        public void LogError(string message, params object[] messageValues)
        {
            _logger.Error(message, messageValues);
        }

        public void LogError(string message, IEnumerable<string> errors, params object[] messageValues)
        {
            _logger
                .ForContext("Errors", errors)
                .Error(message, messageValues);
        }
    }
}
