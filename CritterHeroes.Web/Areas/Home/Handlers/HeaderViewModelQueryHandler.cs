using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Common.Handlers;
using CritterHeroes.Web.Areas.Home.Models;
using CritterHeroes.Web.Areas.Home.Queries;
using CritterHeroes.Web.Contracts.Configuration;

namespace CritterHeroes.Web.Areas.Home.Handlers
{
    public class HeaderViewModelQueryHandler : BaseViewModelQueryHandler<HeaderQuery, HeaderModel>
    {
        private IAppConfiguration _appConfiguration;

        public HeaderViewModelQueryHandler(IAppConfiguration appConfiguration)
        {
            this._appConfiguration = appConfiguration;
        }

        public override Task<HeaderModel> RetrieveAsync(HeaderQuery query)
        {
            HeaderModel model = new HeaderModel();
            model.LogoUrl = GetBlobUrl(_appConfiguration.BlobBaseUrl, query.OrganizationContext.AzureName, query.OrganizationContext.LogoFilename);
            return Task.FromResult(model);
        }
    }
}