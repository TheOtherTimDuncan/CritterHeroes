using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Data.Models.Identity;
using CritterHeroes.Web.Features.Admin.Critters.Models;
using CritterHeroes.Web.Features.Admin.Critters.Queries;
using CritterHeroes.Web.Features.Shared;

namespace CritterHeroes.Web.Features.Admin.Critters
{
    [Route(CrittersController.Route + "/{action=index}")]
    public class CrittersController : BaseAdminController
    {
        public const string Route = "Critters";

        public CrittersController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
            : base(queryDispatcher, commandDispatcher)
        {
        }

        public async Task<ActionResult> Index(CrittersQuery query)
        {
            CrittersModel model = await QueryDispatcher.DispatchAsync(query);
            return View(model);
        }

        [AuthorizeRoles(UserRole.Admin, UserRole.MasterAdmin)]
        public async Task<ActionResult> List(CrittersListQuery query)
        {
            CrittersListModel model = await QueryDispatcher.DispatchAsync(query);
            return JsonCamelCase(model);
        }

        [HttpGet]
        [AuthorizeRoles(UserRole.Admin, UserRole.MasterAdmin)]
        public async Task<ActionResult> Summary()
        {
            CritterSummaryModel model = await QueryDispatcher.DispatchAsync(new CritterSummaryQuery());
            return PartialView("_Summary", model);
        }

        [Authorize(Roles = UserRole.MasterAdmin)]
        public ActionResult Import()
        {
            CritterImportModel model = QueryDispatcher.Dispatch(new CritterImportQuery());
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = UserRole.MasterAdmin)]
        public async Task<ActionResult> Import(CritterImportModel model)
        {
            await CommandDispatcher.DispatchAsync(model);
            return JsonCamelCase(model.Messages);
        }
    }
}
