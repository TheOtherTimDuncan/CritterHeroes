using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CritterHeroes.Web.Data.Models.Identity;
using CritterHeroes.Web.Domain.Contracts.Commands;
using CritterHeroes.Web.Domain.Contracts.Queries;

namespace CritterHeroes.Web.Features.Admin.ErrorLog
{
    [Authorize(Roles = UserRole.MasterAdmin)]
    [Route(ErrorLogController.Route + "/{action=index}")]
    public class ErrorLogController : BaseAdminController
    {
        public const string Route = "ErrorLog";

        public ErrorLogController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
            : base(queryDispatcher, commandDispatcher)
        {
        }
        public ViewResult Index()
        {
            return View();
        }

#if DEBUG
        [AllowAnonymous]
        public ActionResult Test()
        {
            throw new ArgumentNullException("test");
        }
#endif
    }
}
