using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace CH.RescueGroupsExplorer
{
    public class LogEntry
    {
        public string Url
        {
            get;
            set;
        }

        public string Request
        {
            get;
            set;
        }

        public string Response
        {
            get;
            set;
        }

        public HttpStatusCode StatusCode
        {
            get;
            set;
        }
    }
}
