using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CH.Test.Mocks
{
    public class MockHttpClient : Mock<IHttpClient>
    {
        public MockHttpClient(string json)
        {
            SetupMock(json);
        }

        public MockHttpClient(params JProperty[] jsonProperties)
        {
            JObject jobect = new JObject(new JProperty("data", new JObject(jsonProperties)));
            jobect.Add(new JProperty("status", "ok"));
            string json = jobect.ToString(Formatting.Indented);
            SetupMock(json);
        }

        private void SetupMock(string json)
        {
            this.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>())).Returns((string requestUri, HttpContent content) =>
            {
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);

                string requestContent = content.ReadAsStringAsync().Result;
                if (requestContent.Contains("\"action\": \"login\""))
                {
                    response.Content = new StringContent("{ \"status\":\"ok\", data: { \"token\": \"token\", \"tokenHash\": \"tokenHash\" } }");
                }
                else
                {
                    response.Content = new StringContent(json);
                }

                return Task.FromResult(response);
            });
        }
    }
}
