using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Domain.Contracts;
using CH.Domain.Contracts.Storage;
using CH.Domain.Models;
using CH.Domain.Services.Queries;
using CH.Domain.StateManagement;
using FluentAssertions;
using Microsoft.Owin;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;

namespace CH.Test.QueryTests
{
    [TestClass]
    public class OrganizationContextQueryHandlerTests : BaseTest
    {
        [TestMethod]
        public async Task ReturnsOrganizationContextFromOwinContextIfInOwinContext()
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

            Mock<IStorageContext<Organization>> mockStorageContext = new Mock<IStorageContext<Organization>>();

            Mock<IOwinContext> mockOwinContext = new Mock<IOwinContext>();
            mockOwinContext.Setup(x => x.Get<OrganizationContext>(It.IsAny<string>())).Returns(context);

            OrganizationContextQueryHandler queryHandler = new OrganizationContextQueryHandler(mockStateManager.Object, mockStorageContext.Object, mockOwinContext.Object);
            OrganizationContext resultContext = await queryHandler.RetrieveAsync(new OrganizationQuery()
            {
                OrganizationID = context.OrganizationID
            });

            resultContext.Should().Equals(context);

            mockStorageContext.Verify(x => x.GetAsync(It.IsAny<string>()), Times.Never);
            mockStateManager.Verify(x => x.GetContext(), Times.Never);
            mockOwinContext.Verify(x => x.Get<OrganizationContext>(It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public async Task ReturnsOrganizationContextFromCookieIfCookieExists()
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

            Mock<IStorageContext<Organization>> mockStorageContext = new Mock<IStorageContext<Organization>>();

            Mock<IOwinContext> mockOwinContext = new Mock<IOwinContext>();

            OrganizationContextQueryHandler queryHandler = new OrganizationContextQueryHandler(mockStateManager.Object, mockStorageContext.Object, mockOwinContext.Object);
            OrganizationContext resultContext = await queryHandler.RetrieveAsync(new OrganizationQuery()
            {
                OrganizationID = context.OrganizationID
            });

            resultContext.Should().Equals(context);

            mockStateManager.Verify(x => x.GetContext(), Times.Once);
            mockOwinContext.Verify(x => x.Get<OrganizationContext>(It.IsAny<string>()), Times.Once);
            mockStorageContext.Verify(x => x.GetAsync(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public async Task CreatesAndSavesOrganizationContextIfCookieDoesNotExist()
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
            mockStateManager.Setup(x => x.SaveContext(It.IsAny<OrganizationContext>()));

            Mock<IStorageContext<Organization>> mockStorageContext = new Mock<IStorageContext<Organization>>();
            mockStorageContext.Setup(x => x.GetAsync(organization.ID.ToString())).Returns(Task.FromResult(organization));

            Mock<IOwinContext> mockOwinContext = new Mock<IOwinContext>();

            OrganizationContextQueryHandler queryHandler = new OrganizationContextQueryHandler(mockStateManager.Object, mockStorageContext.Object, mockOwinContext.Object);
            OrganizationContext resultContext = await queryHandler.RetrieveAsync(new OrganizationQuery()
            {
                OrganizationID = organization.ID
            });

            resultContext.OrganizationID.Should().Be(organization.ID);

            mockStateManager.Verify(x => x.GetContext(), Times.Once);
            mockStateManager.Verify(x => x.SaveContext(It.IsAny<OrganizationContext>()), Times.Once);
            mockStorageContext.Verify(x => x.GetAsync(organization.ID.ToString()), Times.Once);
            mockOwinContext.Verify(x => x.Get<OrganizationContext>(It.IsAny<string>()), Times.Once);
        }
    }
}
