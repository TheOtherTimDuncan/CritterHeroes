using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CH.Domain.Contracts.Commands;
using CH.Domain.Contracts.Configuration;
using CH.Domain.Contracts.Queries;
using CH.Domain.StateManagement;
using CH.Website.Models;
using TOTD.Utility.Misc;

namespace CH.Website.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher, IAppConfiguration appConfiguration)
            : base(queryDispatcher, commandDispatcher, appConfiguration)
        {
        }

        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        [AllowAnonymous]
        public PartialViewResult Menu()
        {
            OrganizationContext organizationContext = GetOrganizationContext();

            MenuModel model = new MenuModel();
            model.CurrentUser = User;
            model.OrganizationShortName = organizationContext.IfNotNull(x => x.ShortName);
            model.LogoUrl = GetBlobUrl(organizationContext.LogoFilename);
            return PartialView("_Menu", model);
        }

        [ChildActionOnly]
        [AllowAnonymous]
        public PartialViewResult Logo()
        {
            OrganizationContext organizationContext = GetOrganizationContext();

            LogoModel model = new LogoModel();
            model.LogoUrl = GetBlobUrl(organizationContext.LogoFilename);
            return PartialView(model);
        }
    }
}