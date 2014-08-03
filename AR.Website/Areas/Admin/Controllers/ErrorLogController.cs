using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace AR.Website.Areas.Admin.Controllers
{
    public class ErrorLogController : Controller
    {
        public ViewResult Index(string resource)
        {
            return View();
        }
    }
}