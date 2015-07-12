﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Admin.Organizations.Models;
using CritterHeroes.Web.Areas.Admin.Organizations.Queries;
using CritterHeroes.Web.Areas.Admin.Organizations.QueryHandlers;
using CritterHeroes.Web.Contracts.Configuration;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Models;
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

            Mock<IStorageContext<Organization>> mockStorageContext = new Mock<IStorageContext<Organization>>();
            mockStorageContext.Setup(x => x.GetAsync(org.ID.ToString())).Returns(Task.FromResult(org));

            Mock<IOrganizationLogoService> mockLogoService = new Mock<IOrganizationLogoService>();
            mockLogoService.Setup(x => x.GetLogoUrl()).Returns(logoUrl);

            EditProfileQueryHandler handler = new EditProfileQueryHandler(mockAppConfiguration.Object, mockStorageContext.Object, mockLogoService.Object);
            EditProfileModel model = await handler.RetrieveAsync(new EditProfileQuery());
            model.Should().NotBeNull();

            model.Name.Should().Be(org.FullName);
            model.ShortName.Should().Be(org.ShortName);
            model.Email.Should().Be(org.EmailAddress);
            model.LogoUrl.Should().Be(logoUrl);
        }
    }
}