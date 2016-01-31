using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Contracts.Storage;
using Microsoft.Owin;
using Serilog;

namespace CritterHeroes.Web.DataProviders.Azure.Storage.Logging
{
    public class AzureUserLogger : BaseAzureLogger, IUserLogger
    {
        private IOwinContext _owinContext;

        public AzureUserLogger(IAzureService azureService, IOwinContext owinContext)
            : base(azureService, LogCategory.User)
        {
            this._owinContext = owinContext;
        }

        public void LogAction(string message, params object[] messageValues)
        {
            Logger.Information(message, messageValues);
        }

        public void LogError(string message, params object[] messageValues)
        {
            Logger.Error(message, messageValues);
        }

        public void LogError(string message, IEnumerable<string> errors, params object[] messageValues)
        {
            Logger
                .ForContext("Errors", errors)
                .Error(message, messageValues);
        }

        protected override LoggerConfiguration ConfigureLogger(LoggerConfiguration loggerConfiguration)
        {
            return base.ConfigureLogger(loggerConfiguration).Enrich.WithProperty("IPAddress", _owinContext.Request.RemoteIpAddress);
        }
    }
}
