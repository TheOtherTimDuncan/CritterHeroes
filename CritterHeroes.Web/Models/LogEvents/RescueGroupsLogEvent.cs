using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Serilog.Events;

namespace CritterHeroes.Web.Models.LogEvents
{
    public class RescueGroupsLogEvent : AppLogEvent
    {
        public static RescueGroupsLogEvent LogRequest(string url, string request, string response, HttpStatusCode statusCode)
        {
            RescueGroupsContext context = new RescueGroupsContext(url, request, response, statusCode);
            return new RescueGroupsLogEvent(context, "Sent request and received status code {StatusCode}", statusCode);
        }

        private RescueGroupsLogEvent(RescueGroupsContext context, string messageTemplate, params object[] messageValues)
            : base(context, LogEventCategory.RescueGroups, LogEventLevel.Information, messageTemplate, messageValues)
        {
        }

        public class RescueGroupsContext
        {
            public RescueGroupsContext(string url, string request, string response, HttpStatusCode statusCode)
            {
                this.Url = url;
                this.Request = request;
                this.Response = response;
                this.StatusCode = statusCode;
            }

            public string Url
            {
                get;
            }

            public string Request
            {
                get;
            }

            public string Response
            {
                get;
            }

            public HttpStatusCode StatusCode
            {
                get;
            }
        }
    }
}
