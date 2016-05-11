using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Features.Shared;

namespace CritterHeroes.Web.Features.Admin
{
    [RouteArea(AreaName.Admin)]
    public class BaseAdminController : BaseController
    {
        public BaseAdminController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
            : base(queryDispatcher, commandDispatcher)
        {
        }
    }
}
