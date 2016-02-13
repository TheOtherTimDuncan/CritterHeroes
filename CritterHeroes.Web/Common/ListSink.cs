using System;
using System.Collections.Generic;
using System.Linq;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;

namespace CritterHeroes.Web.Common
{
    public static class LoggerConfigurationListSinkExtensions
    {
        public static LoggerConfiguration List(this LoggerSinkConfiguration loggerConfiguration, List<string> messages)
        {
            ListSink sink = new ListSink(messages);
            return loggerConfiguration.Sink(sink);
        }
    }

    public class ListSink : ILogEventSink
    {
        private readonly List<string> _messages;

        private readonly object _syncRoot;

        public ListSink(List<string> messages)
        {
            this._messages = messages;

            this._syncRoot = new object();
        }

        public void Emit(LogEvent logEvent)
        {
            lock (_syncRoot)
            {
                string message = logEvent.RenderMessage();
                _messages.Add(message);
            }
        }
    }
}
