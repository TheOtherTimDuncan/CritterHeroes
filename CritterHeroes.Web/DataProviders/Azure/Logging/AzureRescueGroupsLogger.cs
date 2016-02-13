using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Contracts.Storage;

namespace CritterHeroes.Web.DataProviders.Azure.Logging
{
    public class AzureRescueGroupsLogger : BaseAzureLogger, IRescueGroupsLogger
    {
        public AzureRescueGroupsLogger(IAzureService azureService)
            : base(azureService, LogCategory.RescueGroups)
        {
        }

        public void LogRequest(string url, string request, string response, HttpStatusCode statusCode)
        {
            Logger.Information("Sent {Request} to {Url} and received status code {StatusCode} with {Response}", request, url, statusCode, response);
        }
    }
}
