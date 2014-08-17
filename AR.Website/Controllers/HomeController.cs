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

            if (User.Identity.IsAuthenticated)
            {
                model.IsAuthenticated = true;
                IEnumerable<string> roleNames = ((ClaimsIdentity)User.Identity).Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value);
                model.UserRoles = IdentityRole.All.Where(x => roleNames.Any(r => r == x.Name));
            }
            else
            {
                model.IsAuthenticated = false;
                model.UserRoles = Enumerable.Empty<IdentityRole>();
            }

            return PartialView("_Menu", model);
        }
    }
}