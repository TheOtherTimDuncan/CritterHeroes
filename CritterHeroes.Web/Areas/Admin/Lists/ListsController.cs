using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Admin.Lists.Commands;
using CritterHeroes.Web.Areas.Admin.Lists.Models;
using CritterHeroes.Web.Areas.Admin.Lists.Queries;
using CritterHeroes.Web.Areas.Admin.Lists.Sources;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Dashboard;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Data.Models.Identity;

namespace CritterHeroes.Web.Areas.Admin.Lists
{
    [Authorize(Roles = UserRole.MasterAdmin)]
    [Route("Lists/{action=index}")]
    public class ListsController : BaseAdminController
    {
        private IStateManager<OrganizationContext> _orgStateManager;

        public ListsController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher, IStateManager<OrganizationContext> orgStateManager)
            : base(queryDispatcher, commandDispatcher)
        {
            this._orgStateManager = orgStateManager;
        }

        [HttpGet]
        public ViewResult Index()
        {
            DashboardModel model = new DashboardModel();

            model.TargetStorageItem = new DashboardStorageItem(GetTarget());
            model.SourceStorageItem = new DashboardStorageItem(GetSource());

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

        private IStorageSource GetTarget()
        {
            return new AzureStorageSource();
        }

        private IStorageSource GetSource()
        {
            return new RescueGroupsStorageSource();
        }
    }
}
