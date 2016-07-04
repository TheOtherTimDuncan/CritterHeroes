using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TOTD.Utility.EnumerableHelpers;

namespace CH.RescueGroupsImporter
{
    public class LoggingHandler : DelegatingHandler
    {
        private Writer _writer;

        public LoggingHandler(HttpMessageHandler innerHandler, Writer writer)
            : base(innerHandler)
        {
            _writer = writer;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _writer.WriteLine("-- BEGIN --");
            _writer.WriteLine();

            _writer.Write("Url: ");
            _writer.WriteLine(request.RequestUri.ToString());
            _writer.WriteLine();

            _writer.WriteLine("Headers:");

            foreach (var header in request.Headers)
            {
                _writer.Write($"    {header.Key}=");
                header.Value.NullSafeForEach(x => _writer.Write(x + " "));
                _writer.WriteLine();
            }

            _writer.WriteLine();

            if (request.Content != null)
            {
                _writer.WriteLine("Body:");

                string requestBody = await request.Content.ReadAsStringAsync();
                if (requestBody.StartsWith("{"))
                {
                    _writer.WriteLine(JValue.Parse(requestBody).ToString(Formatting.Indented));
                }
                else
                {
                    _writer.WriteLine(requestBody);
                }

                _writer.WriteLine();
            }

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            _writer.WriteLine("Response:");
            _writer.WriteLine();

            _writer.WriteLine($"Status code: {(int)response.StatusCode} - {response.StatusCode}");
            _writer.WriteLine();

            _writer.WriteLine("Headers: ");

            foreach (var header in response.Headers)
            {
                _writer.Write($"    {header.Key}=");
                header.Value.NullSafeForEach(x => _writer.Write(x + " "));
                _writer.WriteLine();
            }

            _writer.WriteLine();

            if (response.Content != null)
            {
                _writer.WriteLine("Body:");

                string responseBody = await response.Content.ReadAsStringAsync();
                if (responseBody.StartsWith("{"))
                {
                    _writer.WriteLine(JValue.Parse(responseBody).ToString(Formatting.Indented));
                }
                else
                {
                    _writer.WriteLine(responseBody);
                }

                _writer.WriteLine();
            }

            _writer.WriteLine("-- END --");
            _writer.WriteLine();

            return response;
        }
    }
}
