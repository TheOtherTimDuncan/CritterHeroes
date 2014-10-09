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
using AR.Website.Utility;

namespace AR.Website.Areas.Admin.Controllers
{
    [RouteArea(AreaName.Admin)]
    [Authorize(Roles = IdentityRole.RoleNames.MasterAdmin)]
    public class DataMaintenanceController : Controller
    {
        [HttpGet]
        public ViewResult Dashboard()
        {
            DashboardModel model = new DashboardModel();

            model.TargetStorageItem = new DashboardStorageItem(StorageSource.Azure);
            model.SourceStorageItem = new DashboardStorageItem(StorageSource.RescueGroups);

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

            IEnumerable<IStorageContext> storageContexts = StorageSource.GetAll().Select(x => x.StorageContext);
            IDataStatusHandler statusHandler = modelSource.StatusHandler;
            DataStatusModel dataStatus = await statusHandler.GetModelStatusAsync(storageContexts.ToArray());

            DashboardItemStatus model = new DashboardItemStatus();
            model.TargetItem = dataStatus.Items.First(x => x.StorageID == StorageSource.Azure.Value);
            model.SourceItem = dataStatus.Items.First(x => x.StorageID == StorageSource.RescueGroups.Value);
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

            IStorageContext azureContext = StorageSource.Azure.StorageContext;
            await azureContext.DeleteAllAsync<AR.Domain.Models.Data.AnimalStatus>();

            IStorageContext rgContext = StorageSource.RescueGroups.StorageContext;
            IEnumerable<AR.Domain.Models.Data.AnimalStatus> data = await rgContext.GetAllAsync<AR.Domain.Models.Data.AnimalStatus>();

            await azureContext.SaveAsync<AR.Domain.Models.Data.AnimalStatus>(data);

            IDataStatusHandler statusHandler = modelSource.StatusHandler;
            DataStatusModel dataStatus = await statusHandler.GetModelStatusAsync(azureContext, rgContext);

            DashboardItemStatus model = new DashboardItemStatus();
            model.TargetItem = dataStatus.Items.First(x => x.StorageID == StorageSource.Azure.Value);
            model.SourceItem = dataStatus.Items.First(x => x.StorageID == StorageSource.RescueGroups.Value);
            model.DataItemCount = dataStatus.DataItemCount;

            return Json(model);
        }
    }
}