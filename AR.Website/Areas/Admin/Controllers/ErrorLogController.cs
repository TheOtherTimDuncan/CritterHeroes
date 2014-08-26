using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AR.Domain.Identity;
using AR.Website.Utility;

namespace AR.Website.Areas.Admin.Controllers
{
    [RouteArea(AreaName.Admin)]
    [Authorize(Roles = IdentityRole.RoleNames.MasterAdmin)]
    public class ErrorLogController : Controller
    {
        public ViewResult Index()
        {
            return View();
        }
    }
}