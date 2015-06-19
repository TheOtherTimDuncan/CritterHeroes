using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Admin.Lists.Models;
using CritterHeroes.Web.Areas.Admin.Lists.Sources;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.Identity;
using CritterHeroes.Web.Common.StateManagement;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Dashboard;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Contracts.StateManagement;
using CritterHeroes.Web.Models.Json;

namespace CritterHeroes.Web.Areas.Admin.Lists
{
    [Authorize(Roles = IdentityRole.RoleNames.MasterAdmin)]
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

            model.Items =
                from m in DataModelSource.GetAll()
                select new DashboardItemModel(m.ID, m.Title);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Refresh(int modelID)
        {
            DataModelSource modelSource = DataModelSource.FromID(modelID);
            if (modelSource == null)
            {
                return Json(null);
            }

            OrganizationContext orgContext = _orgStateManager.GetContext();
            DashboardItemStatus model = await modelSource.GetDashboardItemStatusAsync(DependencyResolver.Current, GetSource(), GetTarget(), orgContext);
            return Json(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<JsonResult> Sync(int modelID)
        {
            DataModelSource modelSource = DataModelSource.FromID(modelID);
            if (modelSource == null)
            {
                return Json(null);
            }

            OrganizationContext orgContext = _orgStateManager.GetContext();

            CommandResult commandResult = await modelSource.ExecuteSyncAsync(DependencyResolver.Current, orgContext);

            DashboardItemStatus model = await modelSource.GetDashboardItemStatusAsync(DependencyResolver.Current, GetSource(), GetTarget(), orgContext);
            return Json(model);
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