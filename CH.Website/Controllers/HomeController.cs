using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CH.Domain.Contracts.Commands;
using CH.Domain.Contracts.Queries;
using CH.Domain.StateManagement;
using CH.Website.Models;
using CH.Website.Services.Queries;

namespace CH.Website.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
            : base(queryDispatcher, commandDispatcher)
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
            MenuModel model = QueryDispatcher.DispatchAsync<MenuQuery, MenuModel>(new MenuQuery()
            {
                OrganizationContext = this.OrganizationContext,
                CurrentUser = User,
                UserContext = this.UserContext
            }).Result;
            return PartialView("_Menu", model);
        }

        [ChildActionOnly]
        [AllowAnonymous]
        public PartialViewResult Header()
        {
            HeaderModel model = QueryDispatcher.DispatchAsync<OrganizationContext, HeaderModel>(OrganizationContext).Result;
            return PartialView("_Header", model);
        }
    }
}