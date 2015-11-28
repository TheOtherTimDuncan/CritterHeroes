using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Data.Models.Identity;

namespace CritterHeroes.Web.Areas.Admin.ErrorLog
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
    }
}
