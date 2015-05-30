using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Admin.Lists.Models;
using CritterHeroes.Web.Areas.Admin.Lists.Sources;
using CritterHeroes.Web.Areas.Common;
using CritterHeroes.Web.Common.Commands;
using CritterHeroes.Web.Common.Identity;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Dashboard;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Models.Json;

namespace CritterHeroes.Web.Areas.Admin.Lists
{
    [RouteArea(AreaName.Admin)]
    [Authorize(Roles = IdentityRole.RoleNames.MasterAdmin)]
    public class ListsController : BaseController
    {
        public ListsController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
            : base(queryDispatcher, commandDispatcher)
        {
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

            DashboardItemStatus model = await modelSource.GetDashboardItemStatusAsync(DependencyResolver.Current, GetSource(), GetTarget(), OrganizationContext);
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

            CommandResult commandResult = await modelSource.ExecuteSyncAsync(DependencyResolver.Current, OrganizationContext);

            DashboardItemStatus model = await modelSource.GetDashboardItemStatusAsync(DependencyResolver.Current, GetSource(), GetTarget(), OrganizationContext);
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