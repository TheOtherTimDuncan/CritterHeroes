using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AR.Domain.Identity;

namespace AR.Website.Areas.Admin.Controllers
{
    [Authorize(Roles=IdentityRole.RoleNames.MasterAdmin)]
    public class ErrorLogController : Controller
    {
        public ViewResult Index(string resource)
        {
            return View();
        }
    }
}