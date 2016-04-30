using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Test.Mocks;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Data.Models;
using CritterHeroes.Web.Features.Admin.Organizations.Models;
using CritterHeroes.Web.Features.Admin.Organizations.Queries;
using CritterHeroes.Web.Features.Admin.Organizations.QueryHandlers;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test.AdminOrganizationTests
{
    [TestClass]
    public class EditProfileQueryHandlerTests
    {
        [TestMethod]
        public async Task ReturnsViewModel()
        {
            Organization org = new Organization(Guid.NewGuid())
            {
                FullName = "FullName",
                ShortName = "ShortName",
                EmailAddress = "email@email.com"
            };

            string logoUrl = "logourl";

            Mock<IAppConfiguration> mockAppConfiguration = new Mock<IAppConfiguration>();
            mockAppConfiguration.SetupGet(x => x.OrganizationID).Returns(org.ID);

            MockSqlStorageContext<Organization> mockStorageContext = new MockSqlStorageContext<Organization>(org);

            Mock<IOrganizationLogoService> mockLogoService = new Mock<IOrganizationLogoService>();
            mockLogoService.Setup(x => x.GetLogoUrl()).Returns(logoUrl);

            EditProfileQueryHandler handler = new EditProfileQueryHandler(mockAppConfiguration.Object, mockStorageContext.Object, mockLogoService.Object);
            EditProfileModel model = await handler.ExecuteAsync(new EditProfileQuery());
            model.Should().NotBeNull();

            model.Name.Should().Be(org.FullName);
            model.ShortName.Should().Be(org.ShortName);
            model.Email.Should().Be(org.EmailAddress);
            model.LogoUrl.Should().Be(logoUrl);
        }
    }
}
