using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Admin.Lists.Commands;
using CritterHeroes.Web.Areas.Admin.Lists.Models;
using CritterHeroes.Web.Areas.Admin.Lists.Queries;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Data.Models.Identity;

namespace CritterHeroes.Web.Areas.Admin.Lists
{
    [Authorize(Roles = UserRole.MasterAdmin)]
    [Route(ListsController.Route + "/{action=index}")]
    public class ListsController : BaseAdminController
    {
        public const string Route = "Lists";

        public ListsController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
            : base(queryDispatcher, commandDispatcher)
        {
        }

        [HttpGet]
        public ViewResult Index()
        {
            DashboardModel model = new DashboardModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Refresh(ListItemQuery query)
        {
            DashboardItemStatus model = await QueryDispatcher.DispatchAsync(query);
            return Json(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Sync(SyncListItemCommand command)
        {
            CommandResult commandResult = await CommandDispatcher.DispatchAsync(command);
            return Json(command.ItemStatus);
        }
    }
}
