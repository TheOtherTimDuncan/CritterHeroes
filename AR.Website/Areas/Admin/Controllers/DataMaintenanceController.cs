using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AR.Domain.Contracts;
using AR.Domain.Models.Status;
using AR.Website.Areas.Admin.Json;
using AR.Website.Areas.Admin.Models;
using AR.Website.Utility;

namespace AR.Website.Areas.Admin.Controllers
{
    public class DataMaintenanceController : Controller
    {
        [HttpGet]
        public ViewResult Dashboard()
        {
            DashboardModel model = new DashboardModel();

            model.Items =
                from m in DataModelSource.GetAll()
                select new DashboardItemModel(m.Value, m.Title);

            model.Columns =
                from s in StorageSource.GetAll()
                select new DashboardItemColumn(s.Value, s.Title);

            return View(model);
        }

        [HttpPost]
        public async Task<JsonResult> GetModelStatus(int modelID)
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
            model.StorageItems =
                from s in dataStatus.Items
                select new DashboardStorageItemStatus()
                {
                    StorageID = s.StorageID,
                    ValidCount = s.ValidCount,
                    InvalidCount = s.InvalidCount
                };

            return Json(model);
        }
    }
}