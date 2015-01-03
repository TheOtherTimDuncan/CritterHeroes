using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Common.Handlers;
using CritterHeroes.Web.Areas.Home.Models;
using CritterHeroes.Web.Areas.Home.Queries;
using CritterHeroes.Web.Contracts.Configuration;
using TOTD.Utility.ExceptionHelpers;
using TOTD.Utility.Misc;

namespace CritterHeroes.Web.Areas.Home.Handlers
{
    public class MenuViewModelQueryHandler : BaseViewModelQueryHandler<MenuQuery, MenuModel>
    {
        private IAppConfiguration _appConfiguration;

        public MenuViewModelQueryHandler(IAppConfiguration appConfiguration)
        {
            this._appConfiguration = appConfiguration;
        }

        public override Task<MenuModel> RetrieveAsync(MenuQuery query)
        {
            ThrowIf.Argument.IsNull(query, "query");

            MenuModel model = new MenuModel()
            {
                CurrentUser = query.CurrentUser,
                OrganizationShortName = query.OrganizationContext.IfNotNull(x => x.ShortName),
                UserDisplayName = query.UserContext.IfNotNull(x => x.DisplayName),
                LogoUrl = GetBlobUrl(_appConfiguration.BlobBaseUrl, query.OrganizationContext.AzureName, query.OrganizationContext.LogoFilename)
            };
            return Task.FromResult(model);
        }
    }
}