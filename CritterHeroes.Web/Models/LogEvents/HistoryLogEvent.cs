using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Serilog.Events;

namespace CritterHeroes.Web.Models.LogEvents
{
    public class HistoryLogEvent : LogEvent<HistoryLogEvent.HistoryContext>
    {
        public static HistoryLogEvent LogHistory(object entityID, string entityName, Dictionary<string, object> before, Dictionary<string, object> after)
        {
            HistoryContext context = new HistoryContext()
            {
                Before = JsonConvert.SerializeObject(before),
                After = JsonConvert.SerializeObject(after)
            };

            return new HistoryLogEvent(context, "Changed entity {ID} - {Name}", entityID, entityName);
        }

        private HistoryLogEvent(HistoryContext context, string messageTemplate, params object[] messageValues)
            : base(context, LogEventCategory.History, LogEventLevel.Information, messageTemplate, messageValues)
        {
        }

        public class HistoryContext
        {
            public string Before
            {
                get;
                set;
            }

            public string After
            {
                get;
                set;
            }
        }
    }
}
