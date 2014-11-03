using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AR.Domain.Contracts;
using AR.Domain.Identity;
using AR.Domain.Models.Status;
using AR.Website.Areas.Admin.Json;
using AR.Website.Areas.Admin.Models;
using AR.Website.Controllers;
using AR.Website.Sources.Storage;
using AR.Website.Utility;

namespace AR.Website.Areas.Admin.Controllers
{
    [RouteArea(AreaName.Admin)]
    [Authorize(Roles = IdentityRole.RoleNames.MasterAdmin)]
    public class DataMaintenanceController : BaseController
    {
        [HttpGet]
        public ViewResult Dashboard()
        {
            DashboardModel model = new DashboardModel();

            model.TargetStorageItem = new DashboardStorageItem(GetTarget());
            model.SourceStorageItem = new DashboardStorageItem(GetSource());

            model.Items =
                from m in DataModelSource.GetAll()
                select new DashboardItemModel(m.Value, m.Title);

            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> Refresh(int modelID)
        {
            DataModelSource modelSource = DataModelSource.FromValue(modelID);
            if (modelSource == null)
            {
                return Json(null);
            }

            IStorageSource target = GetTarget();
            IStorageSource source = GetSource();

            IDataStatusHandler statusHandler = modelSource.StatusHandler;
            DataStatusModel dataStatus = await statusHandler.GetModelStatusAsync(target, source);

            DashboardItemStatus model = new DashboardItemStatus();
            model.TargetItem = dataStatus.Items.First(x => x.StorageID == target.ID);
            model.SourceItem = dataStatus.Items.First(x => x.StorageID == source.ID);
            model.DataItemCount = dataStatus.DataItemCount;

            return Json(model);
        }

        [HttpPost]
        public async Task<JsonResult> Sync(int modelID)
        {
            DataModelSource modelSource = DataModelSource.FromValue(modelID);
            if (modelSource == null)
            {
                return Json(null);
            }

            IStorageSource target = GetTarget();
            IStorageSource source = GetSource();

            IDataStatusHandler statusHandler = modelSource.StatusHandler;
            DataStatusModel dataStatus = await statusHandler.SyncModelAsync(source, target);

            DashboardItemStatus model = new DashboardItemStatus();
            model.TargetItem = dataStatus.Items.First(x => x.StorageID == target.ID);
            model.SourceItem = dataStatus.Items.First(x => x.StorageID == source.ID);
            model.DataItemCount = dataStatus.DataItemCount;

            return Json(model);
        }

        private IStorageSource GetTarget()
        {
            return new AzureStorageSource(OrganizationContext.AzureName);
        }

        private IStorageSource GetSource()
        {
            return new RescueGroupsStorageSource();
        }
    }
}