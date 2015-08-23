using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Data.Models.Identity;

namespace CritterHeroes.Web.Areas.Admin.Critters
{
    [Authorize(Roles = UserRole.MasterAdmin)]
    [Route("Critters/{action=index}")]
    public class CrittersController : BaseAdminController
    {
        public CrittersController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
            : base(queryDispatcher, commandDispatcher)
        {
        }

        [HttpGet]
        public ViewResult Index()
        {
            return View();
        }
    }
}
