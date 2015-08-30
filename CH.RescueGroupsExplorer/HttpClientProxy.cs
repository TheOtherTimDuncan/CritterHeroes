using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using CritterHeroes.Web.Contracts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TOTD.Utility.StringHelpers;

namespace CH.RescueGroupsExplorer
{
    public class HttpClientProxy : IHttpClient
    {
        private HttpClient _client;
        private TextBox _txtBox;

        public HttpClientProxy(TextBox txtBox)
        {
            _client = new HttpClient();
            _txtBox = txtBox;
        }

        public async Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
        {
            string requestBody = await content.ReadAsStringAsync();
            if (!requestBody.IsNullOrEmpty())
            {
                requestBody = JValue.Parse(requestBody).ToString(Formatting.Indented);
            }
            _txtBox.AppendText(Environment.NewLine);
            _txtBox.AppendText("Request:");
            _txtBox.AppendText(Environment.NewLine);
            _txtBox.AppendText(requestBody);
            _txtBox.AppendText(Environment.NewLine);

            HttpResponseMessage response = await _client.PostAsync(requestUri, content);

            string responseBody=  await response.Content.ReadAsStringAsync();
            if (!responseBody.IsNullOrEmpty())
            {
                responseBody = JValue.Parse(responseBody).ToString(Formatting.Indented);
            }
            _txtBox.AppendText(Environment.NewLine);
            _txtBox.AppendText("Response:");
            _txtBox.AppendText(Environment.NewLine);
            _txtBox.AppendText(responseBody);
            _txtBox.AppendText(Environment.NewLine);


            return response;
        }
    }
}
