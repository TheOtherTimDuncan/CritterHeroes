using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Serilog.Events;

namespace CritterHeroes.Web.Models.LogEvents
{
    public class HistoryLogEvent : AppLogEvent
    {
        public static HistoryLogEvent LogHistory(object entityID, string entityName, Dictionary<string, object> before, Dictionary<string, object> after)
        {
            int breakPos = entityName.IndexOf('_');
            if (breakPos > 0)
            {
                // EF proxies follow type name with '_' and a random ID which we don't need
                entityName = entityName.Substring(0, breakPos);
            }

            HistoryContext context = new HistoryContext(JsonConvert.SerializeObject(before), JsonConvert.SerializeObject(after));

            return new HistoryLogEvent(context, "Changed entity {ID} - {Name}", entityID, entityName);
        }

        private HistoryLogEvent(HistoryContext context, string messageTemplate, params object[] messageValues)
            : base(context, LogEventCategory.History, LogEventLevel.Information, messageTemplate, messageValues)
        {
        }

        public class HistoryContext
        {
            public HistoryContext(string before, string after)
            {
                this.Before = before;
                this.After = after;
            }

            public string Before
            {
                get;
                private set;
            }

            public string After
            {
                get;
                private set;
            }
        }
    }
}
