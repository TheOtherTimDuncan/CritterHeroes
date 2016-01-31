using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CritterHeroes.Web.Contracts.Logging
{
    public interface IUserLogger
    {
        void LogAction(string message, params object[] messageValues);
        void LogError(string message, params object[] messageValues);
        void LogError(string message, IEnumerable<string> errors, params object[] messageValues);
    }
}
