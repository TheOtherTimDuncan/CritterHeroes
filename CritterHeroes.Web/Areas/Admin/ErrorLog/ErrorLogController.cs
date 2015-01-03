using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CritterHeroes.Web.Common.Identity;
using CritterHeroes.Web.Areas.Common;

namespace CritterHeroes.Web.Areas.Admin.ErrorLog
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