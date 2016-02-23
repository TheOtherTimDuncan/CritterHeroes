using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Serilog.Events;

namespace CritterHeroes.Web.Models.LogEvents
{
    public class RescueGroupsLogEvent : LogEvent
    {
        public static RescueGroupsLogEvent LogRequest(string url, string request, string response, HttpStatusCode statusCode)
        {
            return new RescueGroupsLogEvent("Sent {Request} to {Url} and received status code {StatusCode} with {Response}", request, url, statusCode, response);
        }

        private RescueGroupsLogEvent(string messageTemplate, params object[] messageValues)
            : base(LogEventCategory.RescueGroups, LogEventLevel.Information, messageTemplate, messageValues)
        {
        }
    }
}
