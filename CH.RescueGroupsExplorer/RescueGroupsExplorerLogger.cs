using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Models.LogEvents;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TOTD.Utility.StringHelpers;

namespace CH.RescueGroupsExplorer
{
    public class RescueGroupsExplorerLogger : IAppLogger
    {
        private TextBox _txtBox;
        private List<LogEntry> _entries;

        private readonly object _syncRoot;

        public IEnumerable<string> Messages
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public RescueGroupsExplorerLogger(TextBox textBox)
        {
            this._txtBox = textBox;
            this._entries = new List<LogEntry>();

            this._syncRoot = new object();
        }

        public void LogEvent<LogEventType>(LogEventType logEvent) where LogEventType : AppLogEvent
        {
            lock (_syncRoot)
            {
                RescueGroupsLogEvent.RescueGroupsContext context = (RescueGroupsLogEvent.RescueGroupsContext)logEvent.Context;
                LogEntry entry = new LogEntry()
                {
                    Url = context.Url,
                    Request = context.Request,
                    Response = context.Response,
                    StatusCode = context.StatusCode
                };
                _entries.Add(entry);
            }
        }

        public void Flush()
        {
            foreach (LogEntry entry in _entries)
            {
                _txtBox.AppendText("Url: ");
                _txtBox.AppendText(entry.Url);
                _txtBox.AppendText(Environment.NewLine);

                _txtBox.AppendText("Status code: ");
                _txtBox.AppendText(entry.StatusCode.ToString());
                _txtBox.AppendText(Environment.NewLine);

                string requestBody = entry.Request;
                if (!requestBody.IsNullOrEmpty() && requestBody != "Login")
                {
                    requestBody = JValue.Parse(requestBody).ToString(Formatting.Indented);
                }
                _txtBox.AppendText(Environment.NewLine);
                _txtBox.AppendText("Request:");
                _txtBox.AppendText(Environment.NewLine);
                _txtBox.AppendText(requestBody);
                _txtBox.AppendText(Environment.NewLine);

                _txtBox.AppendText(Environment.NewLine);
                string responseBody = entry.Response;
                if (!responseBody.IsNullOrEmpty())
                {
                    responseBody = JValue.Parse(responseBody).ToString(Formatting.Indented);
                }
                _txtBox.AppendText(Environment.NewLine);
                _txtBox.AppendText("Response:");
                _txtBox.AppendText(Environment.NewLine);
                _txtBox.AppendText(responseBody);
                _txtBox.AppendText(Environment.NewLine);
                _txtBox.AppendText(Environment.NewLine);
            }

            _entries.Clear();
        }

        private class LogEntry
        {
            public string Url
            {
                get;
                set;
            }

            public string Request
            {
                get;
                set;
            }

            public string Response
            {
                get;
                set;
            }

            public HttpStatusCode StatusCode
            {
                get;
                set;
            }
        }
    }
}
