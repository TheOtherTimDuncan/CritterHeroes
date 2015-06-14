using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web.Mvc;
using CritterHeroes.Web.Common.Identity;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Identity;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Middleware;
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
        public async Task DoesNothingIfUserContextAlreadyExistsInOwinContext()
        {
            UserContext context = new UserContext()
            {
                UserID = "ID",
                DisplayName = "First Last"
            };

            // No methods should be called on mockResolver
            Mock<IDependencyResolver> mockResolver = new Mock<IDependencyResolver>(MockBehavior.Strict);

            TestEndMiddleware testMiddleware = new TestEndMiddleware();
            testMiddleware.isInvoked.Should().BeFalse();

            Mock<IOwinContext> mockOwinContext = GetMockOwinContext();
            mockOwinContext.Setup(x => x.Get<UserContext>(It.IsAny<string>())).Returns(context);

            UserContextMiddleware middleware = new UserContextMiddleware(testMiddleware, mockResolver.Object);
            await middleware.Invoke(mockOwinContext.Object);

            testMiddleware.isInvoked.Should().BeTrue("next middeleware should always be invoked");

            mockOwinContext.Verify(x => x.Get<UserContext>(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task GetsUserContextFromRequestIfNotAlreadyCachedInOwinContextAndCachesContext()
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
            mockOwinContext.Setup(x => x.Get<UserContext>(It.IsAny<string>())).Returns((UserContext)null);

            TestEndMiddleware testMiddleware = new TestEndMiddleware();
            testMiddleware.isInvoked.Should().BeFalse();

            UserContextMiddleware middleware = new UserContextMiddleware(testMiddleware, mockResolver.Object);
            await middleware.Invoke(mockOwinContext.Object);

            testMiddleware.isInvoked.Should().BeTrue("next middeleware should always be invoked");

            mockStateManager.Verify(x => x.GetContext(), Times.Once);
            mockOwinContext.Verify(x => x.Get<UserContext>(It.IsAny<string>()), Times.Once);
            mockOwinContext.Verify(x => x.Set<UserContext>(It.IsAny<string>(), context), Times.Once);
            mockResolver.Verify(x => x.GetService(typeof(IStateManager<UserContext>)), Times.Once);
        }

        [TestMethod]
        public async Task GetsUserContextFromStorageIfNotAlreadyCachedInRequestAndCachesContext()
        {
            IdentityUser user = new IdentityUser("unit.test")
            {
                FirstName = "First",
                LastName = "Last"
            };

            ClaimsIdentity identity = new ClaimsIdentity("cookie");
            identity.AddClaim(new Claim(AppClaimTypes.UserID, user.Id));
            identity.IsAuthenticated.Should().BeTrue();
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            Mock<IStateManager<UserContext>> mockStateManager = new Mock<IStateManager<UserContext>>();
            mockStateManager.Setup(x => x.GetContext()).Returns((UserContext)null);

            Mock<IApplicationUserStore> mockUserStore = new Mock<IApplicationUserStore>();
            mockUserStore.Setup(x => x.FindByIdAsync(user.Id)).Returns(Task.FromResult(user));

            // Only the setup methods for mockResolver should be called
            Mock<IDependencyResolver> mockResolver = new Mock<IDependencyResolver>(MockBehavior.Strict);
            mockResolver.Setup(x => x.GetService(typeof(IStateManager<UserContext>))).Returns(mockStateManager.Object);
            mockResolver.Setup(x => x.GetService(typeof(IApplicationUserStore))).Returns(mockUserStore.Object);

            Mock<IOwinContext> mockOwinContext = GetMockOwinContext();
            mockOwinContext.Setup(x => x.Get<UserContext>(It.IsAny<string>())).Returns((UserContext)null);
            mockOwinContext.Setup(x => x.Request.User).Returns(principal);

            TestEndMiddleware testMiddleware = new TestEndMiddleware();
            testMiddleware.isInvoked.Should().BeFalse();

            UserContextMiddleware middleware = new UserContextMiddleware(testMiddleware, mockResolver.Object);
            await middleware.Invoke(mockOwinContext.Object);

            testMiddleware.isInvoked.Should().BeTrue("next middeleware should always be invoked");

            mockStateManager.Verify(x => x.GetContext(), Times.Once);
            mockStateManager.Verify(x => x.SaveContext(It.IsAny<UserContext>()), Times.Once);
            mockUserStore.Verify(x => x.FindByIdAsync(user.Id), Times.Once);
            mockResolver.Verify(x => x.GetService(typeof(IStateManager<UserContext>)), Times.Once);
            mockResolver.Verify(x => x.GetService(typeof(IApplicationUserStore)), Times.Once);
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
