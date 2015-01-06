using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CritterHeroes.Web;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Email;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.Logging;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SimpleInjector;
using TOTD.Mvc.FluentHtml;

namespace CH.Test.ControllerTests
{
    public class BaseControllerTest : BaseTest
    {
        public Container container;

        public Mock<IUserLogger> mockUserLogger;
        public Mock<IApplicationSignInManager> mockSignInManager;
        public Mock<IApplicationUserManager> mockUserManager;
        public Mock<IUrlGenerator> mockUrlGenerator;
        public Mock<IEmailClient> mockEmailClient;

        [TestInitialize]
        public void InitializeTest()
        {
            container = new Container();
            DIConfig.RegisterQueryAndCommandHandlers(container, new Assembly[] { typeof(MvcApplication).Assembly });

            mockUserLogger = new Mock<IUserLogger>();
            container.Register<IUserLogger>(() => mockUserLogger.Object);

            mockSignInManager = new Mock<IApplicationSignInManager>();
            container.Register<IApplicationSignInManager>(() => mockSignInManager.Object);

            mockUserManager = new Mock<IApplicationUserManager>();
            container.Register<IApplicationUserManager>(() => mockUserManager.Object);

            mockUrlGenerator = new Mock<IUrlGenerator>();
            container.Register<IUrlGenerator>(() => mockUrlGenerator.Object);

            mockEmailClient = new Mock<IEmailClient>();
            container.Register<IEmailClient>(() => mockEmailClient.Object);
        }

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
