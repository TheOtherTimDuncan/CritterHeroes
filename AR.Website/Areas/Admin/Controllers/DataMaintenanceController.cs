using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AR.Website.Areas.Admin.Controllers
{
    public class DataMaintenanceController : Controller
    {
        [HttpGet]
        public ViewResult Index()
        {
            return View();
        }
    }
}