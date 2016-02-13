using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using CritterHeroes.Web.Contracts.Logging;

namespace CH.RescueGroupsExplorer
{
    public class RescueGroupsExplorerLogger : IRescueGroupsLogger
    {
        private TextBox _txtBox;
        private StringWriter _writer;

        public RescueGroupsExplorerLogger(TextBox textBox)
        {
            _txtBox = textBox;
        }

        public void LogRequest(string url, string request, string response, HttpStatusCode statusCode)
        {
            throw new NotImplementedException();
        }
    }
}
