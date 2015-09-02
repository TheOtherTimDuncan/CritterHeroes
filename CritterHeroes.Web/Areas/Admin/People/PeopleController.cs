using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Admin.People.Models;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Data.Models.Identity;

namespace CritterHeroes.Web.Areas.Admin.People
{
    [Authorize(Roles = UserRole.MasterAdmin)]
    [Route("People/{action=index}")]
    public class PeopleController : BaseAdminController
    {
        public PeopleController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
            : base(queryDispatcher, commandDispatcher)
        {
        }

        [HttpGet]
        public ActionResult Index()
        {
            PeopleSummaryModel model = new PeopleSummaryModel();
            return View(model);
        }
    }
}
