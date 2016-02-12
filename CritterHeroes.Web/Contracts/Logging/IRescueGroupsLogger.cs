using System;
using System.Collections.Generic;

namespace CritterHeroes.Web.Contracts.Logging
{
    public interface IRescueGroupsLogger
    {
        void LogRequest(string url, string request, string response, int statusCode);
    }
}
