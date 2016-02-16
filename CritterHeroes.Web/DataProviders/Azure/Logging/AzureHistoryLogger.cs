using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Contracts.Storage;
using Newtonsoft.Json;

namespace CritterHeroes.Web.DataProviders.Azure.Logging
{
    public class AzureHistoryLogger : BaseAzureLogger, IHistoryLogger
    {
        public AzureHistoryLogger(IAzureService azureService)
            : base(azureService, LogCategory.History)
        {
        }

        public void LogHistory(object entityID, string entityName, Dictionary<string, object> before, Dictionary<string, object> after)
        {
            string jsonBefore = JsonConvert.SerializeObject(before);
            string jsonAfter = JsonConvert.SerializeObject(after);

            Logger
                .ForContext("Before", jsonBefore)
                .ForContext("After", jsonAfter)
                .Information("Changed entity {ID} - {Name}", entityID, entityName);
        }
    }
}
