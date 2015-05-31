using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CritterHeroes.Web.Common.Identity;
using CritterHeroes.Web.Areas.Common;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Contracts.Commands;

namespace CritterHeroes.Web.Areas.Admin.ErrorLog
{
    [Authorize(Roles = IdentityRole.RoleNames.MasterAdmin)]
    [Route("ErrorLog/{action=index}")]
    public class ErrorLogController : BaseAdminController
    {
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