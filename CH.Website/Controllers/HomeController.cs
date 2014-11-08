using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using CH.Domain.Identity;
using CH.Website.Models;
using CH.Website.Utility;
using TOTD.Utility.Misc;

namespace CH.Website.Controllers
{
    public class HomeController : BaseController
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
            model.OrganizationShortName = OrganizationContext.IfNotNull(x => x.ShortName);
            model.LogoUrl = GetBlobUrl(OrganizationContext.LogoFilename);
            return PartialView("_Menu", model);
        }

        [ChildActionOnly]
        [AllowAnonymous]
        public PartialViewResult Logo()
        {
            LogoModel model = new LogoModel();
            model.LogoUrl = GetBlobUrl(OrganizationContext.LogoFilename);
            return PartialView(model);
        }
    }
}