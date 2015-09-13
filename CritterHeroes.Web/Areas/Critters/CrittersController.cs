using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Common;
using CritterHeroes.Web.Areas.Critters.Models;
using CritterHeroes.Web.Areas.Critters.Queries;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Queries;

namespace CritterHeroes.Web.Areas.Critters
{
    [Route("Critters/{action=index}")]
    public class CrittersController : BaseController
    {
        public CrittersController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
            : base(queryDispatcher, commandDispatcher)
        {
        }

        [Route("~/")]
        public async Task<ActionResult> Index(CrittersListQuery query)
        {
            CrittersListModel model = await QueryDispatcher.DispatchAsync(query);
            return View(model);
        }
    }
}
