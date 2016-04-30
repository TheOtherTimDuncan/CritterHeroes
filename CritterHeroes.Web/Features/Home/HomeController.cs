using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Features.Common;
using CritterHeroes.Web.Features.Home.Models;
using CritterHeroes.Web.Features.Home.Queries;

namespace CritterHeroes.Web.Features.Home
{
    [Route(HomeController.Route + "/{action}")]
    public class HomeController : BaseController
    {
        public const string Route = "Home";

        public HomeController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
            : base(queryDispatcher, commandDispatcher)
        {
        }

        [ChildActionOnly]
        [AllowAnonymous]
        public PartialViewResult Menu()
        {
            MenuModel model = QueryDispatcher.Dispatch(new MenuQuery());
            return PartialView("_Menu", model);
        }

        [ChildActionOnly]
        [AllowAnonymous]
        public PartialViewResult Header()
        {
            HeaderModel model = QueryDispatcher.Dispatch(new HeaderQuery());
            return PartialView("_Header", model);
        }
    }
}
