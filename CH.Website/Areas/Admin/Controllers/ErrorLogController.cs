using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CH.Domain.Identity;
using CH.Website.Utility;

namespace CH.Website.Areas.Admin.Controllers
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