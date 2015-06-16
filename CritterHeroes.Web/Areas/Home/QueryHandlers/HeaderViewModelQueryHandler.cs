using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Areas.Home.Models;
using CritterHeroes.Web.Areas.Home.Queries;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Contracts.Storage;

namespace CritterHeroes.Web.Areas.Home.QueryHandlers
{
    public class HeaderViewModelQueryHandler : IQueryHandler<HeaderQuery, HeaderModel>
    {
        private IOrganizationLogoService _logoService;

        public HeaderViewModelQueryHandler(IOrganizationLogoService logoService)
        {
            this._logoService = logoService;
        }

        public HeaderModel Retrieve(HeaderQuery query)
        {
            return new HeaderModel()
            {
                LogoUrl = _logoService.GetLogoUrl()
            };
        }
    }
}