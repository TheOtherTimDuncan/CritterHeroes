using System;
using System.Collections.Generic;
using System.Linq;

namespace CritterHeroes.Web.Contracts.Logging
{
    public interface ICritterLogger
    {
        string Messages
        {
            get;
        }

        void LogAction(string message, params object[] messageValues);
        void LogError(string message, params object[] messageValues);
    }
}
