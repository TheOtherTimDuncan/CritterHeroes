using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using FluentAssertions;
using Moq;
using TOTD.Mvc.FluentHtml;

namespace CH.Test.ControllerTests
{
    public class BaseControllerTest : BaseTest
    {
        public RouteCollection GetRouteCollection()
        {
            RouteCollection result = new RouteCollection();
            result.Add(new Route("{controller}/{action}/{id}", null)
            {
                Defaults = new RouteValueDictionary(new
                {
                    id = "defaultid"
                })
            });
            return result;
        }

        public Mock<HttpContextBase> GetMockHttpContext()
        {
            Mock<IDictionary> mockItems = new Mock<IDictionary>();
            mockItems.SetupGet(x => x[It.IsAny<string>()]).Returns(new Dictionary<string, object>());

            Mock<HttpRequestBase> mockRequest = new Mock<HttpRequestBase>();

            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Setup(x => x.Items).Returns(mockItems.Object);
            mockHttpContext.Setup(x => x.Request).Returns(mockRequest.Object);

            RequestContext requestContext = new RequestContext(mockHttpContext.Object, new RouteData());
            mockRequest.Setup(x => x.RequestContext).Returns(requestContext);

            return mockHttpContext;
        }

        public RouteData GetRequestRouteData(HttpContextBase httpContext)
        {
            return httpContext.Request.RequestContext.RouteData;
        }

        public ControllerContext CreateControllerContext(Mock<HttpContextBase> mockHttpContext, ControllerBase controller)
        {
            return new ControllerContext(mockHttpContext.Object, GetRequestRouteData(mockHttpContext.Object), controller); ;
        }

        public void VerifyRedirectToRouteResult(RedirectToRouteResult redirectResult, string actionName, string controllerName)
        {
            redirectResult.Should().NotBeNull();
            redirectResult.RouteValues[RouteValueKeys.Controller].Should().Be(controllerName);
            redirectResult.RouteValues[RouteValueKeys.Action].Should().Be(actionName);
        }
    }
}
