using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Features.Components.Models;
using CritterHeroes.Web.Features.Components.Queries;
using CritterHeroes.Web.Features.Shared;

namespace CritterHeroes.Web.Features.Components
{
    [Route(Route + "/{action}")]
    public class ComponentsController : BaseController
    {
        public const string Route = "Components";

        public ComponentsController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
            : base(queryDispatcher, commandDispatcher)
        {
        }

        [ChildActionOnly]
        [AllowAnonymous]
        public ActionResult CancelButton()
        {
            CancelButtonModel model = QueryDispatcher.Dispatch(new CancelButtonQuery());
            return PartialView(model);
        }

        [AllowAnonymous]
        public ActionResult ImageNotFound()
        {
            string url = QueryDispatcher.Dispatch(new ImageNotFoundQuery());
            return Redirect(url);
        }
    }
}
