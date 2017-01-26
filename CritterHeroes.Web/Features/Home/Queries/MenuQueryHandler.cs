using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Domain.Contracts;
using CritterHeroes.Web.Domain.Contracts.Queries;
using CritterHeroes.Web.Domain.Contracts.StateManagement;
using CritterHeroes.Web.Domain.Contracts.Storage;
using CritterHeroes.Web.Features.Home.Models;
using CritterHeroes.Web.Shared.Identity;
using CritterHeroes.Web.Shared.StateManagement;
using TOTD.Utility.Misc;

namespace CritterHeroes.Web.Features.Home.Queries
{
    public class MenuQuery : IQuery<MenuModel>
    {
    }

    public class MenuQueryHandler : IQueryHandler<MenuQuery, MenuModel>
    {
        private IOrganizationLogoService _logoService;
        private IHttpUser _httpUser;
        private IStateManager<OrganizationContext> _orgStateManager;
        private IStateManager<UserContext> _userStateManager;

        public MenuQueryHandler(IOrganizationLogoService logoService, IHttpUser httpUser, IStateManager<OrganizationContext> orgStateManager, IStateManager<UserContext> userStateManager)
        {
            this._logoService = logoService;
            this._httpUser = httpUser;
            this._orgStateManager = orgStateManager;
            this._userStateManager = userStateManager;
        }

        public MenuModel Execute(MenuQuery query)
        {
            MenuModel model = new MenuModel()
            {
                OrganizationShortName = _orgStateManager.GetContext().IfNotNull(x => x.ShortName),
                UserDisplayName = _userStateManager.GetContext().IfNotNull(x => x.DisplayName),
                LogoUrl = _logoService.GetLogoUrl(),
                IsLoggedIn = _httpUser.IsAuthenticated,
                ShowAdminMenu = (_httpUser.IsInRole(IdentityRole.Admin) || _httpUser.IsInRole(IdentityRole.MasterAdmin)),
                ShowMasterAdminMenu = (_httpUser.IsInRole(IdentityRole.MasterAdmin))
            };

            return model;
        }
    }
}
