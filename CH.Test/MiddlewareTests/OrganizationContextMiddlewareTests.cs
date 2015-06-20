using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Middleware;
using CritterHeroes.Web.Models;
using FluentAssertions;
using Microsoft.Owin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test.MiddlewareTests
{
    [TestClass]
    public class OrganizationContextMiddlewareTests : BaseTest
    {
        [TestMethod]
        public async Task DoesNothingIfOrganizationContextCookieAlreadyExists()
        {
            OrganizationContext context = new OrganizationContext()
            {
                OrganizationID = Guid.NewGuid(),
                ShortName = "short",
                FullName = "full",
                AzureName = "azure",
                LogoFilename = "logo",
                EmailAddress = "email@email.com",
                SupportedCritters = GetTestSupportedSpecies()
            };

            Mock<IStateManager<OrganizationContext>> mockStateManager = new Mock<IStateManager<OrganizationContext>>();
            mockStateManager.Setup(x => x.GetContext()).Returns(context);

            // Only the setup methods for mockResolver should be called
            Mock<IDependencyResolver> mockResolver = new Mock<IDependencyResolver>(MockBehavior.Strict);
            mockResolver.Setup(x => x.GetService(typeof(IStateManager<OrganizationContext>))).Returns(mockStateManager.Object);

            Mock<IOwinContext> mockOwinContext = new Mock<IOwinContext>();

            TestEndMiddleware testMiddleware = new TestEndMiddleware();
            testMiddleware.isInvoked.Should().BeFalse();

            OrganizationContextMiddleware middleware = new OrganizationContextMiddleware(testMiddleware, mockResolver.Object);
            await middleware.Invoke(mockOwinContext.Object);

            testMiddleware.isInvoked.Should().BeTrue("next middeleware should always be invoked");

            mockStateManager.Verify(x => x.SaveContext(It.IsAny<OrganizationContext>()), Times.Never);
            mockStateManager.Verify(x => x.GetContext(), Times.Once);
            mockResolver.Verify(x => x.GetService(typeof(IStateManager<OrganizationContext>)), Times.Once);
        }

        [TestMethod]
        public async Task GetsOrganizationContextFromStorageIfNotAlreadyCachedInRequestAndCachesContext()
        {
            Organization organization = new Organization()
            {
                ShortName = "short",
                FullName = "full",
                AzureName = "azure",
                LogoFilename = "logo",
                EmailAddress = "email@email.com",
                SupportedCritters = GetTestSupportedSpecies()
            };

            Mock<IStateManager<OrganizationContext>> mockStateManager = new Mock<IStateManager<OrganizationContext>>();
            mockStateManager.Setup(x => x.GetContext()).Returns((OrganizationContext)null);

            Mock<IStorageContext<Organization>> mockStorageContext = new Mock<IStorageContext<Organization>>();
            mockStorageContext.Setup(x => x.GetAsync(organization.ID.ToString())).Returns(Task.FromResult(organization));

            Mock<IAppConfiguration> mockConfiguration = new Mock<IAppConfiguration>();
            mockConfiguration.Setup(x => x.OrganizationID).Returns(organization.ID);

            // Only the setup methods for mockResolver should be called
            Mock<IDependencyResolver> mockResolver = new Mock<IDependencyResolver>(MockBehavior.Strict);
            mockResolver.Setup(x => x.GetService(typeof(IStateManager<OrganizationContext>))).Returns(mockStateManager.Object);
            mockResolver.Setup(x => x.GetService(typeof(IStorageContext<Organization>))).Returns(mockStorageContext.Object);
            mockResolver.Setup(x => x.GetService(typeof(IAppConfiguration))).Returns(mockConfiguration.Object);

            Mock<IOwinContext> mockOwinContext = new Mock<IOwinContext>();
            mockOwinContext.Setup(x => x.Get<OrganizationContext>(It.IsAny<string>())).Returns((OrganizationContext)null);

            TestEndMiddleware testMiddleware = new TestEndMiddleware();
            testMiddleware.isInvoked.Should().BeFalse();

            OrganizationContextMiddleware middleware = new OrganizationContextMiddleware(testMiddleware, mockResolver.Object);
            await middleware.Invoke(mockOwinContext.Object);

            testMiddleware.isInvoked.Should().BeTrue("next middeleware should always be invoked");

            mockStateManager.Verify(x => x.GetContext(), Times.Once);
            mockStateManager.Verify(x => x.SaveContext(It.IsAny<OrganizationContext>()), Times.Once);
            mockStorageContext.Verify(x => x.GetAsync(organization.ID.ToString()), Times.Once);
            mockResolver.Verify(x => x.GetService(typeof(IStateManager<OrganizationContext>)), Times.Once);
            mockResolver.Verify(x => x.GetService(typeof(IStorageContext<Organization>)), Times.Once);
            mockResolver.Verify(x => x.GetService(typeof(IAppConfiguration)), Times.Once);
        }
    }
}
