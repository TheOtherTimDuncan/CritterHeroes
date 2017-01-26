using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CritterHeroes.Web.DataProviders.RescueGroups.Models;
using CritterHeroes.Web.Domain.Contracts;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CH.Test.Mocks
{
    public class MockHttpClient : Mock<IHttpClient>
    {
        private Dictionary<string, object> _responses;
        private Dictionary<string, Action<string>> _callbacks;

        public MockHttpClient()
        {
            _responses = new Dictionary<string, object>();
            _callbacks = new Dictionary<string, Action<string>>();
            SetupMock();
        }

        public MockHttpClient SetupLoginResponse()
        {
            SetupResponseForAction(ObjectActions.Login, "{ \"status\":\"ok\", data: { \"token\": \"token\", \"tokenHash\": \"tokenHash\" } }");
            return this;
        }

        public MockHttpClient SetupListResponse(string json)
        {
            _responses.Add(ObjectActions.List, CreateJsonDataResponse(json));
            return this;
        }

        public MockHttpClient SetupSearchResponse(string json)
        {
            _responses.Add(ObjectActions.Search, CreateJsonDataResponse(json));
            return this;
        }

        public MockHttpClient SetupResponseForAction(string action, object response)
        {
            _responses.Add(action, response);
            return this;
        }

        public MockHttpClient Callback(string action, Action<string> callback)
        {
            this._callbacks.Add(action, callback);
            return this;
        }

        private void SetupMock()
        {
            this.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>())).Returns((string requestUri, HttpContent content) =>
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

                string requestContent = content.ReadAsStringAsync().Result;
                JObject requestJson = JObject.Parse(requestContent);

                JProperty actionProperty = requestJson.Property("action");
                if (actionProperty == null)
                {
                    actionProperty = requestJson.Property("objectAction");
                }
                string action = actionProperty.Value.Value<string>();

                Action<string> callback;
                if (_callbacks.TryGetValue(action, out callback))
                {
                    callback(requestContent);
                }

                object responseData = _responses[action];

                if (responseData is string)
                {
                    response.Content = new StringContent((string)responseData);
                }
                else
                {
                    string responseJson = JsonConvert.SerializeObject(responseData);
                    response.Content = new StringContent((string)responseJson);
                }

                return Task.FromResult(response);
            });
        }

        private string CreateJsonDataResponse(string jsonData)
        {
            string json = $@"
{{
    ""status"": ""ok"",
    ""messages"": {{
        ""generalMessages"": [],
        ""recordMessages"": []
    }},
    ""foundRows"": ""0"",
    ""data"":
        {jsonData}
}}
";
            Console.WriteLine(json);
            return json;
        }
    }
}
