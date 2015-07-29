using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using CH.Test.Mocks;
using CritterHeroes.Web;
using CritterHeroes.Web.Areas.Common;
using CritterHeroes.Web.Common.Dispatchers;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Email;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.Logging;
using CritterHeroes.Web.Contracts.Notifications;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Data.Models.Identity;
using FluentAssertions;
using Microsoft.Owin;
using Microsoft.Owin.Security;
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
        public Mock<IAppSignInManager> mockSignInManager;
        public Mock<IUrlGenerator> mockUrlGenerator;
        public Mock<IEmailClient> mockEmailClient;
        public Mock<IAzureAppUserStore> mockUserStore;
        public Mock<IAuthenticationManager> mockAuthenticationManager;
        public Mock<IHttpUser> mockHttpUser;
        public Mock<IOwinContext> mockOwinContext;
        public Mock<IEmailService> mockEmailService;
        public Mock<IStateManager<OrganizationContext>> mockOrganizationStateManager;
        public Mock<IStateManager<UserContext>> mockUserContextManager;
        public Mock<IAppUserManager> mockUserManager;
        public OrganizationContext organizationContext;
        public UserContext userContext;
        public MockSqlStorageContext<AppUser> mockAppUserStorageContext;

        public const string webAppPath = "/debug/";

        [TestInitialize]
        public void InitializeTest()
        {
            container = new Container();
            DIConfig.RegisterHandlers(container, new Assembly[] { typeof(MvcApplication).Assembly });

            container.Register<ICommandDispatcher, CommandDispatcher>();
            container.Register<IQueryDispatcher, QueryDispatcher>();

            mockUserLogger = new Mock<IUserLogger>();
            container.Register<IUserLogger>(() => mockUserLogger.Object);

            mockSignInManager = new Mock<IAppSignInManager>();
            container.Register<IAppSignInManager>(() => mockSignInManager.Object);

            mockUserManager = new Mock<IAppUserManager>();
            container.Register(() => mockUserManager.Object);

            mockUrlGenerator = new Mock<IUrlGenerator>();
            container.Register<IUrlGenerator>(() => mockUrlGenerator.Object);

            mockEmailClient = new Mock<IEmailClient>();
            container.Register<IEmailClient>(() => mockEmailClient.Object);

            mockUserStore = new Mock<IAzureAppUserStore>();
            container.Register<IAzureAppUserStore>(() => mockUserStore.Object);

            mockAuthenticationManager = new Mock<IAuthenticationManager>();
            container.Register<IAuthenticationManager>(() => mockAuthenticationManager.Object);

            mockHttpUser = new Mock<IHttpUser>();
            container.Register(() => mockHttpUser.Object, Lifestyle.Singleton);

            mockUserContextManager = new Mock<IStateManager<UserContext>>();
            container.Register(() => mockUserContextManager.Object);

            mockOwinContext = new Mock<IOwinContext>();
            container.Register(() => mockOwinContext.Object);

            mockOrganizationStateManager = new Mock<IStateManager<OrganizationContext>>();
            organizationContext = new OrganizationContext();
            mockOrganizationStateManager.Setup(x => x.GetContext()).Returns(organizationContext);
            container.Register(() => mockOrganizationStateManager.Object);

            mockEmailService = new Mock<IEmailService>();
            container.Register(() => mockEmailService.Object);

            mockAppUserStorageContext = new MockSqlStorageContext<AppUser>();
            container.Register(() => mockAppUserStorageContext.Object);

            container.Register<INotificationPublisher, NotificationPublisher>();
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
            mockRequest.Setup(x => x.ApplicationPath).Returns(webAppPath);
            mockRequest.Setup(x => x.Url).Returns(new Uri("http://localhost/debug"));

            Mock<HttpResponseBase> mockResponse = new Mock<HttpResponseBase>();
            mockResponse.Setup(x => x.ApplyAppPathModifier(It.IsAny<string>())).Returns((string s) => s);

            Mock<HttpContextBase> mockHttpContext = new Mock<HttpContextBase>();
            mockHttpContext.Setup(x => x.Items).Returns(mockItems.Object);
            mockHttpContext.Setup(x => x.Request).Returns(mockRequest.Object);
            mockHttpContext.Setup(x => x.Response).Returns(mockResponse.Object);

            RequestContext requestContext = new RequestContext(mockHttpContext.Object, new RouteData());
            mockRequest.Setup(x => x.RequestContext).Returns(requestContext);

            return mockHttpContext;
        }

        public RouteData GetRequestRouteData(HttpContextBase httpContext)
        {
            return httpContext.Request.RequestContext.RouteData;
        }

        public T CreateController<T>() where T : BaseController
        {
            return container.GetInstance<T>();
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
