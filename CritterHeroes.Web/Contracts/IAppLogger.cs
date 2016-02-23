﻿using System;
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

        void LogEvent(LogEvent logEvent);
    }
}