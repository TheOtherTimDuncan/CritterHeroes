using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CritterHeroes.Web.Models.Logging;

namespace CritterHeroes.Web.Contracts.Logging
{
    public interface IUserLogger
    {
        void LogAction(string message, params object[] messageValues);
        void LogError(string message, params object[] messageValues);
        void LogError(string message, IEnumerable<string> errors, params object[] messageValues);
    }
}
