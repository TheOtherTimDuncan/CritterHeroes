using System;
using System.Collections.Generic;
using System.Linq;
using Serilog.Events;
using TOTD.Utility.ExceptionHelpers;

namespace CritterHeroes.Web.Models.LogEvents
{
    public class LogEventCategory
    {
        public const string User = "User";
        public const string Email = "Email";
        public const string Critter = "Critter";
        public const string RescueGroups = "RescueGroups";
        public const string History = "History";
    }

    public class AppLogEvent
    {
        public AppLogEvent(string category, LogEventLevel level, string messageTemplate, params object[] messageValues)
        {
            ThrowIf.Argument.IsNullOrEmpty(category, nameof(category));
            ThrowIf.Argument.IsNullOrEmpty(messageTemplate, nameof(messageTemplate));

            this.Category = category;
            this.Level = level;
            this.MessageTemplate = messageTemplate;
            this.MessageValues = messageValues;
        }

        public AppLogEvent(object context, string category, LogEventLevel level, string messageTemplate, params object[] messageValues)
           : this(category, level, messageTemplate, messageValues)
        {
            ThrowIf.Argument.IsNull(context, nameof(context));

            this.Context = context;
        }

        public string Category
        {
            get;
        }

        public LogEventLevel Level
        {
            get;
        }

        public string MessageTemplate
        {
            get;
        }

        public object[] MessageValues
        {
            get;
        }

        public object Context
        {
            get;
        }
    }
}
