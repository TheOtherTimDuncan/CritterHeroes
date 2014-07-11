using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AR.Website.Areas.Admin.Json;
using AR.Website.Areas.Admin.Models;
using AR.Website.Utility;

namespace AR.Website.Areas.Admin.Controllers
{
    public class DataMaintenanceController : Controller
    {
        [HttpGet]
        public  ViewResult Dashboard()
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

        [HttpGet]
        public JsonResult GetModelStatus(int modelID)
        {
            DashboardItemStatus model = new DashboardItemStatus();
            model.StorageItems = new DashboardStorageItemStatus[] { new DashboardStorageItemStatus() { StorageID = 0, ValidCount = 1, InvalidCount = 2 } };
            return Json(model);
        }
    }
}