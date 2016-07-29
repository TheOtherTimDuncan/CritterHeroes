using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Mvc;
using CH.Test.Mocks;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Models.Identity;
using CritterHeroes.Web.Middleware;
using CritterHeroes.Web.Shared.StateManagement;
using FluentAssertions;
using Microsoft.Owin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test.MiddlewareTests
{
    [TestClass]
    public class UserContextMiddlewareTests : BaseTest
    {
        [TestMethod]
        public async Task DoesNothingIfUserIsNotAuthenticated()
        {
            Mock<IPrincipal> mockPrincipal = new Mock<IPrincipal>();
            mockPrincipal.Setup(x => x.Identity.IsAuthenticated).Returns(false);

            Mock<IOwinContext> mockOwinContext = new Mock<IOwinContext>();
            mockOwinContext.Setup(x => x.Request.User).Returns(mockPrincipal.Object);
            mockOwinContext.Setup(x => x.Request.Path).Returns(new PathString("/Home"));

            // No methods should be called on mockResolver
            Mock<IDependencyResolver> mockResolver = new Mock<IDependencyResolver>(MockBehavior.Strict);

            TestEndMiddleware testMiddleware = new TestEndMiddleware();
            testMiddleware.isInvoked.Should().BeFalse();

            UserContextMiddleware middleware = new UserContextMiddleware(testMiddleware, mockResolver.Object);
            await middleware.Invoke(mockOwinContext.Object);

            testMiddleware.isInvoked.Should().BeTrue("next middeleware should always be invoked");

            mockPrincipal.Verify(x => x.Identity.IsAuthenticated, Times.Once);
        }

        [TestMethod]
        public async Task DoesNothingIfAuthenticatedUserIsLoggingOut()
        {
            Mock<IPrincipal> mockPrincipal = new Mock<IPrincipal>();
            mockPrincipal.Setup(x => x.Identity.IsAuthenticated).Returns(true);

            Mock<IOwinContext> mockOwinContext = new Mock<IOwinContext>();
            mockOwinContext.Setup(x => x.Request.User).Returns(mockPrincipal.Object);
            mockOwinContext.Setup(x => x.Request.Path).Returns(new PathString("/Account/Logout"));

            // No methods should be called on mockResolver
            Mock<IDependencyResolver> mockResolver = new Mock<IDependencyResolver>(MockBehavior.Strict);

            TestEndMiddleware testMiddleware = new TestEndMiddleware();
            testMiddleware.isInvoked.Should().BeFalse();

            UserContextMiddleware middleware = new UserContextMiddleware(testMiddleware, mockResolver.Object);
            await middleware.Invoke(mockOwinContext.Object);

            testMiddleware.isInvoked.Should().BeTrue("next middeleware should always be invoked");

            mockPrincipal.Verify(x => x.Identity.IsAuthenticated, Times.Once);
            mockOwinContext.Verify(x => x.Request.Path, Times.Once);
        }

        [TestMethod]
        public async Task DoesNothingIfUserContextCookieAlreadyExists()
        {
            UserContext context = new UserContext()
            {
                UserID = "ID",
                DisplayName = "First Last"
            };

            Mock<IStateManager<UserContext>> mockStateManager = new Mock<IStateManager<UserContext>>();
            mockStateManager.Setup(x => x.GetContext()).Returns(context);

            // Only the setup methods for mockResolver should be called
            Mock<IDependencyResolver> mockResolver = new Mock<IDependencyResolver>(MockBehavior.Strict);
            mockResolver.Setup(x => x.GetService(typeof(IStateManager<UserContext>))).Returns(mockStateManager.Object);

            Mock<IOwinContext> mockOwinContext = GetMockOwinContext();

            TestEndMiddleware testMiddleware = new TestEndMiddleware();
            testMiddleware.isInvoked.Should().BeFalse();

            UserContextMiddleware middleware = new UserContextMiddleware(testMiddleware, mockResolver.Object);
            await middleware.Invoke(mockOwinContext.Object);

            testMiddleware.isInvoked.Should().BeTrue("next middleware should always be invoked");

            mockStateManager.Verify(x => x.GetContext(), Times.Once);
            mockStateManager.Verify(x => x.SaveContext(It.IsAny<UserContext>()), Times.Never);
            mockResolver.Verify(x => x.GetService(typeof(IStateManager<UserContext>)), Times.Once);
        }

        [TestMethod]
        public async Task GetsUserContextFromStorageIfNotAlreadyCachedInRequestAndCachesContext()
        {
            AppUser user = new AppUser("unit.test");
            user.Person.FirstName = "First";
            user.Person.LastName = "Last";

            ClaimsIdentity identity = new ClaimsIdentity("cookie");
            identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
            identity.IsAuthenticated.Should().BeTrue();
            identity.Name.Should().Be(user.UserName);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            Mock<IStateManager<UserContext>> mockStateManager = new Mock<IStateManager<UserContext>>();
            mockStateManager.Setup(x => x.GetContext()).Returns((UserContext)null);

            MockSqlQueryStorageContext<AppUser> mockUserStorageContext = new MockSqlQueryStorageContext<AppUser>(user);

            // Only the setup methods for mockResolver should be called
            Mock<IDependencyResolver> mockResolver = new Mock<IDependencyResolver>(MockBehavior.Strict);
            mockResolver.Setup(x => x.GetService(typeof(IStateManager<UserContext>))).Returns(mockStateManager.Object);
            mockResolver.Setup(x => x.GetService(typeof(ISqlQueryStorageContext<AppUser>))).Returns(mockUserStorageContext.Object);

            Mock<IOwinContext> mockOwinContext = GetMockOwinContext();
            mockOwinContext.Setup(x => x.Request.User).Returns(principal);

            TestEndMiddleware testMiddleware = new TestEndMiddleware();
            testMiddleware.isInvoked.Should().BeFalse();

            UserContextMiddleware middleware = new UserContextMiddleware(testMiddleware, mockResolver.Object);
            await middleware.Invoke(mockOwinContext.Object);

            testMiddleware.isInvoked.Should().BeTrue("next middeleware should always be invoked");

            mockStateManager.Verify(x => x.GetContext(), Times.Once);
            mockStateManager.Verify(x => x.SaveContext(It.IsAny<UserContext>()), Times.Once);
            mockResolver.Verify(x => x.GetService(typeof(IStateManager<UserContext>)), Times.Once);
            mockResolver.Verify(x => x.GetService(typeof(ISqlQueryStorageContext<AppUser>)), Times.Once);
        }

        private Mock<IOwinContext> GetMockOwinContext()
        {
            Mock<IPrincipal> mockPrincipal = new Mock<IPrincipal>();
            mockPrincipal.Setup(x => x.Identity.IsAuthenticated).Returns(true);

            Mock<IOwinContext> mockOwinContext = new Mock<IOwinContext>();
            mockOwinContext.Setup(x => x.Request.User).Returns(mockPrincipal.Object);
            mockOwinContext.Setup(x => x.Request.Path).Returns(new PathString("/Home"));

            return mockOwinContext;
        }
    }
}
