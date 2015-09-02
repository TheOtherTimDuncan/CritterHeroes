using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts;
using Moq;

namespace CH.Test.Mocks
{
    public class MockHttpClient : Mock<IHttpClient>
    {
        public MockHttpClient(string json)
        {
            SetupMock(json);
        }

        private void SetupMock(string json)
        {
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(json);

            this.Setup(x => x.PostAsync(It.IsAny<string>(), It.IsAny<HttpContent>())).Returns(Task.FromResult(response));
        }
    }
}
