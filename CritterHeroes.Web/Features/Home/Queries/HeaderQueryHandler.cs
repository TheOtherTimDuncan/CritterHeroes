using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Contracts.Storage;
using CritterHeroes.Web.Features.Home.Models;

namespace CritterHeroes.Web.Features.Home.Queries
{
    public class HeaderQuery : IQuery<HeaderModel>
    {
    }

    public class HeaderQueryHandler : IQueryHandler<HeaderQuery, HeaderModel>
    {
        private IOrganizationLogoService _logoService;

        public HeaderQueryHandler(IOrganizationLogoService logoService)
        {
            this._logoService = logoService;
        }

        public HeaderModel Execute(HeaderQuery query)
        {
            return new HeaderModel()
            {
                LogoUrl = _logoService.GetLogoUrl()
            };
        }
    }
}
