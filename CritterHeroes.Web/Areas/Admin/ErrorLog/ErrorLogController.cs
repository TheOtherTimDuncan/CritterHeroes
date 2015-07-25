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