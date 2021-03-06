﻿using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Data.Models.Identity;
using CritterHeroes.Web.Domain.Contracts;
using CritterHeroes.Web.Domain.Contracts.StateManagement;
using CritterHeroes.Web.Domain.Contracts.Storage;
using CritterHeroes.Web.Features.Home.Models;
using CritterHeroes.Web.Features.Home.Queries;
using CritterHeroes.Web.Shared.StateManagement;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CH.Test.HomeTests
{
    [TestClass]
    public class MenuViewModelQueryHandlerTests
    {
        [TestMethod]
        public void ReturnsMenuModel()
        {
            OrganizationContext orgContext = new OrganizationContext()
            {
                ShortName = "shortname"
            };

            UserContext userContext = new UserContext()
            {
                DisplayName = "displayname"
            };

            string logoUrl = "logourl";

            Mock<IStateManager<OrganizationContext>> mockOrgStateManager = new Mock<IStateManager<OrganizationContext>>();
            mockOrgStateManager.Setup(x => x.GetContext()).Returns(orgContext);

            Mock<IStateManager<UserContext>> mockUserStateManager = new Mock<IStateManager<UserContext>>();
            mockUserStateManager.Setup(x => x.GetContext()).Returns(userContext);

            Mock<IOrganizationLogoService> mockLogoService = new Mock<IOrganizationLogoService>();
            mockLogoService.Setup(x => x.GetLogoUrl()).Returns(logoUrl);

            Mock<IHttpUser> mockHttpUser = new Mock<IHttpUser>();
            mockHttpUser.Setup(x => x.IsAuthenticated).Returns(true);

            MenuQueryHandler handler = new MenuQueryHandler(mockLogoService.Object, mockHttpUser.Object, mockOrgStateManager.Object, mockUserStateManager.Object);
            MenuModel model = handler.Execute(new MenuQuery());

            model.OrganizationShortName.Should().Be(orgContext.ShortName);
            model.UserDisplayName.Should().Be(userContext.DisplayName);
            model.LogoUrl.Should().Be(logoUrl);
            model.IsLoggedIn.Should().BeTrue();
            model.ShowAdminMenu.Should().BeFalse();
            model.ShowMasterAdminMenu.Should().BeFalse();
        }

        [TestMethod]
        public void ReturnsMenuModelForAdminUser()
        {
            Mock<IStateManager<OrganizationContext>> mockOrgStateManager = new Mock<IStateManager<OrganizationContext>>();
            mockOrgStateManager.Setup(x => x.GetContext()).Returns(new OrganizationContext());

            Mock<IStateManager<UserContext>> mockUserStateManager = new Mock<IStateManager<UserContext>>();
            mockUserStateManager.Setup(x => x.GetContext()).Returns(new UserContext());

            Mock<IOrganizationLogoService> mockLogoService = new Mock<IOrganizationLogoService>();
            mockLogoService.Setup(x => x.GetLogoUrl()).Returns("logourl");

            Mock<IHttpUser> mockHttpUser = new Mock<IHttpUser>();
            mockHttpUser.Setup(x => x.IsAuthenticated).Returns(true);
            mockHttpUser.Setup(x => x.IsInRole(UserRole.Admin)).Returns(true);

            MenuQueryHandler handler = new MenuQueryHandler(mockLogoService.Object, mockHttpUser.Object, mockOrgStateManager.Object, mockUserStateManager.Object);
            MenuModel model = handler.Execute(new MenuQuery());

            model.IsLoggedIn.Should().BeTrue();
            model.ShowAdminMenu.Should().BeTrue();
            model.ShowMasterAdminMenu.Should().BeFalse();
        }

        [TestMethod]
        public void ReturnsMenuModelForMasterAdminUser()
        {
            Mock<IStateManager<OrganizationContext>> mockOrgStateManager = new Mock<IStateManager<OrganizationContext>>();
            mockOrgStateManager.Setup(x => x.GetContext()).Returns(new OrganizationContext());

            Mock<IStateManager<UserContext>> mockUserStateManager = new Mock<IStateManager<UserContext>>();
            mockUserStateManager.Setup(x => x.GetContext()).Returns(new UserContext());

            Mock<IOrganizationLogoService> mockLogoService = new Mock<IOrganizationLogoService>();
            mockLogoService.Setup(x => x.GetLogoUrl()).Returns("logourl");

            Mock<IHttpUser> mockHttpUser = new Mock<IHttpUser>();
            mockHttpUser.Setup(x => x.IsAuthenticated).Returns(true);
            mockHttpUser.Setup(x => x.IsInRole(UserRole.MasterAdmin)).Returns(true);

            MenuQueryHandler handler = new MenuQueryHandler(mockLogoService.Object, mockHttpUser.Object, mockOrgStateManager.Object, mockUserStateManager.Object);
            MenuModel model = handler.Execute(new MenuQuery());

            model.IsLoggedIn.Should().BeTrue();
            model.ShowAdminMenu.Should().BeTrue();
            model.ShowMasterAdminMenu.Should().BeTrue();
        }
    }
}
