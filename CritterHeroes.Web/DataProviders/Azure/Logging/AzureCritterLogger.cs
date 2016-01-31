using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Contracts.Storage;
using Serilog;

namespace CritterHeroes.Web.DataProviders.Azure.Logging
{
    public class AzureCritterLogger : BaseAzureLogger, ICritterLogger
    {
        private StringWriter _writer;

        public AzureCritterLogger(IAzureService azureService)
            : base(azureService, LogCategory.Critter)
        {
            _writer = new StringWriter();
        }

        public string Messages
        {
            get
            {
                return _writer.ToString();
            }
        }

        public void LogAction(string message, params object[] messageValues)
        {
            Logger.Information(message, messageValues);
        }

        public void LogError(string message, params object[] messageValues)
        {
            Logger.Error(message, messageValues);
        }

        protected override LoggerConfiguration ConfigureLogger(LoggerConfiguration loggerConfiguration)
        {
            return base.ConfigureLogger(loggerConfiguration).WriteTo.TextWriter(_writer);
        }
    }
}
