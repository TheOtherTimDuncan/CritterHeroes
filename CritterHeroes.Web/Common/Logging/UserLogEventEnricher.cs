using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Models.LogEvents;
using Microsoft.Owin;
using Serilog;

namespace CritterHeroes.Web.Common.Logging
{
    public class UserLogEventEnricher : IAppLogEventEnricher<UserLogEvent>
    {
        private IOwinContext _owinContext;

        public UserLogEventEnricher(IOwinContext owinContext)
        {
            this._owinContext = owinContext;
        }

        public ILogger Enrich(ILogger logger, UserLogEvent logEvent)
        {
            return logger.ForContext("IPAddress", _owinContext.Request.RemoteIpAddress);
        }
    }
}
