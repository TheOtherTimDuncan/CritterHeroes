using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CH.Domain.Contracts.Configuration;
using CH.Website.Models;
using CH.Website.Services.Queries;
using TOTD.Utility.ExceptionHelpers;
using TOTD.Utility.Misc;

namespace CH.Website.Services.QueryHandlers
{
    public class MenuViewModelQueryHandler : BaseViewModelQueryHandler<MenuQuery, MenuModel>
    {
        private IAppConfiguration _appConfiguration;

        public MenuViewModelQueryHandler(IAppConfiguration appConfiguration)
        {
            this._appConfiguration = appConfiguration;
        }

        public override Task<MenuModel> Retrieve(MenuQuery query)
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