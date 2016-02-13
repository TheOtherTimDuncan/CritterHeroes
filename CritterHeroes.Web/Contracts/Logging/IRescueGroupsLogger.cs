using System;
using System.Collections.Generic;
using System.Net;

namespace CritterHeroes.Web.Contracts.Logging
{
    public interface IRescueGroupsLogger
    {
        void LogRequest(string url, string request, string response, HttpStatusCode statusCode);
    }
}
