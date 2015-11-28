using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CritterHeroes.Web.Areas.Admin.People.Commands;
using CritterHeroes.Web.Areas.Admin.People.Models;
using CritterHeroes.Web.Contracts.Commands;
using CritterHeroes.Web.Contracts.Queries;
using CritterHeroes.Web.Data.Models.Identity;

namespace CritterHeroes.Web.Areas.Admin.People
{
    [Authorize(Roles = UserRole.MasterAdmin)]
    [Route(PeopleController.Route + "/{action=index}")]
    public class PeopleController : BaseAdminController
    {
        public const string Route = "People";

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ImportPeople(ImportPeopleCommand command)
        {
            await CommandDispatcher.DispatchAsync(command);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ImportBusinesses(ImportBusinessCommand command)
        {
            await CommandDispatcher.DispatchAsync(command);
            return RedirectToAction("Index");
        }
    }
}
