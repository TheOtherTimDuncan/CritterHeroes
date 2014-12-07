using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CH.Domain.Contracts.Commands;
using CH.Domain.Contracts.Configuration;
using CH.Domain.Contracts.Queries;
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
            MenuModel model = new MenuModel()
            {
                CurrentUser = User,
                OrganizationShortName = OrganizationContext.IfNotNull(x => x.ShortName),
                UserDisplayName = UserContext.IfNotNull(x => x.DisplayName),
                LogoUrl = GetBlobUrl(OrganizationContext.LogoFilename)
            };
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