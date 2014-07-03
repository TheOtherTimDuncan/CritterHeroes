using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AR.Website.Areas.Admin.Models;
using AR.Website.Utility;

namespace AR.Website.Areas.Admin.Controllers
{
    public class DataMaintenanceController : Controller
    {
        [HttpGet]
        public  ViewResult Dashboard()
        {
            List<DashboardItemModel> items = new List<DashboardItemModel>();
            items.Add(new DashboardItemModel("Animal Status", "status"));

            DashboardModel model = new DashboardModel();
            model.Items = items;
            model.Columns = StorageSource.GetAll().Select(x => new DashboardItemColumn(x.Value, x.Title));

            return View(model);
        }
    }
}