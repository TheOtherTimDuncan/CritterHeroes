using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Areas.Home.Models;
using CritterHeroes.Web.Areas.Home.Queries;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Contracts.Storage;
using TOTD.Utility.Misc;

namespace CritterHeroes.Web.Areas.Home.QueryHandlers
{
    public class MenuViewModelQueryHandler : IQueryHandler<MenuQuery, MenuModel>
    {
        private IOrganizationLogoService _logoService;
        private IHttpUser _httpUser;
        private OrganizationContext _orgContext;
        private UserContext _userContext;

        public MenuViewModelQueryHandler(IOrganizationLogoService logoService, IHttpUser httpUser, OrganizationContext orgContext, UserContext userContext)
        {
            this._logoService = logoService;
            this._httpUser = httpUser;
            this._orgContext = orgContext;
            this._userContext = userContext;
        }

        public MenuModel Retrieve(MenuQuery query)
        {
            return new MenuModel()
            {
                CurrentUser = _httpUser,
                OrganizationShortName = _orgContext.IfNotNull(x => x.ShortName),
                UserDisplayName = _userContext.IfNotNull(x => x.DisplayName),
                LogoUrl = _logoService.GetLogoUrl()
            };
        }
    }
}