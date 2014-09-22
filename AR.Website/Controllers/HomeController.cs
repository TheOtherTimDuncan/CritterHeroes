using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using AR.Domain.Identity;
using AR.Website.Models;
using AR.Website.Utility;

namespace AR.Website.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        [AllowAnonymous]
        public PartialViewResult Menu()
        {
            MenuModel model = new MenuModel();
            model.CurrentUser = User;
            return PartialView("_Menu", model);
        }
    }
}