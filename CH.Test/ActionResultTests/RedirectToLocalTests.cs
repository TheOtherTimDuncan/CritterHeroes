using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CritterHeroes.Web.Common.ActionResults;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test.ActionResultTests
{
    [TestClass]
    public class RedirectToLocalTests : BaseTest
    {
        [TestMethod]
        public void RedirectsToGivenUrlIfLocal()
        {
            string url = "/debug/previous";

            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>();

            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Setup(x => x.Response).Returns(mockResponse.Object);

            ControllerContext controllerContext = new ControllerContext(mockHttpContext.Object, new RouteData(), new Mock<ControllerBase>().Object);

            RedirectToLocalResult result = new RedirectToLocalResult(url);
            result.ExecuteResult(controllerContext);

            mockResponse.Verify(x => x.Redirect(url, false), Times.Once);
        }
    }
}
