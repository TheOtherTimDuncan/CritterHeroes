using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Routing;
using Moq;

namespace CH.Test.Mocks
{
    public class MockHttpContext : Mock<HttpContextBase>
    {
        public const string WebAppPath = "/debug/";

        public MockHttpContext()
        {
            Mock<IDictionary> mockItems = new Mock<IDictionary>();
            mockItems.SetupGet(x => x[It.IsAny<string>()]).Returns(new Dictionary<string, object>());

            Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();
            mockRequest.Setup(x => x.ApplicationPath).Returns(WebAppPath);
            mockRequest.Setup(x => x.Url).Returns(new Uri("http://localhost/debug"));
            mockRequest.Setup(x => x.Headers).Returns(new NameValueCollection());

            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>();
            mockResponse.Setup(x => x.ApplyAppPathModifier(It.IsAny<string>())).Returns((string s) => s);

            Setup(x => x.Items).Returns(mockItems.Object);
            Setup(x => x.Request).Returns(mockRequest.Object);
            Setup(x => x.Response).Returns(mockResponse.Object);

            RequestContext requestContext = new RequestContext(this.Object, new RouteData());
            mockRequest.Setup(x => x.RequestContext).Returns(requestContext);
        }
    }
}
