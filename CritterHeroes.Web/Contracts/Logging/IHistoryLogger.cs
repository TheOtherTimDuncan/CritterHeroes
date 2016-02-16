using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Contracts.Logging
{
    public interface IHistoryLogger
    {
        void LogHistory(object entityID, string entityName, Dictionary<string, object> before, Dictionary<string, object> after);
    }
}
