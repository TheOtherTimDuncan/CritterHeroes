using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Admin.Critters.Commands;
using CritterHeroes.Web.Areas.Admin.Critters.Models;
using CritterHeroes.Web.Areas.Admin.Critters.Queries;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Data.Models.Identity;

namespace CritterHeroes.Web.Areas.Admin.Critters
{
    [Authorize(Roles = UserRole.MasterAdmin)]
    [Route("Critters/{action=index}")]
    public class CrittersController : BaseAdminController
    {
        public CrittersController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
            : base(queryDispatcher, commandDispatcher)
        {
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            CritterSummaryModel model = await QueryDispatcher.DispatchAsync(new CritterSummaryQuery());
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> UploadJson(UploadJsonFileCommand command)
        {
            await CommandDispatcher.DispatchAsync(command);
            return View(command);
        }
    }
}
