using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CritterHeroes.Web.Contracts.Events;
using CritterHeroes.Web.Models.LogEvents;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TOTD.Utility.StringHelpers;

namespace CH.RescueGroupsExplorer
{
    public class RescueGroupsExplorerLogger : IAppEventPublisher
    {
        private TextBox _txtBox;

        private readonly object _syncRoot;

        public RescueGroupsExplorerLogger(TextBox textBox)
        {
            this._txtBox = textBox;
            this.Entries = new List<LogEntry>();

            this._syncRoot = new object();
        }

        public List<LogEntry> Entries
        {
            get;
        }

        public void Publish<TAppEvent>(TAppEvent appEvent) where TAppEvent : IAppEvent
        {
            lock (_syncRoot)
            {
                RescueGroupsLogEvent logEvent = appEvent as RescueGroupsLogEvent;
                RescueGroupsLogEvent.RescueGroupsContext context = (RescueGroupsLogEvent.RescueGroupsContext)logEvent.Context;
                LogEntry entry = new LogEntry()
                {
                    Url = context.Url,
                    Request = context.Request,
                    Response = context.Response,
                    StatusCode = context.StatusCode
                };
                Entries.Add(entry);
            }
        }

        public void Flush()
        {
            foreach (LogEntry entry in Entries)
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

            Entries.Clear();
        }

        public void Subscribe<TEventType>(Action<TEventType> handler) where TEventType : IAppEvent
        {
            throw new NotImplementedException();
        }
    }
}
