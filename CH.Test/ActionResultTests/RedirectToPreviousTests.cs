using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CritterHeroes.Web.Common.ActionResults;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.StateManagement;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test.ActionResultTests
{
    [TestClass]
    public class RedirectToPreviousTests
    {
        [TestMethod]
        public void RedirectsToPreviousPathFromPageContext()
        {
            PageContext pageContext = new PageContext()
            {
                PreviousPath = "/debug/previous"
            };

            Mock<IPageContextService> mockPageContextService = new Mock<IPageContextService>();
            mockPageContextService.Setup(x => x.GetPageContext()).Returns(pageContext);

            Mock<IDependencyResolver> mockDependencyResolver = new Mock<IDependencyResolver>();
            mockDependencyResolver.Setup(x => x.GetService(typeof(IPageContextService))).Returns(mockPageContextService.Object);
            DependencyResolver.SetResolver(mockDependencyResolver.Object);

            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>();
            
            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Setup(x => x.Response).Returns(mockResponse.Object);

            ControllerContext controllerContext = new ControllerContext(mockHttpContext.Object, new RouteData(), new Mock<ControllerBase>().Object);

            RedirectToPreviousResult result = new RedirectToPreviousResult();
            result.ExecuteResult(controllerContext);

            mockResponse.Verify(x => x.Redirect(pageContext.PreviousPath, false), Times.Once);
        }
    }
}
