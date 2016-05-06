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
            string timezoneID = "Eastern Standard Time";

            Organization org = new Organization(Guid.NewGuid())
            {
                FullName = "FullName",
                ShortName = "ShortName",
                EmailAddress = "email@email.com",
                TimeZoneID = null
            };

            string logoUrl = "logourl";

            Mock<IAppConfiguration> mockAppConfiguration = new Mock<IAppConfiguration>();
            mockAppConfiguration.SetupGet(x => x.OrganizationID).Returns(org.ID);

            MockSqlStorageContext<Organization> mockStorageContext = new MockSqlStorageContext<Organization>(org);

            Mock<IOrganizationLogoService> mockLogoService = new Mock<IOrganizationLogoService>();
            mockLogoService.Setup(x => x.GetLogoUrl()).Returns(logoUrl);

            EditProfileQuery query = new EditProfileQuery()
            {
                JsTime = $"Fri May 06 2016 18:54:29 GMT - 0500({timezoneID})"
            };

            EditProfileQueryHandler handler = new EditProfileQueryHandler(mockAppConfiguration.Object, mockStorageContext.Object, mockLogoService.Object);
            EditProfileModel model = await handler.ExecuteAsync(query);
            model.Should().NotBeNull();

            model.Name.Should().Be(org.FullName);
            model.ShortName.Should().Be(org.ShortName);
            model.Email.Should().Be(org.EmailAddress);
            model.LogoUrl.Should().Be(logoUrl);
            model.TimeZoneID.Should().Be(timezoneID);

            model.TimeZoneOptions.Should().NotBeNullOrEmpty();
            model.TimeZoneOptions.Single(x => x.Value == timezoneID).IsSelected.Should().BeTrue();
        }

        [TestMethod]
        public async Task ConvertsDaylightSavingsTimeZoneToStandardTimeZone()
        {
            string timezoneID = "Eastern Standard Time";

            Organization org = new Organization(Guid.NewGuid())
            {
                TimeZoneID = null
            };

            Mock<IAppConfiguration> mockAppConfiguration = new Mock<IAppConfiguration>();
            mockAppConfiguration.SetupGet(x => x.OrganizationID).Returns(org.ID);

            MockSqlStorageContext<Organization> mockStorageContext = new MockSqlStorageContext<Organization>(org);

            Mock<IOrganizationLogoService> mockLogoService = new Mock<IOrganizationLogoService>();

            EditProfileQuery query = new EditProfileQuery()
            {
                JsTime = $"Fri May 06 2016 18:54:29 GMT - 0400(Eastern Daylight Time)"
            };

            EditProfileQueryHandler handler = new EditProfileQueryHandler(mockAppConfiguration.Object, mockStorageContext.Object, mockLogoService.Object);
            EditProfileModel model = await handler.ExecuteAsync(query);
            model.Should().NotBeNull();

            model.TimeZoneID.Should().Be(timezoneID);

            model.TimeZoneOptions.Should().NotBeNullOrEmpty();
            model.TimeZoneOptions.Single(x => x.Value == timezoneID).IsSelected.Should().BeTrue();
        }
    }
}
