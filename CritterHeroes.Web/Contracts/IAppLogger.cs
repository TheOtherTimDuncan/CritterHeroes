using System;
using System.Collections.Generic;
using CritterHeroes.Web.Models.LogEvents;

namespace CritterHeroes.Web.Contracts
{
    public interface IAppLogger
    {
        IEnumerable<string> Messages
        {
            get;
        }

        void LogEvent(AppLogEvent logEvent);
        void LogEvent<ContextType>(AppLogEvent<ContextType> logEvent) where ContextType : class, new();
    }
}
