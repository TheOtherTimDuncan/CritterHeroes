using System;
using System.Collections.Generic;
using System.Linq;
using CritterHeroes.Web.Domain.Contracts.Queries;
using CritterHeroes.Web.Domain.Contracts.StateManagement;
using CritterHeroes.Web.Features.Components.Models;
using CritterHeroes.Web.Shared.StateManagement;
using TOTD.Utility.Misc;

namespace CritterHeroes.Web.Features.Components.Queries
{
    public class CancelButtonQuery : IQuery<CancelButtonModel>
    {
    }

    public class CancelButtonQueryHandler : IQueryHandler<CancelButtonQuery, CancelButtonModel>
    {
        private IPageContextService _pageContextService;

        public CancelButtonQueryHandler(IPageContextService pageContextService)
        {
            this._pageContextService = pageContextService;
        }

        public CancelButtonModel Execute(CancelButtonQuery query)
        {
            PageContext pageContext = _pageContextService.GetPageContext();
            return new CancelButtonModel()
            {
                PreviousPath = pageContext.IfNotNull(x => x.PreviousPath)
            };
        }
    }
}
